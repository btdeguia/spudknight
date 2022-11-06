using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : Singleton<UIController>
{
    [Header("Hero Refs")]
    [SerializeField] private Slider hero_health_slider;
    private int hero_health = 100;
    [Header("Ability Refs")]
    [SerializeField] private RectTransform parry_cooldown_inside;


    public void HeroTakeDamage(int damage) {
        hero_health -= damage;
        hero_health_slider.value = hero_health;
    }

    public void PressParryButton() {
        parry_cooldown_inside.sizeDelta = new Vector2(300, parry_cooldown_inside.sizeDelta.y);
    }

    public void ReleaseParryButton(float cooldown_time) {
        StartCoroutine(parryCooldown(cooldown_time));
    }

    private IEnumerator parryCooldown(float cooldown_time) {
        float increment = cooldown_time / 50;
        float width_increment = 300 / 50;
        for (int i = 0; i < 50; i++) {
            parry_cooldown_inside.sizeDelta = new Vector2(parry_cooldown_inside.sizeDelta.x - width_increment, parry_cooldown_inside.sizeDelta.y);
            yield return new WaitForSeconds(increment);
        }
    }
}
