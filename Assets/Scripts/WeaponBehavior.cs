using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    [SerializeField] private GameObject weapon_idle;
    [SerializeField] private GameObject weapon_active;
    [SerializeField] private Transform target;
    [SerializeField] private Transform anchor;


    [SerializeField] private int damage;
    [SerializeField] private float distance;
    [SerializeField] private float knockback;

    [SerializeField] private bool is_enemy_weapon;

    private bool activated = false;

    // Update is called once per frame
    void Update()
    {
        move_weapon();
        on_click();
    }

    private void move_weapon() {
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
            if (activated) {
                weapon_idle.SetActive(false);
                weapon_active.SetActive(true);
                activated = true;
                StartCoroutine(swing_weapon());
            }
        } else {
            if (Input.GetMouseButtonDown(0) && !activated) {
                weapon_idle.SetActive(false);
                weapon_active.SetActive(true);
                activated = true;
                StartCoroutine(swing_weapon());
            }
        }
        
        if (!activated) {
            weapon_active.SetActive(false);
            weapon_idle.SetActive(true);
        }
    }

    private IEnumerator swing_weapon() {
        yield return new WaitForSeconds(0.25f);
        activated = false;
    }

    public void Set_Active(bool isActive) {
        activated = isActive;
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
}
