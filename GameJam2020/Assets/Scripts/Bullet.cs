using Photon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : PunBehaviour
{
    [SerializeField]
    float _lifetime = 1.0f;
    float _currTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _currTime += Time.deltaTime;

        if (_currTime >= _lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            // Drop the diamond, and move back a bit.

            
            ContactPoint contactPoint = collision.GetContact(0);
            collision.rigidbody.AddForceAtPosition((-contactPoint.normal) * 8.0f, contactPoint.point, ForceMode.Impulse);


            diamondInteract diamondInt = collision.gameObject.transform.GetComponent<diamondInteract>();
            diamondInt.dropFromBullet();

            Destroy(gameObject);
        }
        else if (collision.transform.tag == "Diamond")
        {
            collision.rigidbody.velocity = Vector3.zero;
        }
    }
}
