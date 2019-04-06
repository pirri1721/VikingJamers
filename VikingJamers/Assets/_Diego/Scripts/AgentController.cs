using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AgentController : MonoBehaviour
{
    public Transform target;

    private NavMeshAgent agent;

    void Start()
    {
       agent = this.GetComponent<NavMeshAgent>();
       agent.SetDestination(target.position);
    }

    void Update()
    {
        agent.SetDestination(target.position);

        if (agent.remainingDistance > agent.stoppingDistance)
            agent.gameObject.GetComponent<Animator>().SetFloat("Speed", 1.0f);
        else
            agent.gameObject.GetComponent<Animator>().SetFloat("Speed", 0.0f);
    }


}
