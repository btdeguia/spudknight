using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeEnemyBehavior : EnemyBehavior
{
    private bool slime_can_jump = true;    

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
}
