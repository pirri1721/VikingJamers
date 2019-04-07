using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class AgentController : MonoBehaviour
{
    public Transform target;
    public AudioSource audioSource;
    public AudioClip[] sounds;
    Animator anim;

    private NavMeshAgent agent;
    float timer = 0.0f;
    float cooldownTime = 2.0f;


    void Awake()
    {
       target = GameObject.FindGameObjectWithTag("Player").transform;
       agent = this.GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
      // agent.SetDestination(target.position);
    }

    void Update()
    {
        agent.SetDestination(target.position);

        Debug.Log(agent.remainingDistance);

        if (agent.remainingDistance > agent.stoppingDistance)
            anim.SetFloat("Speed", 1.0f);
        else
        {
            anim.SetFloat("Speed", 0.0f);
            if (timer > cooldownTime)
            {
                if (anim.parameters.Any(x => x.name == "attack1"))
                {
                    anim.SetTrigger("attack1");
                    audioSource.PlayOneShot(sounds[0]);
                }

                if (agent.gameObject.tag != "EnemyDeath")
                {
                    FindObjectOfType<PlayerVikingo>().Attacked();
                    audioSource.PlayOneShot(sounds[1]);
                }

                timer = 0;
            }
        }

        if (timer < cooldownTime + 1)
            timer += Time.deltaTime;

    }
}
