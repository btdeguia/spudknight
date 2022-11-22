using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : Singleton<UIController>
{
    [Header("Hero Refs")]
    [SerializeField] private Slider hero_health_slider;
    [SerializeField] private RectTransform heart_mask;
    [SerializeField] private RectTransform heart_position;
    [SerializeField] private TextMeshProUGUI hero_health_text;
    [SerializeField] private TextMeshProUGUI hero_currency_text;
    private int hero_health;
    private int max_hero_health;
    [Header("Ability Refs")]
    [SerializeField] private RectTransform parry_cooldown_inside;
    [SerializeField] private Image weapon_box_renderer;
    private IEnumerator parry_cooldown_co;
    [Header("Menu Refs")]
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject menu_text;
    private bool menu_active = false;
    private float temp_time_scale;
    [Header("Other Refs")]
    [SerializeField] private GameObject popup_box;
    [SerializeField] private TextMeshProUGUI popup_text;
    [SerializeField] private Image popup_image;


    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (menu_active) {
                menu_active = false;
                menu.SetActive(false);
                menu_text.SetActive(true);
                Time.timeScale = temp_time_scale;
            } else {
                menu_active = true;
                menu.SetActive(true);
                menu_text.SetActive(false);
                temp_time_scale = Time.timeScale;
                Time.timeScale = 0f;
            }
        }
    }

    public void HeroTakeDamage(int damage) {
        hero_health -= damage;
        // hero_health_slider.value = hero_health;
        // heart_mask.sizeDelta = new Vector2(heart_mask.sizeDelta.x, heart_mask.sizeDelta.y - (damage * 1.4f));
        heart_mask.anchoredPosition = new Vector3(heart_mask.anchoredPosition.x, heart_mask.anchoredPosition.y - (damage * 1.4f), 0);
        heart_position.anchoredPosition = new Vector3(heart_position.anchoredPosition.x, heart_position.anchoredPosition.y + (damage * 1.4f), 0);
        hero_health_text.text = "Health " + hero_health + "/" + max_hero_health;
    }

    public void SetHeroHealth(int health) {
        hero_health = health;
        max_hero_health = hero_health;
        hero_health_text.text = "Health " + max_hero_health + "/" + max_hero_health;
    }

    public void PressParryButton() {
        // parry_cooldown_inside.sizeDelta = new Vector2(300, parry_cooldown_inside.sizeDelta.y);
        parry_cooldown_co = parryHold(1f);
        StartCoroutine(parry_cooldown_co);
    }

    public void ReleaseParryButton(float cooldown_time) {
        if (parry_cooldown_co != null) {
            StopCoroutine(parry_cooldown_co);
            parry_cooldown_co = null;
            StartCoroutine(parryCooldown(cooldown_time));
        }
        
    }

    private IEnumerator parryCooldown(float cooldown_time) {
        parry_cooldown_inside.sizeDelta = new Vector2(300, parry_cooldown_inside.sizeDelta.y);
        float increment = cooldown_time / 50;
        float width_increment = 300 / 50;
        for (int i = 0; i < 50; i++) {
            parry_cooldown_inside.sizeDelta = new Vector2(parry_cooldown_inside.sizeDelta.x - width_increment, parry_cooldown_inside.sizeDelta.y);
            yield return new WaitForSeconds(increment);
        }
    }

    private IEnumerator parryHold(float hold_time) {
        float increment = hold_time / 50;
        float width_increment = 300 / 50;
        for (int i = 0; i < 50; i++) {
            parry_cooldown_inside.sizeDelta = new Vector2(parry_cooldown_inside.sizeDelta.x + width_increment, parry_cooldown_inside.sizeDelta.y);
            yield return new WaitForSeconds(increment);
        }
    }

    public void SetWeaponBoxSprite(Sprite sprit) {
        weapon_box_renderer.sprite = sprit;
    }

    public void SetCurrencyText()
    {
        hero_currency_text.text = "Fertilizer:    " + FinanceController.Instance.GetCurrency(); 
    }

    public void OpenPopup(Sprite w_image, string pop_text) {
        popup_box.SetActive(true);
        if (w_image != null) {
            popup_image.gameObject.SetActive(true);
            popup_image.sprite = w_image;
        }
        popup_text.text = pop_text;
        StartCoroutine(ClosePopup());
    }

    private IEnumerator ClosePopup() {
        yield return new WaitForSeconds(5f);
        popup_image.gameObject.SetActive(false);
        popup_box.SetActive(false);
    } 
}
