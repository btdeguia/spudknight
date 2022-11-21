using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Object Refs")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected SpriteRenderer sprite_renderer;
    [SerializeField] protected Rigidbody2D rigidbody_2d;
    [SerializeField] protected Collider2D collider_2d;
    [SerializeField] protected WeaponBehavior weapon_behavior;
    [SerializeField] protected Transform target;
    [SerializeField] protected GameObject stun_effect;
    [SerializeField] protected GameObject attack_indicator;
    [SerializeField] protected WeaponRigController weapon_rig;

    [Header("Enemy Settings")]
    [SerializeField] protected float speed;
    [SerializeField] protected int health;
    [SerializeField] protected float distance_to_attack;
    [SerializeField] protected int reward_amount;

    [SerializeField] protected bool attacking = false;
    protected bool walking = false;
    protected bool stunned = false;
    protected bool exit = false;
    protected bool knockback_active = false;

    void Start()
    {
        
    }

    void Update()
    {
        Walk_Towards();
    }

    public virtual void SetTarget(Transform target_transfr) {
        target = target_transfr;
    }

    public virtual void SetAnchor() {
        weapon_behavior.SetAnchor(transform);
    }

    public virtual void SetWeapon(WeaponBehavior weapon) {
        weapon_behavior = weapon;
    }

    public virtual Transform GetTarget() {
        return target;
    } 

    public virtual void SetWeaponRig(WeaponRigController rig) {
        weapon_rig = rig;
    }

    public virtual void Walk_Towards() {
        float distance_between = Vector3.Distance(transform.position, target.position);
        if (distance_between > distance_to_attack && !attacking && !stunned) {
            if (!walking) {
                animator.SetBool("is_walking", true);
                walking = true;
            }
            // transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
            Vector3 movement = (target.position - transform.position);
            movement.Normalize();
            rigidbody_2d.velocity =  movement * speed;
            walking = true;
        } else {
            if (!attacking && !stunned) {
                if (walking) {
                    animator.SetBool("is_walking", false);
                    rigidbody_2d.velocity = Vector2.zero;
                    walking = false;
                }
                StartCoroutine(Attack());
            }
            
        }
    }

    public virtual IEnumerator Attack() {
        attacking = true;
        attack_indicator.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        if (!stunned) {
            weapon_behavior.Swing();
        }
        attack_indicator.SetActive(false);
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
        Destroy(weapon_rig.gameObject);
        Destroy(gameObject);
    }

    public virtual IEnumerator Knockback(Collider2D collider) {
        if (collider.gameObject.name != "P_Player" && !knockback_active) {
            knockback_active = true;
            Vector2 direction = collider.transform.parent.localPosition; // get local position of weapon
            // direction.Normalize(); // normalize it (left = positive, right = negative)
            float knockback = collider.transform.parent.GetComponent<WeaponBehavior>().GetKnockback(); // get knockback value from parent
            rigidbody_2d.AddForce(direction * knockback * Time.smoothDeltaTime, ForceMode2D.Impulse);
            Vector3 dest = new Vector3(transform.position.x + direction.x, transform.position.y + direction.y, 0);
            
            yield return new WaitForSeconds(0.25f);
            rigidbody_2d.velocity = Vector2.zero;
            knockback_active = false;
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D collider) {
        // if:
        // not collided with player
        // not the weapon this object is holding
        // not collided with another enemy
        // if (collider.gameObject.name[2] != 'W') {
        //     return;
        //     // Physics2D.IgnoreCollision(collider_2d, collider.GetComponent<Collider>(), true);
        // }
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
                int currCurrency = FinanceController.Instance.GetCurrency();
                int calculation = currCurrency + reward_amount;
                FinanceController.Instance.SetCurrency(calculation);
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
