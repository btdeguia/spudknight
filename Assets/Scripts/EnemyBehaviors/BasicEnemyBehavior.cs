using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyBehavior : EnemyBehavior
{
    public override IEnumerator Attack() {
        StartCoroutine(base.Attack());
        yield return null;
    }
}
