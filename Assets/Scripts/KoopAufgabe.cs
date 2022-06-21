using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopAufgabe : MonoBehaviour
{
    public GameObject door1;
    public GameObject door2;
    
    private bool isOpen = false;
    public bool btn1isPressed = false;
    public bool btn2isPressed = false;
    // Start is called before the first frame update
    void Start()
    {
        door1 = GameObject.Find("Door");
        door2 = GameObject.Find("Door2");
    }

    // Update is called once per frame
    void Update()
    {
        if(btn1isPressed && btn2isPressed)
        {
            isOpen = true;
        }
        else
        {
            isOpen = false;
        }

        if (isOpen)
        {
            door1.transform.position -= Vector3.down;
            door2.transform.position -= Vector3.down;
        }
    }
}
