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
        Vector3 force = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W))
            force += Vector3.forward;

        if (Input.GetKey(KeyCode.A))
            force += Vector3.left;

        if (Input.GetKey(KeyCode.S))
            force += Vector3.back;

        if (Input.GetKey(KeyCode.D))
            force += Vector3.right;

        _rb.AddForce(force.normalized * speedMultiplier);
    }
}
