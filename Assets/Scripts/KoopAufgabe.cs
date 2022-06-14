using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class KoopAufgabe : MonoBehaviour
{
    
    private bool btnPressed = false;

    private bool isCoop;

    private void Start()
    {
        isCoop = ToggleCoop.instance.Equals(null);
        if (!isCoop)
        {
            //disable the button
            GameObject.Find("CoopAufgabe").SetActive(false);
        }
    }

    private void PushButton()
    {
        transform.position -= new Vector3(0, 0.1f, 0);
    }
    
    private void PullButton()
    {
        transform.position += new Vector3(0, 0.1f, 0);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!btnPressed)
        {
            PushButton();
            btnPressed = true;
        }
        
        if (name == "Btn1")
        {
            GameObject.Find("Door").GetComponent<OpenDoor>().btn1Pressed = true;
        }
        else
        {
            GameObject.Find("Door").GetComponent<OpenDoor>().btn2Pressed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (name == "Btn1")
        {
            var btn2Pressed = GameObject.Find("Door").GetComponent<OpenDoor>().btn2Pressed;
            if (!btn2Pressed)
            {
                PullButton();
                GameObject.Find("Door").GetComponent<OpenDoor>().btn1Pressed = false;
                btnPressed = false;
            }
        }
        else
        {
            var btn1Pressed = GameObject.Find("Door").GetComponent<OpenDoor>().btn1Pressed;
            if(!btn1Pressed)
            {
                PullButton();
                GameObject.Find("Door").GetComponent<OpenDoor>().btn2Pressed = false;
                btnPressed = false;
            }
        }
    }
}
