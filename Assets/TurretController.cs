using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TurretController : NetworkBehaviour {

    public float range = 20.0f;
    public float dps = 25.0f;
    public Transform block;
    public LineRenderer lr;

    [SyncVar]
    private Transform target = null;

    private EnemyController enemy = null;


    private void Start()
    {
        lr.SetPosition(0, block.position);
    }

    private void Update()
    {
        if(isServer)
        {
            if(target == null)
            {
                if(lr.enabled)
                {
                    block.eulerAngles = Vector3.zero;
                    lr.enabled = false;
                }

                RaycastHit[] hits = Physics.SphereCastAll(transform.position, range, Vector3.right);
                
                float minDist = float.MaxValue;
                foreach (RaycastHit hit in hits)
                {
                    float dist = Vector3.Distance(transform.position, hit.transform.position);
                    if (hit.transform.tag == "enemy" && dist < minDist)
                    {
                        target = hit.transform;
                        minDist = dist;
                    }
                }

                if(target != null)
                {
                    enemy = target.GetComponent<EnemyController>();
                    lr.SetPosition(1, target.position);
                    lr.enabled = true;
                }
            }
            else
            {
                block.LookAt(target);
                lr.SetPosition(1, target.position);
                enemy.CmdDamage(dps * Time.deltaTime);
                
                if (Vector3.Distance(transform.position, target.position) > range)
                {
                    target = null;
                    block.eulerAngles = Vector3.zero;
                    lr.enabled = false;
                }
            }
        }
        else
        {
            if(target == null && lr.enabled)
            {
                lr.enabled = false;
            }
            else if(target != null)
            {
                lr.enabled = true;
                lr.SetPosition(1, target.position);
            }
        }
    }

}
