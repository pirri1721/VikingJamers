using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public Button btn_backMenu;

    void Start()
    {
        btn_backMenu.onClick.AddListener(BackToMenu);
    }

    void BackToMenu()
    {
        Debug.Log("Cargo el main menu");
    }
}
