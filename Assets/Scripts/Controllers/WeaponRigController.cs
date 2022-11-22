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

    [SerializeField] private List<GameObject> internal_weapon_list = new List<GameObject>();

    [SerializeField] private GameObject weapon_inst;
    private int weapon_pos = 0;

    void OnEnable() {
        if (weapon != null) {
            internal_weapon_list.Add(weapon);

            weapon_inst = Instantiate(weapon);
            weapon_inst.transform.parent = transform;
            WeaponBehavior behavior = weapon_inst.transform.GetChild(0).GetComponent<WeaponBehavior>();
            if (behavior != null) {
                if (is_enemy) {
                    behavior.SetTarget(enemy.GetTarget());
                    behavior.SetAnchor(transform);
                    behavior.SetEnemyWeapon();
                    enemy.SetWeapon(behavior);
                } else {
                    behavior.SetTarget(null);
                    behavior.SetAnchor(transform);
                }
                
            } 
        }
        
    }

    void Update()
    {
        if (weapon != null) {
           transform.position = new Vector3(anchor.position.x, anchor.position.y, 0);
            if (anchor == null) {
                Destroy(gameObject);
            } 
        }

        if (Input.mouseScrollDelta.y > 0) {
            Debug.Log(Input.mouseScrollDelta.y);
            Destroy(weapon_inst);
            weapon_pos++;
            if (weapon_pos >= internal_weapon_list.Count) {
                weapon_pos = 0;
            }
            Instantiate_Weapon();
        }

        if (Input.mouseScrollDelta.y < 0) {
            Debug.Log(Input.mouseScrollDelta.y);
            Destroy(weapon_inst);
            weapon_pos--;
            if (weapon_pos < 0) {
                weapon_pos = internal_weapon_list.Count - 1;
            }
            Instantiate_Weapon();
        }
        
    }

    public void Instantiate_Weapon() {
        weapon_inst = Instantiate(internal_weapon_list[weapon_pos]);
        weapon_inst.transform.parent = transform;
        WeaponBehavior behavior = weapon_inst.transform.GetChild(0).GetComponent<WeaponBehavior>();
        if (behavior != null) {
            if (is_enemy) {
                behavior.SetTarget(enemy.GetTarget());
                behavior.SetAnchor(transform);
                behavior.SetEnemyWeapon();
                enemy.SetWeapon(behavior);
            } else {
                behavior.SetTarget(null);
                behavior.SetAnchor(transform);
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


    public bool IsEnemy() {
        return is_enemy;
    }

    public EnemyBehavior GetEnemyBehavior() {
        return enemy;
    }

    public void AddWeapon(GameObject new_weapon) {
        WeaponBehavior behavior = new_weapon.transform.GetChild(0).GetComponent<WeaponBehavior>();
        if (behavior != null) {
            internal_weapon_list.Add(new_weapon);
        }
    }

    public GameObject GetCurrentWeapon() {
        return internal_weapon_list[weapon_pos];
    }
}
