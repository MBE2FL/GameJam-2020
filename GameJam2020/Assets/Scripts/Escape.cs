using Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escape : PunBehaviour
{
    [SerializeField]
    Light _light;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void turnOn()
    {
        _light.enabled = true;
    }

    public void turnOff()
    {
        _light.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (other.gameObject.GetComponent<diamondInteract>().HoldingDiamond)
            {
                Debug.Log("Game Over!");
            }
        }
    }
}
