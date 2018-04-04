using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class HeartController : NetworkBehaviour {

    public int maxLives = 10;

    public Renderer renderer;

    [SyncVar]
    private int lives;



    private void Start()
    {
        renderer.material.color = new Color(1, 0, 0);

        if (isServer)
        {
            lives = maxLives;
        }
    }

    [Command]
    public void CmdDamage(int amt)
    {
        lives -= amt;

        if(lives <= 0)
        {
            RpcDisconnect();
            Invoke("CmdStopHost", 0.5f);
        }
        else
        {
            renderer.material.color = new Color(lives / (float)maxLives, 0, 0);
            RpcUpdateColor();
        }
    }

    [Command]
    public void CmdStopHost()
    {
        NetworkManager.singleton.StopHost();
    }

    [ClientRpc]
    public void RpcUpdateColor()
    {
        renderer.material.color = new Color(lives / (float)maxLives, 0, 0);
    }

    [ClientRpc]
    public void RpcDisconnect()
    {
        if(!isServer)
        {
            Cursor.lockState = CursorLockMode.None;
            NetworkManager.singleton.StopClient();
        }
    }

}
