using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Examples.Pong;
using DG.Tweening.Core.Easing;

public class CustomNetworkManagerPong : NetworkManagerPong
{
    public PongLogic gameManagerPrefab;

    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        GameObject player = Instantiate(playerPrefab);
        NetworkServer.AddPlayerForConnection(conn, player);

        if (numPlayers == 1)
        {
            GameObject gameManager = Instantiate(gameManagerPrefab.gameObject);
            NetworkServer.Spawn(gameManager);
        }
    }
}