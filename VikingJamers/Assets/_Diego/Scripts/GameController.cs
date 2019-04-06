using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Button btn_backMenu;

    void Start()
    {
        btn_backMenu.onClick.AddListener(Reset);
    }

    void Reset()
    {
        Debug.Log("Cargo el main menu");
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
