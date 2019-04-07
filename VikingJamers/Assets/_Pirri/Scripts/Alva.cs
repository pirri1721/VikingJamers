using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alva : MonoBehaviour
{
    public AudioSource audio;
    public float timeToAudio;
    public GameObject light;
    public float timeToLight;

    // Start is called before the first frame update
    void Start()
    {
        light.SetActive(false);
        StartCoroutine(WaitForAudio());
        StartCoroutine(WaitForLight());
    }

    public IEnumerator WaitForLight()
    {
        yield return new WaitForSeconds(timeToLight);
        light.SetActive(true);
    }

    public IEnumerator WaitForAudio()
    {
        yield return new WaitForSeconds(timeToAudio);
        Debug.Log("audio");
        GameObject.Find("OST").GetComponent<AudioSource>().volume = 0.1f;
        audio.Play();
        yield return new WaitForSeconds(3);
        GameObject.Find("OST").GetComponent<AudioSource>().volume = 0.4f;
    }
}
