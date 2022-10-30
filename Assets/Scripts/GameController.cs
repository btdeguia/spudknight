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

    // Start is called before the first frame update
    void Start()
    {
        hero = GameObject.Find("P_Player");

        rogue = Instantiate(mRogue);
        rogue.GetComponent<RogueEnemyBehavior>().Set_Attr(hero.transform);
        // brute = Instantiate(mBrute);
        // brute.GetComponent<RogueEnemyBehavior>().Set_Attr(hero.transform);
        
        basic = Instantiate(mBasic);
        basic.GetComponent<BasicEnemyBehavior>().Set_Attr(hero.transform);


    }
    // Update is called once per frame
    void Update()
    {
        if (!hero) {
            // hero = Instantiate(mHero);
            
        }
        if (!rogue) {
            rogue = mRogue;
            StartCoroutine(SpawnRogue());
        }
        if (!brute) {
            // brute = Instantiate(mBrute);
        }
        if (!basic) {
            basic = mBasic;
            StartCoroutine(SpawnBasic());
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
}
