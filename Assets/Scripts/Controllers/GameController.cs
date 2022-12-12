using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

[Serializable]
public struct Enemy {
    public GameObject enemy_prefab;
    public GameObject weapon_prefab;
}

public class GameController : Singleton<GameController>
{
    
    [SerializeField] private List<Enemy> enemy_list = new List<Enemy>();
    // Everytime an enemy dies, make a new one
    [SerializeField] private GameObject mWeaponRig;
    [SerializeField] private GameObject mHero;
    private GameObject hero;
    private GameObject rogue;
    private GameObject brute;
    private GameObject basic;

    private GameObject[] enemies;
    private GameObject[] weapons;
    private GameObject enemy;
    int pos = 0;

    // Start is called before the first frame update
    //void Start()
    //{
        


    //}

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        hero = GameObject.Find("P_P_Player");
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
        // if (enemy == null) {
        //     enemy = Instantiate(enemy_list[pos].enemy_prefab);
        //     EnemyBehavior enemy_behavior = enemy.GetComponent<EnemyBehavior>();
        //     enemy_behavior.SetTarget(hero.transform);
        //     GameObject weapon_rig = Instantiate(mWeaponRig);
        //     WeaponRigController controller = weapon_rig.GetComponent<WeaponRigController>();
        //     controller.Set_Attr(enemy_list[pos].weapon_prefab, enemy.transform, enemy_behavior, true);
        //     enemy_behavior.SetWeaponRig(controller);
        //     pos++;
        //     if (pos == enemy_list.Count) {
        //         pos = 0;
        //     }
        // }
    }

    public GameObject GetWeaponFromList(int pos) {
        return enemy_list[pos].weapon_prefab;
    }

    public GameObject GetEnemyFromList(int pos) {
        return enemy_list[pos].enemy_prefab;
    }

    public Enemy GetEnemyStruct(int pos) {
        return enemy_list[pos];
    }

    public GameObject GetHero() {
        return hero;
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
