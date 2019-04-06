using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShipIA : MonoBehaviour
{
    public GameObject targetRight;
    public GameObject targetLeft;
    private GameObject currentTarget;
    private BoatProbes boat;

    public GameObject glove;
    public GameObject board1;
    public GameObject board2;

    private bool firstTime = true;
    private bool right = true;
    private float rangeToShot = 25f;

    // Start is called before the first frame update
    void Start()
    {
        boat = GetComponent<BoatProbes>();
        glove.SetActive(false);
        Time.timeScale = 1f;
        currentTarget = targetRight;
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(Vector3.Distance(transform.position,targetRight.transform.position) < Vector3.Distance(transform.position, targetLeft.transform.position))
        {
            right = true;
            currentTarget = targetRight;
        }
        else
        {
            right = false;
            currentTarget = targetLeft;
            
        }
        */

        if(Vector3.Distance(transform.position,currentTarget.transform.position) > rangeToShot && firstTime)
        {
            Debug.DrawLine(transform.position, transform.up);
           // Debug.Log(Vector3.Distance(transform.position, currentTarget.transform.position));
            transform.LookAt(currentTarget.transform, transform.up);
        }
        else
        {
            if (firstTime)
            {
                Debug.Log("Assault");
                glove.SetActive(true);

                Tween timeTween = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0.5f, 0.2f);
                timeTween.Play();

                //Sequence sequence = DOTween.Sequence();
                Tween gloveT = glove.transform.DOMove(currentTarget.transform.parent.transform.position, 0.5f);
                TweenCallback callback = new TweenCallback(() => ChangeParents());
                gloveT.OnComplete(callback);
                gloveT.Play();

                firstTime = false;
            }
        }
    }

    public void ChangeParents()
    {
        glove.transform.SetParent(targetRight.transform.parent);

        Transform playerShipModel = targetRight.transform.parent.GetChild(0);

        playerShipModel.SetParent(this.transform);
        playerShipModel.transform.localPosition = new Vector3(-12.4f, 4.35f, -1f);
        playerShipModel.transform.rotation = Quaternion.Euler(-90, 0, 90);

        targetRight.transform.parent.gameObject.SetActive(false);


        if (right)
        {
            for (int i = 0; i < 4; i = i + 3)
            {
                Vector3 currentOffset = boat.GetComponent<BoatProbes>()._forcePoints[i]._offsetPosition;
                boat.GetComponent<BoatProbes>()._forcePoints[i]._offsetPosition = new Vector3(currentOffset.x - 2.5f, currentOffset.y, currentOffset.z);
            }
        }
        else
        {
            for (int i = 2; i < 4; i = i + 3)
            {
                Vector3 currentOffset = boat.GetComponent<BoatProbes>()._forcePoints[i]._offsetPosition;
                boat.GetComponent<BoatProbes>()._forcePoints[i]._offsetPosition = new Vector3(currentOffset.x + 2.5f, currentOffset.y, currentOffset.z);
            }
        }

        Tween cameraShake = Camera.main.transform.DOShakePosition(1f, 10, 10, 180);

        TweenCallback callback = new TweenCallback(() => CameraCallback());
        cameraShake.OnComplete(callback);
        cameraShake.Play();
    }

    public void CameraCallback()
    {
        board1.transform.DORotateQuaternion(Quaternion.Euler(0, 0, -103f), 1f).Play();
        //board1.transform.DOLocalRotate(new Vector3(0, 0, -103f), 1f).Play();
        board2.transform.DORotateQuaternion(Quaternion.Euler(0, 0, -103f), 1f).Play();

        Camera.main.GetComponent<CameraShipController>().EnteringEvent();
    }
}
