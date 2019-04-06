using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button btn_play;
    public Button btn_exit;

    void Start()
    {
        btn_play.onClick.AddListener(() => GoToLevel(1));
        btn_exit.onClick.AddListener(() => Exit());
    }

    void GoToLevel(int n)
    {
        SceneManager.LoadScene(n);
    }

    void Exit()
    {
        Application.Quit();
    }
}
