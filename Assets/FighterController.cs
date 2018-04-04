using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FighterController : NetworkBehaviour {

    public float moveSpeed = 5.0f;
    public float jumpForce = 10.0f;
    public float mouseSensitivity = 1.0f;
    public float dps = 50.0f;

    public Camera cam;
    public AudioListener ls;

    public CharacterController cc;
    public LineRenderer lr;
    

    [SyncVar]
    bool lrOn = false;
    [SyncVar]
    Vector3 lrEnd = Vector3.zero;


    private void Start()
    {
        if(!isLocalPlayer)
        {
            cam.enabled = false;
            ls.enabled = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    private void Update()
    {
        lr.SetPosition(0, transform.position - Vector3.up * 0.5f);
        lr.SetPosition(1, lrEnd);
        lr.enabled = lrOn;

        if (!isLocalPlayer)
        {
            return;
        }
        
        float x = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float z = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;
        
        cc.SimpleMove(transform.forward * z + transform.right * x);

        if (cc.isGrounded && Input.GetAxisRaw("Jump") > 0)
        {
            cc.Move(transform.up * jumpForce);
        }

        float mx = Input.GetAxis("Mouse X") * mouseSensitivity;
        float my = Input.GetAxis("Mouse Y") * mouseSensitivity;
        
        transform.eulerAngles += new Vector3(0, mx, 0);
        cam.transform.eulerAngles -= new Vector3(my, 0, 0);

        
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            lrOn = true;

            if (Physics.Raycast(ray, out hit))
            {
                lrEnd = hit.point;

                if (hit.transform.tag == "enemy")
                {
                    hit.transform.GetComponent<EnemyController>().CmdDamage(dps * Time.deltaTime);
                }
            }
            else
            {
                lrEnd = transform.forward * 1000.0f;
            }
        }
        else
        {
            lrOn = false;
        }
    }


}
