using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    [SerializeField]
    Rigidbody _rb;

    public float speedMultiplier = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
            _rb.AddForce(Vector3.forward * speedMultiplier);

        if (Input.GetKey(KeyCode.A))
            _rb.AddForce(Vector3.left * speedMultiplier);

        if (Input.GetKey(KeyCode.S))
            _rb.AddForce(Vector3.back * speedMultiplier);

        if (Input.GetKey(KeyCode.D))
            _rb.AddForce(Vector3.right * speedMultiplier);
    }
}
