using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class button : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (name == "Btn1")
        {
            GameObject.Find("KoopAufgabe").GetComponent<KoopAufgabe>().btn1isPressed = true;
            press();
        }
        else
        {
            GameObject.Find("KoopAufgabe").GetComponent<KoopAufgabe>().btn2isPressed = true;
            press();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (name == "Btn1")
        {
            if (GameObject.Find("KoopAufgabe").GetComponent<KoopAufgabe>().btn2isPressed == false)
            {
                GameObject.Find("KoopAufgabe").GetComponent<KoopAufgabe>().btn1isPressed = false;
                pull();
            }
        }
        else
        {
            if (GameObject.Find("KoopAufgabe").GetComponent<KoopAufgabe>().btn1isPressed == false)
            {
                GameObject.Find("KoopAufgabe").GetComponent<KoopAufgabe>().btn2isPressed = false;
                pull();
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
