﻿using System.Collections;
using System.Collections.Generic;
using Photon;
using UnityEngine;

public class PlayerLobby : PunBehaviour
{
    public GameObject p1, p2, diamond;
    public override void OnConnectedToPhoton()
    {
        print("connected to photon");
    }

    public override void OnJoinedLobby()
    {
        print("Joined Lobby");

        RoomOptions ops = new RoomOptions();
        ops.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom("new room", ops, PhotonNetwork.lobby);
    }

    public override void OnJoinedRoom()
    {
        print("jointed room ;>");
        GameObject player, thediamond;

        if (PhotonNetwork.isMasterClient)
        {
            player = PhotonNetwork.Instantiate(p1.name, p1.transform.position, Quaternion.identity, 0);

            //instantiate diamond 
            thediamond = PhotonNetwork.Instantiate(diamond.name, diamond.transform.position, diamond.transform.rotation, 0);

            player.GetComponent<diamondInteract>().DiamondView(thediamond);
        }
        else
        {
            player = PhotonNetwork.Instantiate(p2.name, p2.transform.position, Quaternion.identity, 0);
        }
        Camera.main.GetComponent<CameraMovement>().inst(player);
    }
    public override void OnConnectedToMaster()
    {
        print("joined Master");
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        print("new player: " + newPlayer);
    }
    private void Awake()
    {
        PhotonNetwork.automaticallySyncScene = true;
    }

    private void Start()
    {
        connect();
    }

    void connect()
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