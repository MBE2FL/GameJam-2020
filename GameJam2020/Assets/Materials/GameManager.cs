using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    List<Material> _fadeMaterials;

    int _baseColourID;

    EscapeManager _escapeManager;


    public EscapeManager EscapeManager
    {
        get
        {
            return _escapeManager;
        }
    }


    private void Awake()
    {
        _baseColourID = Shader.PropertyToID("_BaseColor");

        // Set all fade materials to not be visible on game start.
        foreach (Material material in _fadeMaterials)
        {
            //material.SetFloat("Fade", 0.0f);

            Color colour = material.GetColor(_baseColourID);
            colour.a = 0.0f;
            material.SetColor(_baseColourID, colour);
        }


        // Find the escape manager.
        _escapeManager = GetComponent<EscapeManager>();

        if (!_escapeManager)
        {
            Debug.LogError("Game Manager: Failed to find the escape manager!");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

#if UNITY_EDITOR
    private void OnApplicationQuit()
    {
        // Set all fade materials to be visible on application quit.
        foreach (Material material in _fadeMaterials)
        {
            //material.SetFloat("Fade", 0.0f);

            Color colour = material.GetColor(_baseColourID);
            colour.a = 1.0f;
            material.SetColor(_baseColourID, colour);
        }
    }
#endif

    // Update is called once per frame
    void Update()
    {
        
    }
}
