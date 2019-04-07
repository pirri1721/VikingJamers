using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutBoatGM : MonoBehaviour
{
    public GameObject[] islands;
    public GameObject enemyShip;

    public GameObject[] shipSkin;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i < shipSkin.Length; i++)
        {
            shipSkin[i].SetActive(false);
        }

        enemyShip.SetActive(false);

        for (int i=0; i< islands.Length; i++)
        {
            islands[i].transform.position = new Vector3(Random.Range(-100, 100), islands[i].transform.position.y, islands[i].transform.position.z);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Player")
        {
            Debug.Log("Launch Event");
            enemyShip.SetActive(true);
            shipSkin[PlayerPrefs.GetInt("level")].SetActive(true);
        }
    }
}
