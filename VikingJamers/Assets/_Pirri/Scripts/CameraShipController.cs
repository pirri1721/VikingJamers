using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShipController : MonoBehaviour
{
    public GameObject Target;
    public GameObject targetEntering;
    public GameObject sternPoint;
    public float smoothSpeed = 0.125f;

    public Vector3 offset;
    public bool entering;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!entering)
        {
            Vector3 desiredPosition = Target.transform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
        else
        {
            transform.LookAt(targetEntering.transform);
        }
    }

    public void EnteringEvent()
    {
        entering = true;

        transform.DOMove(sternPoint.transform.position,0.5f);//stern 
    }
}
