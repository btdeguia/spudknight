using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Object Refs")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sprite_renderer;
    [SerializeField] private Rigidbody2D rigidbody_2d;
    [SerializeField] private WeaponBehavior weapon_behavior;
    [SerializeField] private Transform target;
    [Header("Enemy Settings")]
    [SerializeField] private float speed;
    [SerializeField] private int health;
    [SerializeField] private int distance_to_attack;

    private bool attacking = false;
    private bool walking = false;
    private bool stunned = false;
    private bool exit = false;

    void Start()
    {
        
    }

    void Update()
    {
        Walk_Towards();
    }

    public virtual void Walk_Towards() {
        float distance_between = Vector3.Distance(transform.position, target.position);
        if (distance_between > distance_to_attack && !attacking && !stunned) {
            if (!walking) {
                animator.SetBool("is_walking", true);
                walking = true;
            }
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
        } else {
            if (!attacking && !stunned) {
                attacking = true;
                if (walking) {
                    animator.SetBool("is_walking", false);
                    walking = false;
                }
                StartCoroutine(Attack());
            }
            
        }
    }

    public virtual IEnumerator Attack() {
        yield return new WaitForSeconds(0.25f);
        if (!stunned) {
            weapon_behavior.Set_Active(true);
        }
        yield return new WaitForSeconds(1f);
        attacking = false;
    }

    public virtual IEnumerator Hitstun() {
        yield return new WaitForSeconds(0.5f);
        stunned = false;
        exit = false;
    }

    public virtual IEnumerator Knockback(Collider2D collider) {
        if (collider.gameObject.name != "P_Player") {
            Vector2 direction = collider.transform.parent.localPosition; // get local position of weapon
            direction.x = direction.x + 1.25f; // normalize it (left = positive, right = negative)
            float knockback = collider.transform.parent.GetComponent<WeaponBehavior>().GetKnockback(); // get knockback value from parent
            rigidbody_2d.AddForce(direction * knockback * Time.smoothDeltaTime, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.25f);
            rigidbody_2d.velocity = Vector2.zero;
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collider) {
        if (!stunned && collider.gameObject.name != "P_Player" && collider.gameObject != weapon_behavior.gameObject && collider.gameObject.name[2] != 'E') {
            WeaponBehavior collider_weapon_behavior = collider.transform.parent.GetComponent<WeaponBehavior>();
            if (collider_weapon_behavior != null) { // if is a weapon
                if (!weapon_behavior.IsEnemyWeapon()) {
                    health -= collider_weapon_behavior.GetDamage();
                    StartCoroutine(Knockback(collider));
                    sprite_renderer.color /= 1.1f;
                    stunned = true;
                    StartCoroutine(Hitstun()); 
                }
            } else { // else is not a weapon
                health--;
                sprite_renderer.color /= 1.1f;
                stunned = true;
                StartCoroutine(Hitstun()); 
            }
        }
        
    }

    public virtual void OnTriggerExit2D(Collider2D collider) {
        if (!exit) {
            sprite_renderer.color *= 1.1f;
            exit = true;
        }
    }
}
