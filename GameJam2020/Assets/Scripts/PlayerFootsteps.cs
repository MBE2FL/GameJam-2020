using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
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
        if(player.GetComponent<Rigidbody>().velocity.x > 0.5f || player.GetComponent<Rigidbody>().velocity.x < -0.5f ||
           player.GetComponent<Rigidbody>().velocity.z > 0.5f || player.GetComponent<Rigidbody>().velocity.z < -0.5f)
        {
            //play footsteps
            if(!audioSource.isPlaying)
            {
                int rand = Random.Range(0, 3);

                audioSource.clip = footsteps[rand];
                audioSource.PlayDelayed(0.2f);
                //Debug.Log("it be playing");
                //choose next sound and play. maybe add timer for cooldown
            }
        }
    }
}
