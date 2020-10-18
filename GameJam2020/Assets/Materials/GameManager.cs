using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    List<Material> _fadeMaterials;


    private void Awake()
    {
        // Set all fade materials to not be visible on game start.
        foreach (Material material in _fadeMaterials)
        {
            material.SetFloat("Fade", 0.0f);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
