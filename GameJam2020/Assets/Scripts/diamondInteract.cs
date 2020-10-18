using System.Collections;
using System.Collections.Generic;
using Photon;
using UnityEngine;
public class diamondInteract : PunBehaviour
{

    bool triggerRange = false;
    bool holdingDiamond = false;

    GameObject diamond;
    PhotonView diamondView;
    Rigidbody diamondRB;

    // Start is called before the first frame update
    void Start()
    {
      //  diamondView = obj.GetComponent<PhotonView>();
        diamond = null;
        diamondRB = null;
    }

    public void DiamondView(GameObject obj)
    {
        diamondView = obj.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.isMine && PhotonNetwork.connected)
            return;

        if (Input.GetKeyDown(KeyCode.E) && triggerRange && !holdingDiamond)
        {
            diamond.transform.SetParent(this.transform); //sets the diamond to the transform of the player
            diamond.transform.localPosition = new Vector3(0, 1, 1.25f);

            diamondRB = diamond.GetComponent<Rigidbody>(); //sets the rigidbody and makes ti kinematic
            diamondRB.isKinematic = true;

            holdingDiamond = true;

            diamondView.TransferOwnership(PhotonNetwork.player);
        }
        else if (Input.GetKeyDown(KeyCode.E) && holdingDiamond || Input.GetKeyDown(KeyCode.R) && holdingDiamond)
        {
            diamond.transform.localPosition = new Vector3(0, 1, 2);
            diamond.transform.SetParent(null); //makes diamond an orphan
            diamond = null;

            diamondRB.isKinematic = false;
            if (Input.GetKeyDown(KeyCode.R)) //allows the player to throw the diamond
                diamondRB.AddForce(transform.forward * 500);
            diamondRB = null;

            holdingDiamond = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.isMine && PhotonNetwork.connected)
            return;
        if (other.tag == "Diamond")
        {
            triggerRange = true;
            diamond = other.transform.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!photonView.isMine && PhotonNetwork.connected)
            return;

        if (other.tag == "Diamond")
        {
            triggerRange = false;
        }
    }
}