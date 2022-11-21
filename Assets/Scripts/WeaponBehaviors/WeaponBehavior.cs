using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    [Header("Object Refs")]
    [SerializeField] private GameObject weapon_idle;
    [SerializeField] private GameObject weapon_active;
    [SerializeField] private Transform target;
    [SerializeField] private Transform anchor;
    [SerializeField] private SpriteRenderer idle_renderer;
    [SerializeField] private SpriteRenderer active_renderer;
    [SerializeField] private PolygonCollider2D collider;

    [Header("Weapon Attributes")]
    [SerializeField] private int damage;
    [SerializeField] private float distance;
    [SerializeField] private float knockback;
    [SerializeField] private float end_lag;
    [SerializeField] private bool is_enemy_weapon;
    private bool weapon_box_set = false;

    [Header("Charged Weapon Attributes")]
    [SerializeField] private bool charged;
    [SerializeField] private Animator animator;
    [SerializeField] private float charge_time;
    [SerializeField] private Sprite base_image;
    [SerializeField] private Sprite[] charging_frames;
    [SerializeField] private Sprite[] active_frames;
    [SerializeField] private float active_frames_window;

    [SerializeField] private bool held = false;
    [SerializeField] private bool ready = false;
    private IEnumerator charging_func;

    [SerializeField] private bool activated = false;
    [SerializeField] private bool eneme_swing = false;
    [SerializeField] protected int currencyValue = 0;

    // Update is called once per frame
    void Update()
    {
        move_weapon();
        on_click();
    }

    public void SetTarget(Transform target_transfr) {
        target = target_transfr;
    }

    public void SetAnchor(Transform anchor_transfr) {
        anchor = anchor_transfr;
    }

    private void move_weapon() {
        if (!is_enemy_weapon && !weapon_box_set) {
            weapon_box_set = true;
            UIController.Instance.SetWeaponBoxSprite(base_image);
        }
        Vector3 v3target = new Vector3();
        if (target != null) { // if there is a target (i.e. player) use that
            v3target = target.position;
            v3target = Camera.main.WorldToScreenPoint(target.position);
        } else { // else target the cursor
            v3target = Input.mousePosition;
        }
        // logic to orbit the weapon around the anchor
        Vector3 orbit_target = v3target;
        orbit_target.z = (anchor.position.z - Camera.main.transform.position.z);
        orbit_target = Camera.main.ScreenToWorldPoint(orbit_target);
        orbit_target = orbit_target - anchor.position;
        float angle;
        angle = Mathf.Atan2(orbit_target.y, orbit_target.x) * Mathf.Rad2Deg;
        if (angle < 0.0f) {
            angle += 360f;
        }
        float xpos = Mathf.Cos(Mathf.Deg2Rad * angle) * distance;
        float ypos = Mathf.Sin(Mathf.Deg2Rad * angle) * distance;
        transform.position = new Vector3(anchor.position.x + xpos, anchor.position.y + ypos, 0);
        // logic to rotate the weapon towards the target
        // Debug.Log(weapon_idle.transform.position);
        Vector3 difference = Camera.main.ScreenToWorldPoint(v3target) - anchor.position;
        difference.Normalize();
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        if (rotationZ < -90 || rotationZ > 90) {
            if(anchor.eulerAngles.y == 0) {
                transform.localRotation = Quaternion.Euler(180, 0, -rotationZ);
            } else if (anchor.eulerAngles.y == 180) {
                transform.localRotation = Quaternion.Euler(180, 180, -rotationZ);
            }
        }
    }

    private void on_click() {
        if (target != null) {
            if (eneme_swing) {
                if (!activated) {
                    activated = true;
                    StartCoroutine(swing_weapon());
                }
            }
        } else {
            if (!Input.GetKey(KeyCode.LeftShift)) {
                if (charged) {
                    if (Input.GetMouseButtonDown(0) && !activated) {
                        Set_Held(true);
                    }
                    if (Input.GetMouseButtonUp(0) && !activated) {
                        Set_Held(false);
                    }
                } else {
                    if (Input.GetMouseButtonDown(0) && !activated) {
                        StartCoroutine(swing_weapon());
                    }
                }
            }
                
            
        }
        
        // if (!activated) {
        //     weapon_active.SetActive(false);
        //     weapon_idle.SetActive(true);
        // }
    }

    // private IEnumerator swing_weapon() {
    //     weapon_idle.SetActive(false);
    //     weapon_active.SetActive(true);
    //     activated = true;
    //     yield return new WaitForSeconds(end_lag);
    //     activated = false;
    // }

    private IEnumerator swing_weapon() {
        activated = true;
        weapon_idle.SetActive(false);
        idle_renderer.sprite = base_image;
        weapon_active.SetActive(true);
        for (int i = 0; i < active_frames.Length; i++) {
            active_renderer.sprite = active_frames[i];
            if (!is_enemy_weapon) {
                UIController.Instance.SetWeaponBoxSprite(active_frames[i]);
            }
            yield return new WaitForSeconds(active_frames_window);
        }
        weapon_idle.SetActive(true);
        weapon_active.SetActive(false);
        if (!is_enemy_weapon) {
                UIController.Instance.SetWeaponBoxSprite(base_image);
            }
        yield return new WaitForSeconds(end_lag);
        activated = false;
    }

    private IEnumerator charge_weapon() {
        for (int i = 0; i < charging_frames.Length; i++) {
            idle_renderer.sprite = charging_frames[i];
            yield return new WaitForSeconds(charge_time / charging_frames.Length);
        }
        ready = true;
    }

    public void Swing() {
        StartCoroutine(swing_weapon());
    }
    public void Set_Held(bool isHeld) {
        held = isHeld;
        if (held) { // 
            charging_func = charge_weapon();
            StartCoroutine(charging_func);
        } else {
            if (ready) {
                activated = true;
                StartCoroutine(swing_weapon());
            } else {
                if (charging_func != null) {
                    StopCoroutine(charging_func);
                }
                idle_renderer.sprite = base_image;
                charging_func = null;
            }
            ready = false;
        }
    }

    public float GetKnockback() {
        return knockback;
    }

    public int GetDamage() {
        return damage;
    }

    public bool IsEnemyWeapon() {
        return is_enemy_weapon;
    }

    public void SetEnemyWeapon() {
        is_enemy_weapon = true;
    }

    public int GetCurrency()
    {
        return currencyValue;
    }
}
