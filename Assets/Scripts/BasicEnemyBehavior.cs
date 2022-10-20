using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyBehavior : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sprite_renderer;
    [SerializeField] private Rigidbody2D rigidbody;
    [SerializeField] private WeaponBehavior weapon_behavior;
    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    private bool attacking = false;
    private bool walking = false;
    [SerializeField] private bool stunned = false;
    private bool exit = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        walk_towards();
    }

    private void walk_towards() {
        float distance_between = Vector3.Distance(transform.position, target.position);
        if (distance_between > 3 && !attacking && !stunned) {
            if (!walking) {
                animator.SetBool("is_walking", true);
                walking = true;
            }
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.deltaTime * speed);
        } else {
            if (!attacking && !stunned) {
                attacking = true;
                if (walking) {
                    animator.SetBool("is_walking", false);
                    walking = false;
                }
                StartCoroutine(attack());
            }
            
        }
    }

    private IEnumerator attack() {
        yield return new WaitForSeconds(0.25f);
        if (!stunned) {
            weapon_behavior.Set_Active(true);
        }
        yield return new WaitForSeconds(1f);
        attacking = false;
    }

    private IEnumerator hitstun() {
        yield return new WaitForSeconds(0.5f);
        stunned = false;
        exit = false;
    }

    public void OnCollisionEnter2D(Collision2D collision) {
        if (!stunned) {
            sprite_renderer.color /= 1.1f;
            stunned = true;
            StartCoroutine(hitstun());
        }
        
    }

    public void OnCollisionExit2D(Collision2D collision) {
        if (!exit) {
            sprite_renderer.color *= 1.1f;
            exit = true;
        }
    }
}
