using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemiesSpawner : MonoBehaviour
{
    public GameObject[] prefabEnemies;
    private GameObject prefabEnemy;
    public int numEnemies;

    private int enemiesOut;
    private bool stop;
    float timer = 0.0f;
    float cooldownTime = 8f;

    void Start()
    {
        FindObjectOfType<PlayerVikingo>().UpdateEnemigos(numEnemies);

        prefabEnemy = prefabEnemies[PlayerPrefs.GetInt("level")];
    }

    void Update()
    {
        if (!stop)
        {
            if (timer > cooldownTime)
            {
                Instantiate(prefabEnemy, this.transform.position, Quaternion.identity);
                enemiesOut++;
                timer = 0;
                cooldownTime = Random.Range(8.0f, 15.0f);
               // Debug.Log("Saco enemigo");
            }

            if (timer < cooldownTime + 1)
                timer += Time.deltaTime;

            if (enemiesOut == numEnemies)
                stop = true;
        }
           
    }
}
