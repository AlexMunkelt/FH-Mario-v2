using System;
using System.Collections;
using UnityEngine;

public class OpenDoor : MonoBehaviour
    {
        public bool btn1Pressed = false;
        public bool btn2Pressed = false;
        private GameObject door2;
        private bool doorOpen = false;

        private void _OpenDoor()
        {
            if (!doorOpen)
            {
                door2.transform.Rotate(0, 90, 0);
                transform.Rotate(0, 90, 0);
                doorOpen = true;
            }
        }

        private void Start()
        {
            door2 = GameObject.Find("Door2");
        }

        private void Update()
        {
            if (btn1Pressed && btn2Pressed && !doorOpen)
            {
                if (!doorOpen)
                {
                    _OpenDoor();
                }
            }
        }
    }