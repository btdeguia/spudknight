using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer sprite_renderer;
    [SerializeField] private Rigidbody2D rigidbody_2d;
    [SerializeField] private float speed;
    [SerializeField] private int health;
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

    private IEnumerator knockback(Collider2D collider) {
        if (collider.transform.parent != null) {
            Vector2 direction = collider.transform.parent.localPosition; // get local position of weapon
            direction.x = direction.x + 1.25f; // normalize it (left = positive, right = negative)
            float knockback = collider.transform.parent.GetComponent<WeaponBehavior>().GetKnockback(); // get knockback value from parent
            rigidbody_2d.AddForce(direction * knockback * Time.smoothDeltaTime, ForceMode2D.Impulse);
            yield return new WaitForSeconds(0.25f);
            rigidbody_2d.velocity = Vector2.zero;
        }
        
    }

    public void OnTriggerEnter2D(Collider2D collider) {
        if (!stunned) {
            health--; // player will always take 1 damage
            sprite_renderer.color /= 1.1f;
            
            StartCoroutine(knockback(collider));
            stunned = true;
            StartCoroutine(hitstun()); 
        }
        
    }

    public void OnTriggerExit2D(Collider2D collider) {
        if (!exit) {
            sprite_renderer.color *= 1.1f;
            exit = true;
        }
    }
}
