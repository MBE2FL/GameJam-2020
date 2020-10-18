using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class diamondInteract : MonoBehaviour
{

    bool triggerRange = false;
    bool holdingDiamond = false;

    GameObject diamond;
    Rigidbody diamondRB;

    // Start is called before the first frame update
    void Start()
    {
        diamond = null;
        diamondRB = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && triggerRange && !holdingDiamond)
        {
            diamond.transform.SetParent(this.transform);//sets the diamond to the transform of the player
            diamond.transform.localPosition = new Vector3(0, 1, 1.25f);

            diamondRB = diamond.GetComponent<Rigidbody>();//sets the rigidbody and makes ti kinematic
            diamondRB.isKinematic = true;

            holdingDiamond = true;
        }
        else if (Input.GetKeyDown(KeyCode.E) && holdingDiamond || Input.GetKeyDown(KeyCode.R) && holdingDiamond)
        {
            diamond.transform.localPosition = new Vector3(0, 1, 2);
            diamond.transform.SetParent(null);//makes diamond an orphan
            diamond = null;

            diamondRB.isKinematic = false;
            if (Input.GetKeyDown(KeyCode.R))//allows the player to throw the diamond
                diamondRB.AddForce(transform.forward * 500);
            diamondRB = null;

            holdingDiamond = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Diamond")
        {
            triggerRange = true;
            diamond = other.transform.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Diamond")
        {
            triggerRange = false;
        }
    }
}
