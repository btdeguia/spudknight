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
    [SerializeField] private List<GameObject> weapon_list = new List<GameObject>();
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
    }

    public int GetWeaponListSize() {
        return weapon_list.Count;
    }

    public GameObject GetWeaponFromList(int pos) {
        return weapon_list[pos];
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
}
