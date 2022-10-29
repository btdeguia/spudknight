using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RogueEnemyBehavior : EnemyBehavior
{
    public override IEnumerator Attack()
    {
        yield return new WaitForSeconds(0.10f);
        Debug.Log("attacked!");
        StartCoroutine(base.Attack());
    }
}
