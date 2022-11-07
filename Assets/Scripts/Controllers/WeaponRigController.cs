using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRigController : MonoBehaviour
{

    [SerializeField] private Transform anchor;
    [SerializeField] private GameObject weapon;
    [SerializeField] private PlayerBehavior player;
    [SerializeField] private EnemyBehavior enemy;
    [SerializeField] private bool is_enemy;

    void OnEnable() {
        if (weapon != null) {
           GameObject weapon_inst = Instantiate(weapon);
            weapon_inst.transform.parent = transform;
            WeaponBehavior behavior = weapon_inst.transform.GetChild(0).GetComponent<WeaponBehavior>();
            if (behavior != null) {
                if (is_enemy) {
                    behavior.Set_Attr(enemy.GetTarget(), transform);
                    behavior.SetEnemyWeapon();
                } else {
                    behavior.Set_Attr(null, transform);
                }
                
            } 
        }
        
    }

    void Update()
    {
        if (weapon != null) {
           transform.position = new Vector3(anchor.position.x, anchor.position.y, 0);
            if (anchor == null) {
                Destroy(this);
            } 
        }
        
    }

    public void Set_Attr(GameObject weaponPrefab, Transform anchor_transfr, EnemyBehavior enemy_behavior, bool isEnemy) {
        weapon = weaponPrefab;
        anchor = anchor_transfr;
        enemy = enemy_behavior;
        is_enemy = isEnemy;
        OnEnable();
    }
}
