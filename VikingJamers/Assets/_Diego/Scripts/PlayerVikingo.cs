﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerVikingo : MonoBehaviour
{
    public GameObject canvasGameOver;
    public GameObject canvasWin;
    public int vidas = 5;
    public int oleadaNum;
    public GameObject[] barcos;
    public GameObject[] spawners;
    public GameObject canvasOleada;

    public int enemigosRestantes;
    private float timer, cooldownTime = 5;

    void Awake()
    {
       canvasGameOver.SetActive(false);
        canvasWin.SetActive(false);
    }

    void Start()
    {
        if (PlayerPrefs.HasKey("level"))
        {
            oleadaNum = PlayerPrefs.GetInt("level");
            ActivarOleada(oleadaNum);
        }

        else
        {
            oleadaNum = 0;
            PlayerPrefs.SetInt("level", 0);
        }
    }

    public void Attacked()
    {
        //Debug.Log("DAÑO RECIBIDO");
        if (vidas > 0)
        {

            switch (vidas)
            {
                case 5: FindObjectOfType<CameraFilterPack_AAA_Blood_Plus>().Blood_1 = 1;
                    break;
                case 4:
                    FindObjectOfType<CameraFilterPack_AAA_Blood_Plus>().Blood_3 = 1;
                    break;
                case 3:
                    FindObjectOfType<CameraFilterPack_AAA_Blood_Plus>().Blood_5 = 1;
                    break;
                case 2:
                    FindObjectOfType<CameraFilterPack_AAA_Blood_Plus>().Blood_7 = 1;
                    break;
                case 1:
                    FindObjectOfType<CameraFilterPack_AAA_Blood_Plus>().Blood_9 = 1;
                    break;
                case 0:
                    FindObjectOfType<CameraFilterPack_AAA_Blood_Plus>().Blood_11 = 1;
                    break;
            }
            vidas--;

        }

        else
        {
            Debug.Log("GAME OVER");
            
            GetComponent<vp_FPInput>().MouseCursorForced = true;
            GetComponent<vp_FPInput>().MouseCursorBlocksMouseLook = true;
            canvasGameOver.SetActive(true);
            Time.timeScale = 0;
        }
            
    }

    void Update()
    {
        if (vidas <= 5 && vidas > 0)
        {
            if (timer > cooldownTime)
            {
                RecuperoVida();
                timer = 0;
            }

            if (timer < cooldownTime + 1)
                timer += Time.deltaTime;
        }

        if (Input.GetKeyDown("o"))
        {
            if (oleadaNum < barcos.Length)
                ActivarOleada(oleadaNum++);
            else
                oleadaNum = 0;
        }
    }

    void RecuperoVida()
    {
        //Debug.Log("recupero vida");

            switch (vidas)
            {
                case 5:
                    FindObjectOfType<CameraFilterPack_AAA_Blood_Plus>().Blood_1 = 0;
                    break;
                case 4:
                    FindObjectOfType<CameraFilterPack_AAA_Blood_Plus>().Blood_3 = 0;
                    break;
                case 3:
                    FindObjectOfType<CameraFilterPack_AAA_Blood_Plus>().Blood_5 = 0;
                    break;
                case 2:
                    FindObjectOfType<CameraFilterPack_AAA_Blood_Plus>().Blood_7 = 0;
                    break;
                case 1:
                    FindObjectOfType<CameraFilterPack_AAA_Blood_Plus>().Blood_9 = 0;
                    break;
                case 0:
                    FindObjectOfType<CameraFilterPack_AAA_Blood_Plus>().Blood_11 = 0;
                    break;
            }
        if(vidas < 5)
            vidas++;
    }

    public void UpdateEnemigos(int n)
    {
        enemigosRestantes += n;

        if(enemigosRestantes == 0 && oleadaNum < barcos.Length-1)
        {
            Debug.Log("Oleada completada");
            canvasOleada.SetActive(true);
            StartCoroutine(WaitUnPoco());
        }
    }

    IEnumerator WaitUnPoco()
    {
        yield return new WaitForSeconds(7);

        if (oleadaNum < barcos.Length-1)
        {
            oleadaNum++;
            Debug.Log(oleadaNum);
            PlayerPrefs.SetInt("level", oleadaNum);
            SceneManager.LoadSceneAsync(1);
        }
        else
        {
            Debug.Log("YOU WIN");
            canvasWin.SetActive(true);
        }
            
    }

    void ActivarOleada(int num)
    {
        for (int i = 0; i < barcos.Length; i++)
        {
            barcos[i].SetActive(false);
        }

        barcos[num].SetActive(true);

        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i].SetActive(false);
        }

        spawners[num*2].SetActive(true);
        spawners[num * 2 + 1].SetActive(true);
    }
}
