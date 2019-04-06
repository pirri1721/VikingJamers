using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVikingo : MonoBehaviour
{
    public GameObject canvasGameOver;
    private int vidas = 0;

    void Awake()
    {
       canvasGameOver.SetActive(false);
    }

    public void Attacked()
    {
        //Debug.Log("DAÑO RECIBIDO");
        if (vidas <= 5)
        {
            vidas++;

            switch (vidas)
            {
                case 0: FindObjectOfType<CameraFilterPack_AAA_Blood_Plus>().Blood_1 = 1;
                    break;
                case 1:
                    FindObjectOfType<CameraFilterPack_AAA_Blood_Plus>().Blood_3 = 1;
                    break;
                case 2:
                    FindObjectOfType<CameraFilterPack_AAA_Blood_Plus>().Blood_5 = 1;
                    break;
                case 3:
                    FindObjectOfType<CameraFilterPack_AAA_Blood_Plus>().Blood_7 = 1;
                    break;
                case 4:
                    FindObjectOfType<CameraFilterPack_AAA_Blood_Plus>().Blood_9 = 1;
                    break;
                case 5:
                    FindObjectOfType<CameraFilterPack_AAA_Blood_Plus>().Blood_11 = 1;
                    break;
            }
          
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
}
