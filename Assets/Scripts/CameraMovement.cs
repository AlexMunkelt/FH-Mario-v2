using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class CameraMovement : MonoBehaviour
{

    private GameObject Player1;

    private GameObject Player2;

    private Vector3 Middle_Vec;

    private float MinAspect = 0;
    // Start is called before the first frame update
    void Start()
    {
        Player1 = GameObject.Find("Player");
        Player2 = GameObject.Find("Player2");
        Middle_Vec = (Player1.transform.position + Player2.transform.position) / 2;
        Camera.main.transform.position = Middle_Vec;
        Camera.main.
    }

    
    // Update is called once per frame
    void Update()
    {
        if (MinAspect == 0)
        {
            MinAspect = Camera.main.aspect;
            print("MinAspect: ");
            print(MinAspect);
        }
        Player1 = GameObject.Find("Player");
        Player2 = GameObject.Find("Player2");
        
        Middle_Vec = (Player1.transform.position + Player2.transform.position) / 2;
        Middle_Vec.y += 1.5f;
        Middle_Vec.z = -68;
        Camera.main.transform.position = Middle_Vec;
        /*float aspect = (
            Player1.transform.position.x > Player2.transform.position.x
                ? Player1.transform.position.x - Player2.transform.position.x
                : Player2.transform.position.x - Player1.transform.position.x) / (
            Player1.transform.position.y > Player2.transform.position.y
                ? Player1.transform.position.y - Player2.transform.position.y
                : Player2.transform.position.y - Player1.transform.position.y);
        if (aspect > MinAspect)
        {
            Camera.main.aspect = aspect;
        }*/
    }
}
