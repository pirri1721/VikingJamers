using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipIA : MonoBehaviour
{
    public GameObject target;
    private BoatProbes boat;

    // Start is called before the first frame update
    void Start()
    {
        boat = GetComponent<BoatProbes>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position,target.transform.position) > 15f)
        {
            transform.LookAt(target.transform, transform.up);
        }
        else
        {
            Debug.Log("Assault");
        }
    }
}
