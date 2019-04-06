using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShipController : MonoBehaviour
{
    public GameObject Target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position =  Vector3.Lerp(transform.position, Target.transform.position + Target.transform.up, Time.deltaTime);
    }
}
