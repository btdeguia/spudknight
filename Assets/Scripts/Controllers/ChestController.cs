using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject dropped_item;
    [SerializeField] private GameObject open_sprite;
    [SerializeField] private GameObject closed_sprite;

    private bool within_open_dsitance = false;
    private bool opened = false;

    void Update() {
        if (Input.GetKey(KeyCode.E) && !opened) {
            opened = true;
            open_sprite.SetActive(true);
            closed_sprite.SetActive(false);
            GameObject item = Instantiate(dropped_item, transform.position, new Quaternion(0, 0, 0, 0));
            item.GetComponent<DroppedItemBehavior>().SetWeapon(weapon);;
            // if (item_behavior != null) {
            //     item_behavior.
            // }
        }
    }

    public void OnTriggerEnter2D(Collider2D collider) {
        if (!opened) {
            PlayerBehavior behavior = collider.gameObject.GetComponent<PlayerBehavior>();
            if (behavior != null) {
            within_open_dsitance = true; 
            }
        }
        
    }

    public void OnTriggerExit2D(Collider2D collider) {
        if (!opened) {
            within_open_dsitance = false;
        }
    }
}
