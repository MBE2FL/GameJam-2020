using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    [SerializeField]
    Rigidbody _rb;

    public float speedMultiplier = 10.0f;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        LookAtMouse();

    }

    private void FixedUpdate()
    {
        //basic movement system for the player using the rigidbody

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

    private void LookAtMouse()
    {
        //this function shoots a ray from where the mouse is in pixel space down to 3d space
        //if it detects a hit on the ground the plyaers y rotation will face toward the position of the hit

        RaycastHit hit;
        Ray mouseCoord = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(mouseCoord, out hit, 100.0f, ~10))
        {
            transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
        }
    }
}
