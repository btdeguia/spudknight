using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BruteEnemyBehavior : EnemyBehavior
{

    void Start()
    {
        reward_amount = 25;
    }

    public override IEnumerator Attack()
    {
        attacking = true;
        base.weapon_behavior.Set_Held(true);
        yield return new WaitForSeconds(1.05f);
        base.weapon_behavior.Set_Held(false);
        yield return new WaitForSeconds(1f);
        attacking = false;
    }

    public override IEnumerator Hitstun()
    {
        yield return null;
    }
}