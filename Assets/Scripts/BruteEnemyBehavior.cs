using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BruteEnemyBehavior : EnemyBehavior
{
    public override IEnumerator Attack()
    {
        yield return new WaitForSeconds(5f);
        Debug.Log("attacked!");
        StartCoroutine(base.Attack());
    }

    public override IEnumerator Hitstun()
    {
        yield return null;
    }
}