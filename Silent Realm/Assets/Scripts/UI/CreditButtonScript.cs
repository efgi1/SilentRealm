using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditButtonScript : MonoBehaviour
{
    public GameObject creditText;
    private bool isShown = false;
    // Start is called before the first frame update
    void Start()
    {
        creditText.SetActive(isShown);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowCredits()
    {
        if (!isShown)
        {
            isShown = true;
            creditText.SetActive(isShown);
        } else
        {
            isShown = false;
            creditText.SetActive(isShown);
        }
    }
}
