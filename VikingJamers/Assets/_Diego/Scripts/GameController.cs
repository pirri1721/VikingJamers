using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Button btn_backMenu;
    public Button btn_reset;

    void Start()
    {
        btn_reset.onClick.AddListener(Reset);
        btn_backMenu.onClick.AddListener(BackMenu);
    }

    void Reset()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void BackMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }
}
