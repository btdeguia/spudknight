using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class BossRoomController : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private Collider2D collider_2d;
    [SerializeField] private GameObject wall;
    [SerializeField] private AudioClip audioClip;
    private bool spawned = false;
    private bool dead = false;
    private void Update()
    {
        if (dead) {
            wall.SetActive(false);
        }
    }

    public void bossDead()
    {
        dead = true;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log(collision.gameObject.name + " encountered the gate ");
        if (collision.gameObject.name[2] == 'P') {
            if (!spawned) {
                Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), collider_2d);
                StartCoroutine(StartBossFight());
                wall.SetActive(true);
                spawned = true;
            }
            
        }
    }

    private IEnumerator StartBossFight() {
        AudioSource auid_source = Camera.main.GetComponent<AudioSource>();
        if (auid_source != null)
        {
            auid_source.clip = audioClip;
            auid_source.Play();
        }
        yield return new WaitForSeconds(0.5f);
        UIController.Instance.ShowBossScreen();
        yield return new WaitForSecondsRealtime(5f);
        boss.SetActive(true);
        boss.GetComponent<BossEnemyBehavior>().SetSpawner(gameObject);
    }
}