using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class BtnController : MonoBehaviour
{

    public void OnRestartBtnClick()
    {
        Debug.Log("clicked");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
