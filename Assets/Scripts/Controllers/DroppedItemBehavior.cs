using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItemBehavior : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite_renderer;
    [SerializeField] private GameObject weapon;
    private bool within_pickup_dsitance = false;
    private PlayerBehavior behavior;
    // Start is called before the first frame update
    void OnEnable()
    {
        sprite_renderer.sprite = weapon.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E) && within_pickup_dsitance   ) {
            Pickup();
        }
    }

    public void SetWeapon(GameObject new_weapon) {
        weapon = new_weapon;
    }

    public void OnTriggerEnter2D(Collider2D collider) {
        behavior = collider.gameObject.GetComponent<PlayerBehavior>();
        if (behavior != null) {
           within_pickup_dsitance = true; 
        }
        
    }

    public void OnTriggerExit2D(Collider2D collider) {
        behavior = null;
        within_pickup_dsitance = false;
    }

    public void Pickup() {
        if (behavior != null) {
            behavior.Pickup_Weapon(weapon);
        }
        Destroy(gameObject);
    }
}
