using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;
public class PlayerFootsteps : PunBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    List<AudioClip> footsteps;
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    GameObject player;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.isMine && PhotonNetwork.connected)
            return;

        //checks if the player is moving around
        if (player.GetComponent<Rigidbody>().velocity.x > 0.5f || player.GetComponent<Rigidbody>().velocity.x < -0.5f ||
            player.GetComponent<Rigidbody>().velocity.z > 0.5f || player.GetComponent<Rigidbody>().velocity.z < -0.5f)
        {
            //play footsteps
            if (!audioSource.isPlaying)
            {
                int rand = Random.Range(0, footsteps.Count);

                audioSource.clip = footsteps[rand];
                audioSource.PlayDelayed(0.2f);
            }
        }
    }
}