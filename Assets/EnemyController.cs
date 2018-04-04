using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Networking;

public class EnemyController : NetworkBehaviour {

    public float maxHP = 100.0f;

    public NavMeshAgent agent;

    public GameObject target;

    [SyncVar]
    private float hp;



    private void Start()
    {
        agent.enabled = true;
        if(isServer)
        {
            hp = maxHP;
        }
    }

    private void Update()
    {
        if(target != null)
        {
            agent.SetDestination(target.transform.position);
        }
    }

    [Command]
    public void CmdDamage(float amt)
    {
        hp -= amt;

        if(hp <= 0)
        {
            Destroy(gameObject);
        }
    }


}
