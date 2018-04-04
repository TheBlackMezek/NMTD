using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BuilderController : NetworkBehaviour {

    public float moveSpeed = 5.0f;
    public float yPos = 40.0f;
    public float turretPersonalSpace = 5.0f;

    public Camera cam;
    public AudioListener al;

    public GameObject turretPrefab;
    public GameObject fighterPrefab;


    private void Start()
    {
        if (isServer && NetworkServer.connections.Count > 1)
        {
            GameObject fighter = Instantiate(fighterPrefab);
            fighter.transform.position = new Vector3(0, 10, 0);
            NetworkServer.SpawnWithClientAuthority(fighter, gameObject);
            Destroy(gameObject);
        }

        if(!isLocalPlayer)
        {
            cam.enabled = false;
            al.enabled = false;
        }
        else
        {
            transform.position = new Vector3(0, yPos, 0);
            transform.eulerAngles = new Vector3(90, 0, 0);
        }
    }

    private void Update()
    {
        if(!isLocalPlayer)
        {
            return;
        }

        float x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.position += new Vector3(x, 0, z);


        if(Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit))
            {
                if(hit.transform.tag == "turret_placeable")
                {
                    bool turretTooClose = false;
                    RaycastHit[] hits = Physics.SphereCastAll(hit.point, turretPersonalSpace, Vector3.up);
                    foreach(RaycastHit h in hits)
                    {
                        if(h.transform.tag == "turret")
                        {
                            turretTooClose = true;
                            break;
                        }
                    }

                    if(!turretTooClose)
                    {
                        GameObject t = Instantiate(turretPrefab);
                        t.transform.position = hit.point;
                        NetworkServer.Spawn(t);
                    }
                }
            }
        }
    }

}
