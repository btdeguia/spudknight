using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    [Header("Object Refs")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sprite_renderer;
    [SerializeField] private Rigidbody2D rigidbody_2d;
    [Header("Player Settings")]
    [SerializeField] private float speed;
    [SerializeField] private int health;
    private bool walking = false;
    private bool facing_back = false;
    private bool stunned;
    private bool exit;

    [Header("Shield Settings")]
    [SerializeField] private Transform shield_transform;
    [SerializeField] private SpriteRenderer shield_renderer;
    [SerializeField] private Sprite shield_idle;
    [SerializeField] private Sprite shield_active;
    [SerializeField] private Vector3[] shield_anchors;
    private Vector3 idle_anchor;
    private Vector3 active_anchor;
    private bool positive_x = false;
    private bool parrying = false;
    private bool blocking = false;
    private bool block_available = true;

    

    

    // Start is called before the first frame update
    void Start()
    {
        idle_anchor = shield_anchors[0];
        active_anchor = shield_anchors[1];
        shield_transform.position = idle_anchor;
    }

    // Update is called once per frame
    void Update()
    {
        walk();
        move_cursor();
        block();
    }

    private void walk() {
        walking = false;
        if (Input.GetKey(KeyCode.W)) {
            transform.Translate(0, speed * Time.deltaTime, 0);
            walking = true;
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            walking = true;
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            walking = true;
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.Translate(0, -speed * Time.deltaTime, 0);
            walking = true;
        }
        animator.SetBool("is_walking", walking);
    }

    private void move_cursor() {
        // Vector3 pos = Input.mousePosition;
        // float mouse_pos_y = pos.y - 220 - (transform.position.y * 40);
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        float mouse_pos_y = difference.y;
        float mouse_pos_x = difference.x;
        if (mouse_pos_y > 0 && facing_back) {
            animator.SetTrigger("switch_facing");
            facing_back = false;
        }
        if (mouse_pos_y < 0 && !facing_back) {
            animator.SetTrigger("switch_facing");
            facing_back = true;
        }
        if (mouse_pos_x > 0 && !positive_x) {
            positive_x = true;
            idle_anchor = shield_anchors[0];
            active_anchor = shield_anchors[1];
            update_shield();
        }
        if (mouse_pos_x < 0 && positive_x) {
            positive_x = false;
            idle_anchor = shield_anchors[3];
            active_anchor = shield_anchors[2];
            update_shield();
        }
    }

    private void block() {
        if (Input.GetKeyDown(KeyCode.LeftShift) && block_available) {
            blocking = true;
            UIController.Instance.PressParryButton();
            update_shield();
            StartCoroutine(parry());
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) && blocking) {
            blocking = false;
            UIController.Instance.ReleaseParryButton(1f);
            update_shield();
            StartCoroutine(block_cooldown());
        }
    }

    private IEnumerator parry() {
        parrying = true;
        yield return new WaitForSeconds(0.5f);
        parrying = false;
    }

    private IEnumerator parry_cooldown() {
        yield return new WaitForSeconds(0.5f);
        shield_renderer.sprite = shield_idle;
    }

    private void update_shield() {
        if (blocking) {
            shield_transform.localPosition = active_anchor;
        } else {
            shield_transform.localPosition = idle_anchor;
        }
    }

    private IEnumerator block_cooldown() {
        block_available = false;
        yield return new WaitForSeconds(1f);
        block_available = true;
    }

    private IEnumerator hitstun() {
        yield return new WaitForSeconds(0.5f);
        stunned = false;
        exit = false;
    }

    private IEnumerator knockback(Collider2D collider) {
        if (collider.transform.parent != null) {
            Vector2 direction = collider.transform.parent.localPosition; // get local position of weapon
            direction.x = direction.x + 1.25f; // normalize it (left = positive, right = negative)
            float knockback = collider.transform.parent.GetComponent<WeaponBehavior>().GetKnockback(); // get knockback value from parent
            rigidbody_2d.AddForce(direction * knockback * Time.smoothDeltaTime, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.25f);
            rigidbody_2d.velocity = Vector2.zero;
        }
        
    }

    public void OnTriggerEnter2D(Collider2D collider) {
            Debug.Log("collided with " + collider.gameObject.name);
            if (collider.gameObject.name[2] == 'E') { // contact damage
                if (parrying) {
                    collider.gameObject.GetComponent<EnemyBehavior>().Parried();
                }
                if (!blocking) {
                    health--; 
                    UIController.Instance.HeroTakeDamage(1);
                }
            }
            if (collider.transform.parent != null) {
                WeaponBehavior weapon = collider.transform.parent.GetComponent<WeaponBehavior>();
                if (weapon != null) {
                    if (parrying) {
                        Debug.Log("parried");
                        shield_renderer.sprite = shield_active;
                        StartCoroutine(parry_cooldown());
                        if (weapon.transform.parent != null && weapon.transform.parent.parent != null) {
                            EnemyBehavior enemy = weapon.transform.parent.parent.GetComponent<EnemyBehavior>();
                            if (enemy != null) {
                                enemy.Parried();
                            }
                        } 
                    }
                    if (!blocking) {
                        health -= weapon.GetDamage();
                        UIController.Instance.HeroTakeDamage(weapon.GetDamage());
                        // sprite_renderer.color /= 1.1f;
            
                        StartCoroutine(knockback(collider));
                        stunned = true;
                        StartCoroutine(hitstun()); 
                    }
                    
                }
            }
            
            
        
    }

    public void OnTriggerExit2D(Collider2D collider) {
        if (!exit) {
            sprite_renderer.color *= 1.1f;
            exit = true;
        }
    }
}
