using System.Collections;
using System.Collections.Generic;
using Photon;
using UnityEngine;

public class PlayerLobby : UnityEngine.MonoBehaviour
{
   
    private void Awake()
    {
        PhotonNetwork.automaticallySyncScene = true;
    }

    private void Start()
    {
        connection();
    }
    void connection()
    {
        // we check if we are connected or not, we join if we are , else we initiate the connection to the server.
        if (PhotonNetwork.connected)
        {
            // #Critical we need at this point to attempt joining a Random Room. If it fails, we'll get notified in OnJoinRandomFailed() and we'll create one.
            PhotonNetwork.JoinRandomRoom();
            print("Photon connected");
        }
        else
        {
            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.ConnectUsingSettings("1");
            PhotonNetwork.gameVersion = "1";

        }
    }
}