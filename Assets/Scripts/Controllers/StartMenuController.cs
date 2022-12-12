using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] private Animator bg_animator;
    [SerializeField] private Image fade_bg;
    [SerializeField] private GameObject instructions;

    void OnEnable() {
        StartCoroutine(startSequence());
    }

    private IEnumerator startSequence() {
        bg_animator.SetBool("fade_in", true);
        for (float i = 1; i > 0; i -= 0.1f) {
            yield return new WaitForSeconds(0.01f);
            Color tempColor = fade_bg.color;
            tempColor.a = i;
            fade_bg.color = tempColor;
        }
        fade_bg.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.001f);
        bg_animator.SetBool("start_idle", true);
    }

    public void Play() {
        StartCoroutine(startGame());
    }

    private IEnumerator startGame() {
        fade_bg.gameObject.SetActive(true);
        for (float i = 0; i <= 1; i += 0.1f) {
            yield return new WaitForSeconds(0.01f);
            Color tempColor = fade_bg.color;
            tempColor.a = i;
            fade_bg.color = tempColor;
        }
        Color finalColor = fade_bg.color;
        finalColor.a = 1;
        fade_bg.color = finalColor;
        SceneManager.LoadScene("Main");
    }

    public void Instructinos() {
        instructions.SetActive(true);
    }

    public void BackFromInstructions() {
        instructions.SetActive(false);
    }

    public void Quit() {
        Application.Quit();
    }
}
