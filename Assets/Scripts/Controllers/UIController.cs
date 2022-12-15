using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : Singleton<UIController>
{
    [SerializeField] private Canvas canvas;
    [Header("Hero Refs")]
    [SerializeField] private Slider hero_health_slider;
    [SerializeField] private RectTransform heart_mask;
    [SerializeField] private RectTransform heart_position;
    
    [SerializeField] private TextMeshProUGUI hero_health_text;
    [SerializeField] private TextMeshProUGUI hero_currency_text;
    [SerializeField] private RectTransform currency;
    [SerializeField] private RectTransform small_currency;
    private int hero_health;
    private int max_hero_health;
    [Header("Ability Refs")]
    [SerializeField] private RectTransform parry_cooldown_inside;
    [SerializeField] private Image weapon_box_renderer;
    private IEnumerator parry_cooldown_co;
    [Header("Menu Refs")]
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject menu_text;
    [SerializeField] private GameObject credits_screen;
    private bool menu_active = false;
    private float temp_time_scale;

    private GameObject last_page = null;
    [Header("Other Refs")]
    [SerializeField] private GameObject popup_box;
    [SerializeField] private TextMeshProUGUI popup_text;
    [SerializeField] private Image popup_image;
    [SerializeField] private GameObject death_screen;
    [SerializeField] private GameObject btn_restart;
    [SerializeField] private GameObject win_screen;
    [SerializeField] private Image fade_bg;
    [Header("Boss Refs")]
    [SerializeField] private GameObject boss_screen;
    [SerializeField] private GameObject boss_attr;
    [SerializeField] private Slider boss_healthbar;

    void OnEnable() {
        StartCoroutine(startSequence());
    }

    private IEnumerator startSequence() {
        yield return new WaitForSeconds(1f);
        for (float i = 1; i > 0; i -= 0.1f) {
            yield return new WaitForSeconds(0.01f);
            Color tempColor = fade_bg.color;
            tempColor.a = i;
            fade_bg.color = tempColor;
        }
        fade_bg.gameObject.SetActive(false);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.M)) {
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

    public void ShowDeathScreen() {
        death_screen.SetActive(true);
        btn_restart.SetActive(true);
    }

    public void ShowWinScreen() {
        win_screen.SetActive(true);
        Time.timeScale = 0f;
    }


    public void ShowBossScreen() {
        StartCoroutine(StartBossScreen());
    }

    private IEnumerator StartBossScreen() {
        float prevTime = Time.timeScale;
        Time.timeScale = 0;
        boss_screen.SetActive(true);
        yield return new WaitForSecondsRealtime(5f);
        boss_screen.SetActive(false);
        boss_attr.SetActive(true);
        Time.timeScale = prevTime;
    }

    public void BossTakeDamage(int damage) {
        boss_healthbar.value -= damage;
    }

    public void PlayAgain() {
        StartCoroutine(restartGame());
    }

    public void Quit() {
        StartCoroutine(exitGame());
    }

    private IEnumerator restartGame() {
        fade_bg.gameObject.SetActive(true);
        for (float i = 0; i <= 1; i += 0.1f) {
            Debug.Log(i);
            yield return new WaitForSecondsRealtime(0.01f);
            Color tempColor = fade_bg.color;
            tempColor.a = i;
            fade_bg.color = tempColor;
            Debug.Log("end of loop");
        }
        Color finalColor = fade_bg.color;
        finalColor.a = 1;
        fade_bg.color = finalColor;
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        yield return null;
    }

    private IEnumerator exitGame() {
        fade_bg.gameObject.SetActive(true);
        for (float i = 0; i <= 1; i += 0.1f) {
            yield return new WaitForSecondsRealtime(0.01f);
            Color tempColor = fade_bg.color;
            tempColor.a = i;
            fade_bg.color = tempColor;
        }
        Color finalColor = fade_bg.color;
        finalColor.a = 1;
        fade_bg.color = finalColor;
        Time.timeScale = 1f;
        Application.Quit();
    }

    public void GainCurrency(Vector3 pos) {
        Vector3 screenpoint = Camera.main.WorldToScreenPoint(pos);
        screenpoint /= canvas.scaleFactor;
        Debug.Log("screen point: " + screenpoint);
        // Vector2 localPoint = Vector3.zero;
        // RectTransform currency_rect = heart_position.parent.GetComponent<RectTransform>();
        // RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), screenpoint, Camera.main, out localPoint);
        // Vector3 v3LocalPoint = localPoint;
        Vector3 v3LocalPoint = new Vector3(screenpoint.x - (1920 / 2), screenpoint.y - (1080 / 2), 0);
        Debug.Log("local point: " + v3LocalPoint);
        StartCoroutine(currencyAnimation(v3LocalPoint));
    }

    private IEnumerator currencyAnimation(Vector3 pos) {
        small_currency.gameObject.SetActive(true);
        small_currency.anchoredPosition = pos;
        Vector3 end_pos = new Vector3(-850, 258, 0);
        yield return new WaitForSeconds(0.5f);
        for (float i = 0; i < 1; i += 0.1f) {
            small_currency.anchoredPosition = Vector3.Lerp(pos, end_pos, i);
            yield return new WaitForSeconds(0.05f);
        }
        small_currency.anchoredPosition = end_pos;
        small_currency.gameObject.SetActive(false);
        currency.localScale = new Vector2(1.5f, 1.5f);
        SetCurrencyText();
        yield return new WaitForSeconds(0.25f);
        currency.localScale = new Vector2(1f, 1f);
    }

    public void BackButton() {
        if (last_page != null) {
            credits_screen.SetActive(false);
            last_page.SetActive(true);
        }
    }

    public void ViewCredits(GameObject close) {
        last_page = close;
        last_page.SetActive(false);
        credits_screen.SetActive(true);
    }
}
