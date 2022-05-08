using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsScript : MonoBehaviour
{
    public GameObject controlsUI;

    void Start()
    {
        controlsUI.SetActive(false);
    }

    public void ShowControls()
    {
        controlsUI.SetActive(!controlsUI.activeSelf);
    }
}
