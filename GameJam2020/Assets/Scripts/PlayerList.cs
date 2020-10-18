using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class PlayerList : MonoBehaviour
{
    public TMP_InputField p1, p2;
    public Button ready;

    private void Update()
    {
        setWaiting();
    }
    public void setWaiting()
    {
        p1.text = "P1 Ready!" + (PhotonNetwork.isMasterClient? "(ME)": "");
        if (PhotonNetwork.playerList.Length > 1)
        {
            p2.text = "P2 Ready!" + (!PhotonNetwork.isMasterClient? "(ME)": "");
            ready.interactable = true;
        }

    }
}