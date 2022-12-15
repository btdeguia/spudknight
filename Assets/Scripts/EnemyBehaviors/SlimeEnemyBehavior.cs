using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemyBehavior : EnemyBehavior
{
    [SerializeField] private GameObject slime_prefab;
    [SerializeField] private Sprite mitosis_sprite;
    [SerializeField] private Sprite default_sprite;
    private bool slime_can_jump = true;    
    private bool isDead = false;
    private bool can_mitosis = true;

    public override IEnumerator Attack()
    {
        if (!base.stunned) {
           if (slime_can_jump) {
                //show player that slime is about to jump
                base.attacking = true;
                base.attack_indicator.SetActive(true);
                yield return new WaitForSeconds(1f);
                base.attack_indicator.SetActive(false);

                //give the slime the target to jump to
                Vector3 dash_target = new Vector3(base.target.position.x, base.target.position.y, 0);
                //set the jump animtion on 

                base.animator.SetBool("is_dashing", true);
                //base.rigidbody_2d.freezeRotation = false;

                //jump to enemy
                Vector3 dash_direction = dash_target - transform.position;
                base.rigidbody_2d.AddForce(dash_direction * 2.5f, ForceMode2D.Impulse);

                yield return new WaitForSeconds(.5f);
                base.animator.SetBool("is_dashing", false);
                base.rigidbody_2d.velocity = Vector2.zero;
                base.rigidbody_2d.freezeRotation = true;
                transform.rotation = new Quaternion(0, 0, 0, 0); 

                slime_can_jump = false;
                base.attacking = false;

                StartCoroutine(jump_cooldown());
            }
        }   
    }

    private IEnumerator jump_cooldown() {
        yield return new WaitForSeconds(2.5f);
        slime_can_jump = true;
    }

    public override void OnTriggerEnter2D(Collider2D collider) {
        // if:
        // not collided with player
        // not the weapon this object is holding
        // not collided with another enemy
        // if (collider.gameObject.name[2] != 'W') {
        //     return;
        //     // Physics2D.IgnoreCollision(collider_2d, collider.GetComponent<Collider>(), true);
        // }
        if (isDead) return;
        Debug.Log("Slime hit: by " + collider.gameObject.name);
        if (collider.gameObject.name != "P_P_Player" && collider.gameObject.name[2] != 'E') {
            // Debug.Log(collider.gameObject.name + " collided with " + gameObject.name);
            WeaponBehavior collider_weapon_behavior = collider.transform.parent.GetComponent<WeaponBehavior>();
            if (collider_weapon_behavior != null) { // if is a weapon
                if (!collider_weapon_behavior.IsEnemyWeapon()) { // if is not an enemy weapon
                    base.health -= collider_weapon_behavior.GetDamage();
                    StartCoroutine(base.damage_effect());
                    if (health > 0) {
                        StartCoroutine(base.Knockback(collider));
                        // sprite_renderer.color /= 1.1f;
                        StartCoroutine(base.Hitstun()); 
                    }
                    
                }
            } else { // else is not a weapon, probably contact damage
                base.health--;
                // sprite_renderer.color /= 1.1f;
                // StartCoroutine(Hitstun()); 
            }
            if (health <= 0) {
                Death();
                int currCurrency = FinanceController.Instance.GetCurrency();
                int calculation = currCurrency + reward_amount;
                FinanceController.Instance.SetCurrency(calculation);
                UIController.Instance.GainCurrency(transform.position);
            }
        }
        
    }

    public void SetMitosisFalse() {
        can_mitosis = false;
    }

    public override void Death() {
        isDead = true;
        base.rigidbody_2d.velocity = Vector2.zero;
        base.sprite_renderer.material = base.default_mat;
        StopAllCoroutines();
        if (can_mitosis) {
            Debug.Log(gameObject.name);
            spawner.GetComponent<GateController>().addToDead();
            StartCoroutine(mitosis());
        } else {
            Destroy(gameObject);
        }
        
    }

    private IEnumerator mitosis() {
        stunned = true;
        base.animator.enabled = false;
        base.sprite_renderer.sprite = mitosis_sprite;
        yield return new WaitForSeconds(1f);

        GameObject hero = GameController.Instance.GetHero();

        Vector3 enemy_local_pos = transform.position;

        slime_prefab = (GameObject) Resources.Load("P_E_Slime");

        GameObject slime_1 = Instantiate(slime_prefab, new Vector3(enemy_local_pos.x - 1, enemy_local_pos.y, 0), new Quaternion(0, 0, 0, 0));
        GameObject slime_2 = Instantiate(slime_prefab, new Vector3(enemy_local_pos.x + 1, enemy_local_pos.y, 0), new Quaternion(0, 0, 0, 0));
        SlimeEnemyBehavior slime_behavior_1 = slime_1.GetComponent<SlimeEnemyBehavior>();
        SlimeEnemyBehavior slime_behavior_2 = slime_2.GetComponent<SlimeEnemyBehavior>();
        slime_behavior_1.SetTarget(hero.transform);
        slime_behavior_2.SetTarget(hero.transform);
        slime_behavior_1.SetMitosisFalse();
        slime_behavior_2.SetMitosisFalse();
        slime_1.transform.localScale = new Vector2(0.75f, 0.75f);
        slime_2.transform.localScale = new Vector2(0.75f, 0.75f);
        Destroy(gameObject);
    }
}
