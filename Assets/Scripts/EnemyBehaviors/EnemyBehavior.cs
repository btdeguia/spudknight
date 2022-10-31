using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Object Refs")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected SpriteRenderer sprite_renderer;
    [SerializeField] protected Rigidbody2D rigidbody_2d;
    [SerializeField] protected WeaponBehavior weapon_behavior;
    [SerializeField] protected Transform target;
    [SerializeField] protected GameObject stun_effect;

    [Header("Enemy Settings")]
    [SerializeField] protected float speed;
    [SerializeField] protected int health;
    [SerializeField] protected int distance_to_attack;

    [SerializeField] protected bool attacking = false;
    protected bool walking = false;
    protected bool stunned = false;
    protected bool exit = false;

    void Start()
    {
        
    }

    void Update()
    {
        Walk_Towards();
    }

    public virtual void Set_Attr(Transform target_transfr) {
        target = target_transfr;
        weapon_behavior.Set_Attr(target_transfr);
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
                if (walking) {
                    animator.SetBool("is_walking", false);
                    walking = false;
                }
                StartCoroutine(Attack());
            }
            
        }
    }

    public virtual IEnumerator Attack() {
        attacking = true;
        yield return new WaitForSeconds(0.25f);
        if (!stunned) {
            weapon_behavior.Swing();
        }
        yield return new WaitForSeconds(1f);
        attacking = false;
    }

    public virtual IEnumerator Hitstun() {
        stunned = true;
        yield return new WaitForSeconds(0.5f);
        stunned = false;
        exit = false;
    }

    public virtual void Death() {
        StopAllCoroutines();
        Destroy(gameObject);
    }

    public virtual IEnumerator Knockback(Collider2D collider) {
        if (collider.gameObject.name != "P_Player") {
            Debug.Log("knockback applied");
            Vector2 direction = collider.transform.parent.localPosition; // get local position of weapon
            direction.x = direction.x + 1.25f; // normalize it (left = positive, right = negative)
            float knockback = collider.transform.parent.GetComponent<WeaponBehavior>().GetKnockback(); // get knockback value from parent
            rigidbody_2d.AddForce(direction * knockback * Time.smoothDeltaTime, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.25f);
            rigidbody_2d.velocity = Vector2.zero;
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collider) {
        // if:
        // not collided with player
        // not the weapon this object is holding
        // not collided with another enemy
        if (collider.gameObject.name != "P_Player" && collider.gameObject != weapon_behavior.gameObject && collider.gameObject.name[2] != 'E') {
            Debug.Log(collider.gameObject.name + " collided with " + gameObject.name);
            WeaponBehavior collider_weapon_behavior = collider.transform.parent.GetComponent<WeaponBehavior>();
            if (collider_weapon_behavior != null) { // if is a weapon
                if (!collider_weapon_behavior.IsEnemyWeapon()) { // if is not an enemy weapon
                    health -= collider_weapon_behavior.GetDamage();
                    if (health > 0) {
                        StartCoroutine(Knockback(collider));
                        // sprite_renderer.color /= 1.1f;
                        StartCoroutine(Hitstun()); 
                    }
                    
                }
            } else { // else is not a weapon, probably contact damage
                health--;
                // sprite_renderer.color /= 1.1f;
                // StartCoroutine(Hitstun()); 
            }
            if (health <= 0) {
                Death();
            }
        }
        
    }

    public virtual void OnTriggerExit2D(Collider2D collider) {
        if (!exit) {
            sprite_renderer.color *= 1.1f;
            exit = true;
        }
    }

    public virtual void Parried() {
        StartCoroutine(Parry_Stun());
    }

    public virtual IEnumerator Parry_Stun() {
        stunned = true;
        stun_effect.SetActive(true);
        stun_effect.transform.rotation = new Quaternion(0, 180, 0, 0);
        yield return new WaitForSeconds(0.25f);
        stun_effect.transform.rotation = new Quaternion(0, 0, 0, 0);
        yield return new WaitForSeconds(0.25f);
        stun_effect.transform.rotation = new Quaternion(0, 180, 0, 0);
        yield return new WaitForSeconds(0.25f);
        stun_effect.transform.rotation = new Quaternion(0, 0, 0, 0);
        yield return new WaitForSeconds(0.25f);
        stun_effect.SetActive(false);
        stunned = false;
    }
}
