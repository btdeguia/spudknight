using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sprite_renderer;
    [SerializeField] private float speed;
    private bool walking = false;
    private bool facing_back = false;
    private bool stunned;
    private bool exit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        walk();
        move_cursor();
    }

    private void walk() {
        walking = false;
        if (Input.GetKey(KeyCode.W)) {
            transform.Translate(0, speed * Time.deltaTime, 0);
            walking = true;
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.Translate(-speed * Time.deltaTime, 0, 0);
            walking = true;
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.Translate(speed * Time.deltaTime, 0, 0);
            walking = true;
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.Translate(0, -speed * Time.deltaTime, 0);
            walking = true;
        }
        animator.SetBool("is_walking", walking);
    }

    private void move_cursor() {
        // Vector3 pos = Input.mousePosition;
        // float mouse_pos_y = pos.y - 220 - (transform.position.y * 40);
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();
        float mouse_pos_y = difference.y;
        // Debug.Log(mouse_pos_y);
        if (mouse_pos_y < 0 && facing_back) {
            animator.SetTrigger("switch_facing");
            facing_back = false;
        }
        if (mouse_pos_y > 0 && !facing_back) {
            animator.SetTrigger("switch_facing");
            facing_back = true;
        }
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
