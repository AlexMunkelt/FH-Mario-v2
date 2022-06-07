using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopAufgabe : MonoBehaviour
{
    private GameObject[] btns;

    private GameObject[] doors;

    private GameObject[] players;

    private float btn1Y;
    
    private float btn2Y;
    
    private bool btnsPushed = false;
    
    // Start is called before the first frame update
    void Start()
    {
        btns = new[] {GameObject.Find("Btn1"), GameObject.Find("Btn2")};
        doors = new[] {GameObject.Find("Door"), GameObject.Find("Door2")};
        players = new[] {GameObject.Find("Player"), GameObject.Find("Player2")};
        btn1Y = btns[0].transform.position.y;
        btn2Y = btns[1].transform.position.y;
        
        //Add Colliders
        foreach (GameObject door in doors)
        {
            door.AddComponent<BoxCollider>();
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
        var playerPos = player.transform.position;
        var btnPos = btn.transform.position;
        return playerPos.y.Equals(btnPos.y) && playerPos.x.Equals(btnPos.x);
    }
    
    //Push down button
    void PushDown(GameObject btn)
    {
        var btnY = btn.transform.position.y;
        print("BtnY: " + btnY);
        while (btnY >= -0.05f)
        {
            btn.transform.position = new Vector3(btn.transform.position.x, btnY, btn.transform.position.z);
            btnY -= 0.01f;
        }
    }
    
    //Push up button
    void PushUp(GameObject btn)
    {
        var btnY = btn.transform.position.y;
        while (btnY <= 0.05f)
        {
            btn.transform.position = new Vector3(btn.transform.position.x, btnY, btn.transform.position.z);
            btnY += 0.01f;
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
        if(PlayerOnBtn(btns[0], players[0]) || PlayerOnBtn(btns[0], players[1]))
        {
            PushDown(btns[0]);
        }
        else
        {
            PushUp(btns[0]);
        }
        
        if(PlayerOnBtn(btns[1], players[0]) || PlayerOnBtn(btns[1], players[1]))
        {
            PushDown(btns[1]);
        }
        else
        {
            PushUp(btns[1]);
        }
        
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
