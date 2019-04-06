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

    private bool firstTime = true;
    private bool right;

    // Start is called before the first frame update
    void Start()
    {
        boat = GetComponent<BoatProbes>();
        glove.SetActive(false);
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
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

        if(Vector3.Distance(transform.position,currentTarget.transform.position) > 8f && firstTime)
        {
            transform.LookAt(currentTarget.transform, transform.up);
        }
        else
        {
            if (firstTime)
            {
                Debug.Log("Assault");
                glove.SetActive(true);

                Tween cameraShake = DOTween.To(() => Time.timeScale, x => Time.timeScale = x, 0.25f, 0.25f);
                cameraShake.Play();

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

        targetRight.transform.parent.GetChild(0).SetParent(this.transform);

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

        Tween cameraShake = Camera.main.transform.DOShakePosition(0.5f, 1, 10, 90);
        cameraShake.Play();
    }
}
