using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class Diamond : PunBehaviour
{
    [SerializeField]
    List<Transform> _spawnPoints = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void resetDiamond()
    {
        // Select a random spawn point.
        if (_spawnPoints.Count <= 0)
        {
            Debug.LogError("Diamond: There were no spawn points assigned!");
        }

        int randomIndex = Random.Range(0, _spawnPoints.Count);
        Transform transform = _spawnPoints[randomIndex];

        // Notify the network.
        photonView.RPC("resetTransform", PhotonTargets.All, transform.position);
    }

    [PunRPC]
    void resetTransform(Vector3 position)
    {
        transform.SetParent(null);

        transform.position = position;
    }
}
