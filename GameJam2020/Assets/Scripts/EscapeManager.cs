using Photon;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class EscapeManager : PunBehaviour
{
    [SerializeField]
    List<Transform> _escapePoints;

    [SerializeField]
    List<Transform> _diamondSpawnPoints;

    bool _diamondGrabbed = false;

    [SerializeField]
    float _escapeTime = 30.0f;
    [SerializeField]
    float _currTime = 0.0f;
    [SerializeField]
    Transform _currEscapePoint;

    Vignette _vignette;
    [SerializeField]
    float _vignetteSpeed = 2.0f;
    [SerializeField, Range(0.0f, 1.0f)]
    float _vignetteCoverage = 0.6f;

    GameObject _diamond;

    public event System.Action _onDiamondReset;



    private void Awake()
    {
        // Find the post process volume, and it's vignette.
        Volume postProcessVolume = GameObject.Find("Post Process Volume").GetComponent<Volume>();

        if (!postProcessVolume)
        {
            Debug.LogError("Escape Manager: Failed to find the post process volume!");
        }
        else
        {
            postProcessVolume.profile.TryGet(out _vignette);

            if (!_vignette)
            {
                Debug.LogError("Escape Manager: Failed to find the vignette!");
            }
        }


        // Find the diamond.
        _diamond = GameObject.Find("Diamond");

        if (!_diamond)
        {
            Debug.LogError("Escape Manager: Failed to find the diamond!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Run on the master client only.
        if (PhotonNetwork.isMasterClient)
        {
            if (_diamondGrabbed)
            {
                _currTime += Time.deltaTime;

                // Ran out of time to escape.
                if (_currTime >= _escapeTime)
                {
                    Debug.Log("Escape point compromised!");

                    // Pick new random escape point.
                    //pickEscapePoint(_currEscapePoint);

                    // Reset escape timer.
                    _currTime = 0.0f;

                    // Reset visual effect.
                    _vignette.intensity.value = 0.0f;

                    // Reset the diamond.
                    //_diamond.resetDiamond();
                    pickDiamondSpawnPoint();

                    _diamondGrabbed = false;
                    photonView.RPC("updateDiamondGrabbed", PhotonTargets.Others, _diamondGrabbed);

                    return;
                }


                _vignette.intensity.value = Mathf.Abs(_vignetteCoverage * Mathf.Sin(Time.time * _vignetteSpeed));
            }
        }
        else
        {
            if (_diamondGrabbed)
            {
                _vignette.intensity.value = Mathf.Abs(_vignetteCoverage * Mathf.Sin(Time.time * _vignetteSpeed));
            }
        }
    }

    public void addDiamondInteractable(diamondInteract diamondInt)
    {
        // Run on the master client only.
        if (PhotonNetwork.isMasterClient)
        {
            diamondInt._onDiamondGrabbed += onDiamondGrabbed;
            diamondInt._onDiamondDropped += onDiamondDropped;
        }
    }

    public void removeDiamondInteractable(diamondInteract diamondInt)
    {
        // Run on the master client only.
        if (PhotonNetwork.isMasterClient)
        {
            diamondInt._onDiamondGrabbed -= onDiamondGrabbed;
            diamondInt._onDiamondDropped -= onDiamondDropped;
        }
    }

    [PunRPC]
    void updateDiamondGrabbed(bool diamondGrabbed)
    {
        _diamondGrabbed = diamondGrabbed;

        if (!_diamondGrabbed)
        {
            _vignette.intensity.value = 0.0f;
        }
    }

    void onDiamondGrabbed()
    {
        _diamondGrabbed = true;
        photonView.RPC("updateDiamondGrabbed", PhotonTargets.Others, _diamondGrabbed);

        // Display effect.

        // Display random escape point.
        pickEscapePoint(null);
    }

    void onDiamondDropped()
    {
        _diamondGrabbed = false;
        photonView.RPC("updateDiamondGrabbed", PhotonTargets.Others, _diamondGrabbed);

        // Stop display effect.
        _vignette.intensity.value = 0.0f;

        // Hide designated escape point.
        _currEscapePoint = null;
    }

    void pickEscapePoint(Transform prevPoint)
    {
        if (_escapePoints.Count <= 0)
        {
            Debug.LogError("Escape Manager: There were no escape points assigned!");
            return;
        }

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

    void pickDiamondSpawnPoint()
    {
        // Select a random spawn point.
        if (_diamondSpawnPoints.Count <= 0)
        {
            Debug.LogError("Diamond: There were no spawn points assigned!");
        }

        int randomIndex = Random.Range(0, _diamondSpawnPoints.Count);
        Transform transform = _diamondSpawnPoints[randomIndex];

        // Notify the network.
        photonView.RPC("resetDiamond", PhotonTargets.All, transform.position);
    }

    [PunRPC]
    void resetDiamond(Vector3 position)
    {
        _diamond.transform.SetParent(null);

        _diamond.transform.position = position;

        // Notify all listeners.
        _onDiamondReset();
    }
}
