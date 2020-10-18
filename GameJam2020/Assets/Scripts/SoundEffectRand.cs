using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectRand : MonoBehaviour //This script will be for playing spooky sound effects
{
    [SerializeField]
    AudioSource AudioSource;
    [SerializeField]
    List<AudioClip> spookyEffects;
    [SerializeField]
    List<EffectTrigger> triggers; //This will be used later maybe when the map comes in
    [SerializeField]
    float spookyTimerStart;

    float spookyTimer;
    bool isTriggered = false;

    // Start is called before the first frame update
    void Start()
    {
        spookyTimer = spookyTimerStart;
    }

    // Update is called once per frame
    void Update()
    {
        spookyTimer -= Time.deltaTime;

        for(int i = 0; i < triggers.Count; i++)//checks the list of triggers if any have been triggered
        {
            if(triggers[i].triggered)//if yes then can have a chance to play a spooky sound
            {
                isTriggered = true;
                break;
            }
        }

        int spookyChance = Random.Range(0,10); // random chance of the effect playing

        if(spookyTimer <= 0.0f && spookyChance > 4 && isTriggered)
        {
            int spookyNum = Random.Range(0, spookyEffects.Count);
            AudioSource.clip = spookyEffects[spookyNum];//chooses and plays the audio clip
            AudioSource.Play();

            spookyTimer = spookyTimerStart;
            isTriggered = false;
        }

        isTriggered = false;
    }
}
