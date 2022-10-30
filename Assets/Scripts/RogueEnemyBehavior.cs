using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueEnemyBehavior : EnemyBehavior
{

    [SerializeField] private int distance_to_dash;
    private int distance_to_attack_temp;
    private bool rogue_can_dash = true;

    void Start() {
        distance_to_attack_temp = base.distance_to_attack;
        base.distance_to_attack = distance_to_dash;
    }

    public override IEnumerator Attack()
    {
        yield return new WaitForSeconds(1f);
        if (!base.stunned) {
           if (rogue_can_dash) {
                StartCoroutine(rogue_dash());
            } else {
                StartCoroutine(attack_helper());
            } 
        }
        
    }

    private IEnumerator rogue_dash() {
        float lerp_distance = 0;
        Vector3 dash_target = base.target.position;

        Vector3 direction = (dash_target - transform.position);
        float distance = direction.magnitude;
        direction = direction / distance;
        direction = direction * 2f;

        for (int i = 0; i < 5; i++) {
            transform.position = Vector3.Lerp(transform.position, direction, lerp_distance);
            lerp_distance += 0.2f;
            yield return new WaitForSeconds(0.01f);
        }
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
        yield return new WaitForSeconds(0.25f);
        if (!stunned) {
            weapon_behavior.Set_Active(true);
        }
        yield return new WaitForSeconds(1f);
        attacking = false;
    }
}
