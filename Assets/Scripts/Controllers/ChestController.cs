using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject dropped_item;
    [SerializeField] private GameObject open_sprite;
    [SerializeField] private GameObject closed_sprite;
    [SerializeField] private TextMesh chest_text;

    [SerializeField] private bool within_open_dsitance = false;
    [SerializeField] private bool opened = false;

    void OnEnable() {

        int weapon_pos = Random.Range(1, GameController.Instance.GetWeaponListSize());
        weapon = GameController.Instance.GetWeaponFromList(weapon_pos);
    }
    
    void Update() {
        if (Input.GetKey(KeyCode.E) && !opened && within_open_dsitance) {
            if (FinanceController.Instance.GetCurrency() >= weapon.transform.GetChild(0).GetComponent<WeaponBehavior>().GetCurrency())
            {
                opened = true;
                open_sprite.SetActive(true);
                closed_sprite.SetActive(false);
                chest_text.gameObject.SetActive(false);
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
                chest_text.gameObject.SetActive(true);
                chest_text.text = "Fertilizer Required To Open: " + weapon.transform.GetChild(0).GetComponent<WeaponBehavior>().GetCurrency();
            }
        }
        
    }

    public void OnTriggerExit2D(Collider2D collider) {
        if (!opened) {
            within_open_dsitance = false;
            chest_text.gameObject.SetActive(false);
        }
    }
}
