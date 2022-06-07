using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopAufgabe : MonoBehaviour
{
    private GameObject[] btns;

    private GameObject[] doors;

    private GameObject[] players;
    
    private bool btnsPushed = false;
    
    // Start is called before the first frame update
    void Start()
    {
        btns = new[] {GameObject.Find("Btn1"), GameObject.Find("Btn2")};
        doors = new[] {GameObject.Find("Door"), GameObject.Find("Door2")};
        players = new[] {GameObject.Find("Player"), GameObject.Find("Player2")};
        
        //Add Rigidbody
        foreach (GameObject[] a in new []{btns, doors})
        {
            foreach (GameObject o in a)
            {
                o.AddComponent<Rigidbody>();
            }
        }
        
        //freeze btn position and rotation
        foreach (GameObject[] a in new []{btns, doors})
        {
            foreach (GameObject o in a)
            {
                o.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
            }
        }

    }
    
    bool PlayerOnBtn(GameObject btn, GameObject player)
    {
        //Push down btn if player is on it
        if(player.transform.position == btn.transform.position)
        {
            PushDown(btn);
            return true;
        }
        return false;
    }
    
    //Push down button
    void PushDown(GameObject btn)
    {
        
        //Get Color of btn
        Color c = btn.GetComponent<Renderer>().material.color;
        //Add Glow Effect to btn
        btn.GetComponent<Renderer>().material.SetColor("_EmissionColor", c);
        
        var btnY = btn.transform.position.y;
        while (btnY >= -0.05f)
        {
            btn.transform.position = new Vector3(btn.transform.position.x, btnY, btn.transform.position.z);
            btnY -= 0.01f;
        }
    }

    void OpenDoor(GameObject door)
    {
        //Rotate door to open
        var doorRot = door.transform.rotation.eulerAngles.y;
        while (doorRot <= 90)
        {
            door.transform.rotation = Quaternion.Euler(0, doorRot, 0);
            doorRot += 1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerOnBtn(btns[0], players[0]) && PlayerOnBtn(btns[1], players[1]))
        {
            btnsPushed = true;
        }else if(PlayerOnBtn(btns[0], players[1]) && PlayerOnBtn(btns[1], players[0]))
        {
            btnsPushed = true;
        }
        
        if(btnsPushed)
        {
            foreach (var door in doors)
            {
                OpenDoor(door);
            }
        }
        
    }
}
