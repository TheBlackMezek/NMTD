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

    private float pathCalcWait = 1.0f;
    private float timer = 0;



    private void Start()
    {
        agent.enabled = true;
        if(isServer)
        {
            hp = maxHP;
        }
        InvokeRepeating("RecalcPath", pathCalcWait, pathCalcWait);
    }

    private void RecalcPath()
    {
        agent.ResetPath();
        agent.SetDestination(target.transform.position);
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

    public void Damage(float amt)
    {
        CmdDamage(amt);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(isServer && collision.gameObject.tag == "heart")
        {
            collision.gameObject.GetComponent<HeartController>().CmdDamage(1);
            Destroy(gameObject);
        }
    }


}
