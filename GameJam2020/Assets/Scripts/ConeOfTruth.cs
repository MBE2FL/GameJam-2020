﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Photon;

public class ConeOfTruth : PunBehaviour
{
    [SerializeField]
    float _radius = 10.0f;
    [SerializeField, Range(0.0f, 5.0f)]
    float _fadeRadiusMultiplier = 1.5f;
    [SerializeField, Range(0.0f, 0.25f)]
    float _fadeTheshold = 0.05f;
    [SerializeField]
    List<Collider> _collidersInRange = new List<Collider>();

    int _baseColourID;

    private void Awake()
    {
        _baseColourID = Shader.PropertyToID("_BaseColor");
    }

    // Start is called before the first frame update
    void Start()
    {
        // Non-local player's cone of truth.
        if (!photonView.isMine && PhotonNetwork.connected)
        {
            // Turn off the non-local player's light.
            Transform lightTrans = transform.Find("Light");

            if (!lightTrans)
            {
                Debug.LogError("Cone Of Truth: Failed to find non-local player's light child object!");
            }
            else
            {
                Light light = lightTrans.GetComponent<Light>();

                if (!light)
                {
                    Debug.LogError("Cone Of Truth: Failed to find non-local player's light!");
                }
                else
                {
                    light.gameObject.SetActive(false);
                }
            }
        }
        // Local player's cone of truth.
        else
        {
            // Use the default layer.
            gameObject.layer = 0;


            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

            if (!meshRenderer)
            {
                Debug.LogError("Cone Of Truth: Failed to find local player's mesh renderer!");
            }
            else
            {
                Color colour = meshRenderer.material.GetColor(_baseColourID);
                colour.a = 1.0f;
                meshRenderer.material.SetColor(_baseColourID, colour);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.isMine && PhotonNetwork.connected)
            return;

        // Adjust the visibility of all the previous frame's objects.
        for (int i = 0; i < _collidersInRange.Count; ++i)
        {
            Collider collider = _collidersInRange[i];

            MeshRenderer meshRenderer = collider.gameObject.GetComponent<MeshRenderer>();

            if (meshRenderer)
            {
                Vector3 playerToObj = collider.transform.position - transform.position;

                float fade = ((_radius * _fadeRadiusMultiplier) - playerToObj.magnitude) / (_radius);
                fade = Mathf.Clamp01(fade);

                //meshRenderer.material.SetFloat("Fade", fade);
                Color colour = meshRenderer.material.GetColor(_baseColourID);
                colour.a = fade;
                meshRenderer.material.SetColor(_baseColourID, colour);

                // Remove collider and set visibility to zero, when below fade theshold.
                if (fade < _fadeTheshold)
                {
                    //meshRenderer.material.SetFloat("Fade", 0.0f);
                    colour = meshRenderer.material.GetColor(_baseColourID);
                    colour.a = 0.0f;
                    meshRenderer.material.SetColor(_baseColourID, colour);

                    //_collidersInRange.RemoveAt(i);

                    Collider temp = _collidersInRange[_collidersInRange.Count - 1];
                    _collidersInRange[i] = temp;
                    _collidersInRange.RemoveAt(_collidersInRange.Count - 1);

                    --i;
                }

                Debug.DrawRay(transform.position, playerToObj.normalized * _radius, Color.green);
            }
        }

        // Find all the objects in range for this frame.
        int layerMask = 1 << 9;
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius * _fadeRadiusMultiplier, layerMask);

        //colliders = _collidersInRange.

        foreach (Collider collider in colliders)
        {
            // Check if the object is unobstructed from the player's view.
            Vector3 playerToObjDir = (collider.transform.position - transform.position).normalized;
            RaycastHit hit;

            if (Physics.Raycast(transform.position, playerToObjDir, out hit, _radius * _fadeRadiusMultiplier))
            {
                // Object is occluded from the player's line of sight.
                if (collider != hit.collider)
                {
                    // The object has prevously been seen.
                    if (_collidersInRange.Contains(collider))
                    {
                        // Set the object's visibilty to zero.
                        MeshRenderer meshRenderer = collider.gameObject.GetComponent<MeshRenderer>();

                        if (meshRenderer)
                        {
                            Color colour = meshRenderer.material.GetColor(_baseColourID);
                            colour.a = 0.0f;
                            meshRenderer.material.SetColor(_baseColourID, colour);
                        }
                    }

                    continue;
                }

                // The object has not previously been seen.
                if (!_collidersInRange.Contains(collider))
                {
                    _collidersInRange.Add(collider);
                }
            }
        }

        Debug.DrawRay(transform.position, transform.forward * _radius, Color.red);
    }
}