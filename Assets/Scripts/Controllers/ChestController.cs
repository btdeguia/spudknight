using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject dropped_item;
    [SerializeField] private GameObject open_sprite;
    [SerializeField] private GameObject closed_sprite;

    [SerializeField] private bool within_open_dsitance = false;
    [SerializeField] private bool opened = false;

    void Update() {
        if (Input.GetKey(KeyCode.E) && !opened && within_open_dsitance) {
            Debug.Log(weapon.transform.GetChild(0).GetComponent<WeaponBehavior>().GetCurrency());
            if (FinanceController.Instance.GetCurrency() >= weapon.transform.GetChild(0).GetComponent<WeaponBehavior>().GetCurrency())
            {
                opened = true;
                open_sprite.SetActive(true);
                closed_sprite.SetActive(false);
                GameObject item = Instantiate(dropped_item, transform.position, new Quaternion(0, 0, 0, 0));
                item.GetComponent<DroppedItemBehavior>().SetWeapon(weapon);
                FinanceController.Instance.BuyWeapon(weapon);
                UIController.Instance.SetCurrencyText();
                // if (item_behavior != null) {
                //     item_behavior.
                // }

            } else {
                UIController.Instance.OpenPopup(null, "Not enough currency!");
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("collided with the chest");
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
