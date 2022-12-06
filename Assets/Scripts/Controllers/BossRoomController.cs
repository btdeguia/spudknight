using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class BossRoomController : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    [SerializeField] private Collider2D collider_2d;

    private bool locked = false;

    void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log(collision.gameObject.name + " encountered the gate ");
        if (collision.gameObject.name[2] == 'P') {
            if (!locked) {
                Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), collider_2d);
                StartCoroutine(StartBossFight());
            }
            
        }
    }

    void OnCollisionExit2D(Collision2D collision) {
        Debug.Log("collision exit");
        locked = true;
    }

    private IEnumerator StartBossFight() {
        yield return new WaitForSeconds(0.5f);
        UIController.Instance.ShowBossScreen();
        yield return new WaitForSecondsRealtime(5f);
        boss.SetActive(true);
    }
}