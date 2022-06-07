using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoopAufgabe : MonoBehaviour
{
    private GameObject[] btns;

    private GameObject[] doors;

    private GameObject[] players;
    
    // Start is called before the first frame update
    void Start()
    {
        btns = new[] {GameObject.Find("Btn1"), GameObject.Find("Btn2")};
        doors = new[] {GameObject.Find("Door"), GameObject.Find("Door2")};
        players = new[] {GameObject.Find("Player"), GameObject.Find("Player2")};
    }
    
    // Add Collider to Buttons
    void 

    // Update is called once per frame
    void Update()
    {
        
    }
}
