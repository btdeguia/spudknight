using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Everytime an enemy dies, make a new one
    [SerializeField] private BasicEnemyBehavior basic_enemy;
    [SerializeField] private GameObject mHero;
    [SerializeField] private GameObject mRogue;
    [SerializeField] private GameObject mBrute;
    [SerializeField] private GameObject mBasic;
    private GameObject hero;
    private GameObject rogue;
    private GameObject brute;
    private GameObject basic;

    private GameObject[] enemies;
    private GameObject enemy;
    int pos = 0;

    // Start is called before the first frame update
    void Start()
    {
        hero = GameObject.Find("P_Player");
        enemies = new GameObject[3];
        enemies[0] = mBasic;
        enemies[1] = mRogue;
        enemies[2] = mBrute;
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
            enemy.GetComponent<EnemyBehavior>().Set_Attr(hero.transform);
            pos++;
            if (pos == enemies.Length) {
                pos = 0;
            }
        }
    }

    private IEnumerator SpawnBasic() {
        yield return new WaitForSeconds(1f);
        basic = Instantiate(mBasic);
        basic.GetComponent<BasicEnemyBehavior>().Set_Attr(hero.transform);
    }

    private IEnumerator SpawnRogue() {
        yield return new WaitForSeconds(1f);
        rogue = Instantiate(mRogue);
        rogue.GetComponent<RogueEnemyBehavior>().Set_Attr(hero.transform);
    }

    private IEnumerator SpawnBrute() {
        yield return new WaitForSeconds(1f);
        brute = Instantiate(mBrute);
        brute.GetComponent<BruteEnemyBehavior>().Set_Attr(hero.transform);
    }
}
