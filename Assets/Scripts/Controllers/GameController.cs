using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Everytime an enemy dies, make a new one
    [SerializeField] private GameObject mWeaponRig;
    [SerializeField] private GameObject mHero;
    [SerializeField] private GameObject mRogue;
    [SerializeField] private GameObject mBrute;
    [SerializeField] private GameObject mBasic;
    [SerializeField] private GameObject mKnives;
    [SerializeField] private GameObject mHammer;
    [SerializeField] private GameObject mFork;
    private GameObject hero;
    private GameObject rogue;
    private GameObject brute;
    private GameObject basic;

    private GameObject[] enemies;
    private GameObject[] weapons;
    private GameObject enemy;
    int pos = 0;

    // Start is called before the first frame update
    void Start()
    {
        hero = GameObject.Find("P_P_Player");
        enemies = new GameObject[3];
        weapons = new GameObject[3];
        enemies[0] = mBasic;
        enemies[1] = mRogue;
        enemies[2] = mBrute;
        weapons[0] = mFork;
        weapons[1] = mKnives;
        weapons[2] = mHammer;
        // rogue = Instantiate(mRogue);
        // rogue.GetComponent<RogueEnemyBehavior>().Set_Attr(hero.transform);

        // brute = Instantiate(mBrute);
        // brute.GetComponent<BruteEnemyBehavior>().Set_Attr(hero.transform);
        
        // basic = Instantiate(mBasic);
        // basic.GetComponent<BasicEnemyBehavior>().Set_Attr(hero.transform);


    }
    // Update is called once per frame
    void Update()
    {
        if (enemy == null) {
            enemy = Instantiate(enemies[pos]);
            EnemyBehavior enemy_behavior = enemy.GetComponent<EnemyBehavior>();
            enemy_behavior.Set_Attr(hero.transform);
            // GameObject weapon_rig = Instantiate(mWeaponRig);
            // WeaponRigController controller = weapon_rig.GetComponent<WeaponRigController>();
            // controller.Set_Attr(weapons[pos], enemy.transform, enemy_behavior, true);
            pos++;
            if (pos == enemies.Length) {
                pos = 0;
            }
        }
    }

    // private IEnumerator SpawnBasic() {
    //     yield return new WaitForSeconds(1f);
    //     basic = Instantiate(mBasic);
    //     basic.GetComponent<BasicEnemyBehavior>().Set_Attr(hero.transform);
    // }

    // private IEnumerator SpawnRogue() {
    //     yield return new WaitForSeconds(1f);
    //     rogue = Instantiate(mRogue);
    //     rogue.GetComponent<RogueEnemyBehavior>().Set_Attr(hero.transform);
    // }

    // private IEnumerator SpawnBrute() {
    //     yield return new WaitForSeconds(1f);
    //     brute = Instantiate(mBrute);
    //     brute.GetComponent<BruteEnemyBehavior>().Set_Attr(hero.transform);
    // }
}
