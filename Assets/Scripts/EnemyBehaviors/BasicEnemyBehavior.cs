using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyBehavior : EnemyBehavior
{
    void Start()
    {
        reward_amount = 10;
    }

    public override IEnumerator Attack() {
        StartCoroutine(base.Attack());
        yield return null;
    }
}
