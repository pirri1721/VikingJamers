using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShipController : MonoBehaviour
{
    public GameObject Target;
    public float smoothSpeed = 0.125f;

    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 desiredPosition = Target.transform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
