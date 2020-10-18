using System.Collections;
using System.Collections.Generic;
using Photon;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLobby : PunBehaviour
{
    public GameObject p1, p2;
    public TMP_Text lobbyName;

    public void createRoom()
    {
        RoomOptions ops = new RoomOptions();
        ops.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom(lobbyName.text, ops, PhotonNetwork.lobby);
        PhotonNetwork.LoadLevel("Waiting");
    }

    public void toGame()
    {
        if (PhotonNetwork.isMasterClient)
            PhotonNetwork.LoadLevel("samplescene");
    }

    public void onGameStart()
    {
        GameObject player;

        if (PhotonNetwork.isMasterClient)
            player = PhotonNetwork.Instantiate(p1.name, p1.transform.position, p1.transform.rotation, 0);
        else
            player = PhotonNetwork.Instantiate(p2.name, p2.transform.position, p2.transform.rotation, 0);

        Camera.main.GetComponent<CameraMovement>().inst(player);
    }

    public override void OnConnectedToPhoton()
    {
        print("connected to photon");
    }

    public override void OnJoinedLobby()
    {
        print("Joined Lobby");
    }

    public override void OnJoinedRoom()
    {
        print("jointed room ;>");
        //  GameObject.FindObjectOfType<PlayerList>().setWaiting();
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
            //PhotonNetwork.JoinRandomRoom();
            print("Photon connected already");
        }
        else
        {
            // #Critical, we must first and foremost connect to Photon Online Server.
            PhotonNetwork.ConnectUsingSettings("1");
            PhotonNetwork.gameVersion = "1";

        }
    }
}