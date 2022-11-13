using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct EnemySpawnPoint {
    public int index;
    public Vector3 pos;
}

public class GateController : MonoBehaviour
{
    [SerializeField] private List<EnemySpawnPoint> enemies_in_room = new List<EnemySpawnPoint>();
    [SerializeField] private GameObject weapon_rig;
    [SerializeField] private Collider2D collider_2d;

    [SerializeField] private Transform room;

    private bool locked = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemies() {
        for (int i = 0; i < enemies_in_room.Count; i++) {
            SpawnEnemy(enemies_in_room[i]);
        }
    }

    private void SpawnEnemy(EnemySpawnPoint enemy_spawn_point) {
        Enemy enemy = GameController.Instance.GetEnemyStruct(enemy_spawn_point.index);

        GameObject hero = GameController.Instance.GetHero();

        Vector3 enemy_local_pos = room.TransformPoint(enemy_spawn_point.pos);

        GameObject enemy_obj = Instantiate(enemy.enemy_prefab, enemy_local_pos, new Quaternion(0, 0, 0, 0));
        EnemyBehavior enemy_behavior = enemy_obj.GetComponent<EnemyBehavior>();
        enemy_behavior.SetTarget(hero.transform);
        GameObject weapon_rig_obj = Instantiate(weapon_rig);
        WeaponRigController controller = weapon_rig_obj.GetComponent<WeaponRigController>();
        controller.Set_Attr(enemy.weapon_prefab, enemy_obj.transform, enemy_behavior, true);
        enemy_behavior.SetWeaponRig(controller);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log(collision.gameObject.name + " encountered the gate ");
        if (collision.gameObject.name[2] == 'P') {
            if (!locked) {
                Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), collider_2d);
                SpawnEnemies();
            }
            
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        Debug.Log("collision exit");
        locked = true;
    }
}
