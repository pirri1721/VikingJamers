using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class AgentController : MonoBehaviour
{
    public Transform target;

    private NavMeshAgent agent;
    float timer = 0.0f;
    float cooldownTime = 2.0f;


    void Awake()
    {
       target = GameObject.FindGameObjectWithTag("Player").transform;
       agent = this.GetComponent<NavMeshAgent>();
      // agent.SetDestination(target.position);
    }

    void Update()
    {
        agent.SetDestination(target.position);

        if (agent.remainingDistance > agent.stoppingDistance)
            agent.gameObject.GetComponent<Animator>().SetFloat("Speed", 1.0f);
        else {
            agent.gameObject.GetComponent<Animator>().SetFloat("Speed", 0.0f);
            if (timer > cooldownTime)
            {
                if (agent.gameObject.GetComponent<Animator>().parameters.Any(x => x.name == "attack1"))
                    agent.gameObject.GetComponent<Animator>().SetTrigger("attack1");
                FindObjectOfType<PlayerVikingo>().Attacked();
                timer = 0;
            }
        }

        if (timer < cooldownTime + 1)
            timer += Time.deltaTime;

    }
}
