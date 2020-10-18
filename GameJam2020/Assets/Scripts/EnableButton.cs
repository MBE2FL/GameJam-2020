using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class EnableButton : MonoBehaviour
{
    public void enableButton(bool enable)
    =>
        GetComponent<Button>().interactable = enable;
    
}