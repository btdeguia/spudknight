using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueEnemyBehavior : EnemyBehavior
{

    [SerializeField] private Sprite default_sprite;
    [SerializeField] private Sprite dashing_sprite;
    [SerializeField] private int distance_to_dash;
    private int distance_to_attack_temp;
    private bool rogue_can_dash = true;

    void Start() {
        reward_amount = 15;
        distance_to_attack_temp = base.distance_to_attack;
        base.distance_to_attack = distance_to_dash;
    }

    public override IEnumerator Attack()
    {
        Debug.Log("rogue attack method");
        if (!base.stunned) {
           if (rogue_can_dash) {
                base.attacking = true;
                yield return new WaitForSeconds(1f);
                StartCoroutine(rogue_dash());
            } else {
                if (!base.attacking) {
                    attacking = true;
                    yield return new WaitForSeconds(0.25f);
                    StartCoroutine(attack_helper());
                }
            } 
        }
        
    }

    private IEnumerator rogue_dash() {
        float lerp_distance = 0;
        Debug.Log("position of target: " + base.target.position);
        Vector3 dash_target = new Vector3(base.target.position.x, base.target.position.y, 0);
        Vector3 dash_overshoot = dash_target.normalized * 5f;
        dash_target += dash_overshoot;

        base.animator.SetBool("is_dashing", true);
        transform.up = dash_target - transform.position;


        for (int i = 0; i < 5; i++) {
            transform.position = Vector3.Lerp(transform.position, dash_target, lerp_distance);
            lerp_distance += 0.2f;
            yield return new WaitForSeconds(0.05f);
        }

        base.animator.SetBool("is_dashing", false);
        transform.rotation = new Quaternion(0, 0, 0, 0); 

        rogue_can_dash = false;
        base.attacking = false;

        

        StartCoroutine(dash_cooldown());
    }

    private IEnumerator dash_cooldown() {
        base.distance_to_attack = distance_to_attack_temp;
        yield return new WaitForSeconds(5f);
        rogue_can_dash = true;
        base.distance_to_attack = distance_to_dash;
    }

    private IEnumerator attack_helper() {
        base.attacking = true;
        yield return new WaitForSeconds(0.25f);
        if (!stunned) {
            weapon_behavior.Swing();
        }
        yield return new WaitForSeconds(1f);
        base.attacking = false;
    }
}
