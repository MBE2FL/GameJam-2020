using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConeOfTruth : MonoBehaviour
{
    [SerializeField]
    float _radius = 10.0f;
    List<Collider> _collidersInRange = new List<Collider>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Disable visibility of all previous frame's objects.
        foreach (Collider collider in _collidersInRange)
        {
            MeshRenderer meshRenderer = collider.gameObject.GetComponent<MeshRenderer>();

            if (meshRenderer)
            {
                meshRenderer.enabled = false;
            }
        }

        _collidersInRange.Clear();


        // Find all the objects in range for this frame, and enable their visibility.
        int layerMask = 1 << 9;
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius, layerMask);

        foreach (Collider collider in colliders)
        {
            // Check if the object is unobstructed from the player's view.
            Vector3 playerToObj = collider.transform.position - transform.position;
            Vector3 playerToObjDir = playerToObj.normalized;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, playerToObjDir, out hit, _radius))
            {
                if (collider != hit.collider)
                {
                    continue;
                }

                MeshRenderer meshRenderer = collider.gameObject.GetComponent<MeshRenderer>();

                if (meshRenderer)
                {
                    meshRenderer.enabled = true;
                }

                _collidersInRange.Add(collider);

                Debug.DrawRay(transform.position, playerToObjDir * _radius, Color.green);
            }
        }

        //_collidersInRange = new List<Collider>(colliders);



        Debug.DrawRay(transform.position, transform.forward * _radius, Color.red);
    }
}
