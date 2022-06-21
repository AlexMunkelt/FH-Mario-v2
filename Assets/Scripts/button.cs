using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour
{
    private bool isPressed = false;
    private void OnTriggerEnter(Collider other)
    {
        if (name == "Btn1")
        {
            GameObject.Find("KoopAufgabe").GetComponent<KoopAufgabe>().btn1isPressed = true;
        }
        else
        {
            GameObject.Find("KoopAufgabe").GetComponent<KoopAufgabe>().btn2isPressed = true;
        }
        if(!isPressed)
            press();
        isPressed = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (name == "Btn1")
        {
            if (GameObject.Find("KoopAufgabe").GetComponent<KoopAufgabe>().btn2isPressed == false)
            {
                GameObject.Find("KoopAufgabe").GetComponent<KoopAufgabe>().btn1isPressed = false;
                pull();
                isPressed = false;
            }
        }
        else
        {
            if (GameObject.Find("KoopAufgabe").GetComponent<KoopAufgabe>().btn1isPressed == false)
            {
                GameObject.Find("KoopAufgabe").GetComponent<KoopAufgabe>().btn2isPressed = false;
                pull();
                isPressed = false;
            }
        }
    }

    //animation for button press
    private void press()
    {
        transform.position -= new Vector3(0, 0.5f, 0);
    }

    private void pull()
    {
        transform.position += new Vector3(0, 0.5f, 0);
    }
}
