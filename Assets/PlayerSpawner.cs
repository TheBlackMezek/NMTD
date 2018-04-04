using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSpawner : NetworkManager {
    
    public GameObject fighterPrefab;


    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        if(NetworkServer.connections.Count > 1)
        {
            playerPrefab = fighterPrefab;
        }
        base.OnServerAddPlayer(conn, playerControllerId);
    }

}
