using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyBehavior : EnemyBehavior
{
    public override IEnumerator Attack() {
        yield return new WaitForSeconds(0.25f);
        Debug.Log("attacked!");
        StartCoroutine(base.Attack());
    }
}
