using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class EscapeManager : MonoBehaviour
{
    [SerializeField]
    List<Transform> _escapePoints;

    bool _diamondGrabbed = false;

    [SerializeField]
    float _escapeTime = 30.0f;
    [SerializeField]
    float _currTime = 0.0f;
    [SerializeField]
    Transform _currEscapePoint;

    private void Awake()
    {
        _escapePoints = new List<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_diamondGrabbed)
        {
            _currTime += Time.deltaTime;

            // Ran out of time to escape.
            if (_currTime >= _escapeTime)
            {
                Debug.Log("Escape point compromised!");

                // Pick new random escape point.
                pickEscapePoint(_currEscapePoint);

                // Reset escape timer.
                _currTime = 0.0f;
            }
        }
    }

    void onDiamondGrabbed()
    {
        _diamondGrabbed = true;

        // Display effect.

        // Display random escape point.
        pickEscapePoint(null);
    }

    void onDiamondDropped()
    {
        _diamondGrabbed = false;

        // Stop display effect.

        // Hide designated escape point.
        _currEscapePoint = null;

        // Reset escape timer.
        _currTime = 0.0f;
    }

    void pickEscapePoint(Transform prevPoint)
    {
        // Player has a previous escape point.
        if (prevPoint)
        {
            List<Transform> temp = new List<Transform>(_escapePoints);
            temp.Remove(prevPoint);

            int randomIndex = Random.Range(0, temp.Count);

            _currEscapePoint = temp[randomIndex];
        }
        // Player does not have a previous escape point.
        else
        {
            int randomIndex = Random.Range(0, _escapePoints.Count);

            _currEscapePoint = _escapePoints[randomIndex];
        }
    }
}
