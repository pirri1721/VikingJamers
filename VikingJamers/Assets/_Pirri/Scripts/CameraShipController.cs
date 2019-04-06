using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CameraShipController : MonoBehaviour
{
    public GameObject Target;
    public GameObject targetEntering;
    public GameObject sternPoint;
    public float smoothSpeed = 0.125f;

    public Vector3 offset;
    public bool entering;
    private float timeTravelling = 1.5f;

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
            if(desiredPosition.x > 100 || desiredPosition.x < -100)
            {
                if(desiredPosition.x > 200 || desiredPosition.x < -200)
                {
                    DestroyPlayerShip();
                }
            }
            else
            {
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
                transform.position = smoothedPosition;
            }
        }
        else
        {
            transform.LookAt(targetEntering.transform);
        }
    }

    private void DestroyPlayerShip()
    {
        //Destroy player ship
        //GameOver.SetActive(true);
    }

    public void EnteringEvent()
    {
        entering = true;

        //targetEntering.transform.DOMove(targetEntering.transform.position + targetEntering.transform.parent.parent.transform.position - targetEntering.transform.parent.transform.position, 1f).Play();

        Tween enteringTween = transform.DOMove(sternPoint.transform.position,timeTravelling).Play();
        TweenCallback callback = new TweenCallback(() => AsyncLoad());
        enteringTween.OnComplete(callback);
        enteringTween.Play();
    }

    public void AsyncLoad()
    {
        if (Vector3.Distance(transform.position, sternPoint.transform.position) > 3f)
        {
            timeTravelling -= timeTravelling / 2;
            EnteringEvent();
        }
        else
        {
            Time.timeScale = 1f;
            transform.SetParent(targetEntering.transform.parent);
            Debug.Log("ChangeScene");
        }
    }
}
