using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    Camera _cam;
    Transform _player;

    [SerializeField]
    float _pitch = 65.0f;
    float _pitchVel;
    [SerializeField]
    float _pitchLag = 10.0f;
    [SerializeField]
    float _lagSpeed = 10.0f;
    [SerializeField]
    float _yOffset = 20.0f;
    [SerializeField]
    float _zOffset = -4.0f;


    Vector3 _targetPos;
    Vector3 _currVel;



    private void Awake()
    {
        // Find the player's camera.
        _cam = Camera.main;


        // Find the player's transform.
        GameObject playerGO = GameObject.FindGameObjectWithTag("Player");

        if (!playerGO)
        {
            Debug.LogError("Camera Movement: Failed to find the player's game object!");
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        _player = playerGO.transform;
    }

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
        // Smoothly follow the player.
        Vector3 targetPos = _player.position;
        targetPos.y += _yOffset;
        targetPos.z += _zOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _currVel, _lagSpeed * Time.fixedDeltaTime);


        // Rotate just the camera's pitch.
        //transform.rotation = Quaternion.Euler(new Vector3(_pitch, transform.rotation.y, transform.rotation.z));
        Vector3 camRot = transform.rotation.eulerAngles;
        float currentCamPitch = Mathf.SmoothDampAngle(camRot.x, _pitch, ref _pitchVel, _pitchLag * Time.fixedDeltaTime);
        transform.rotation = Quaternion.Euler(new Vector3(currentCamPitch, camRot.y, camRot.z));
    }
}
