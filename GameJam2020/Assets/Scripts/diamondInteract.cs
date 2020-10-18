using System.Collections;
using System.Collections.Generic;
using Photon;
using UnityEngine;
public class diamondInteract : PunBehaviour
{

    bool triggerRange = false;
    bool holdingDiamond = false;

    public GameObject diamond;
    PhotonView diamondView;
    Rigidbody diamondRB;

    public event System.Action _onDiamondGrabbed;
    public event System.Action _onDiamondDropped;

    EscapeManager _escapeManager;

    // Start is called before the first frame update
    void Start()
    {
        diamondView = GameObject.FindGameObjectWithTag("Diamond").GetComponent<PhotonView>();
        diamond = GameObject.FindGameObjectWithTag("Diamond").transform.gameObject;

        diamondRB = diamond.GetComponent<Rigidbody>(); //sets the rigidbody and makes ti kinematic


        // Find the escape manager.
        GameObject gameManagerGO = GameObject.FindGameObjectWithTag("Game Manager");

        if (!gameManagerGO)
        {
            Debug.LogError("Diamond Interact: Failed to find the game manager object!");
        }
        else
        {
            _escapeManager = gameManagerGO.GetComponent<EscapeManager>();

            if (!_escapeManager)
            {
                Debug.LogError("Diamond Interact: Failed to find the escape manager!");
            }
            else
            {
                _escapeManager.addDiamondInteractable(this);

                _escapeManager._onDiamondReset += onDiamondReset;
            }
        }
    }

    private void OnDestroy()
    {
        if (_escapeManager)
        {
            _escapeManager.removeDiamondInteractable(this);

            _escapeManager._onDiamondReset -= onDiamondReset;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.isMine && PhotonNetwork.connected)
            return;

        if (Input.GetKeyDown(KeyCode.E) && triggerRange && !holdingDiamond)
        {
            photonView.RPC("GetDiamond", PhotonTargets.All, null);
        }
        else if (Input.GetKeyDown(KeyCode.E) && holdingDiamond || Input.GetKeyDown(KeyCode.R) && holdingDiamond)
        {
            photonView.RPC("DropDiamond", PhotonTargets.All, null);
        }

    }

    [PunRPC]
    void GetDiamond()
    {
        diamond.transform.SetParent(this.transform); //sets the diamond to the transform of the player
        diamond.transform.localPosition = new Vector3(0, 1, 1.25f);

        diamondRB.isKinematic = true;

        holdingDiamond = true;

        diamondView.TransferOwnership(PhotonNetwork.player);


        // Notify listeners of diamond pickup.
        _onDiamondGrabbed();
    }

    [PunRPC]
    void DropDiamond()
    {
        diamond.transform.localPosition = new Vector3(0, 1, 2);
        diamond.transform.SetParent(null); //makes diamond an orphan

        diamondRB.isKinematic = false;
        if (Input.GetKeyDown(KeyCode.R)) //allows the player to throw the diamond
            diamondRB.AddForce(transform.forward * 500);
       
        holdingDiamond = false;


        // Notify listeners of diamond drop.
        _onDiamondDropped();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!photonView.isMine && PhotonNetwork.connected)
            return;
        if (other.tag == "Diamond")
        {
            triggerRange = true;
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

    void onDiamondReset()
    {
        diamondRB.isKinematic = false;
        holdingDiamond = false;
        triggerRange = false;
    }
}