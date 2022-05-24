using System;
using System.Collections;
using static System.Math;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private int MaxFOV = 40;
    [Range(-1f, 0f), SerializeField] private float Dropdown_speed = -0.25f;
    private GameObject Player1;

    private GameObject Player2;

    private Vector3 Middle_Vec;
    
    private Vector3 Camera_Pos;

    private float expand_collider_box = 2.5f;
    

    private float MinFOV;
    // Start is called before the first frame update
    void Start()
    {
        AddCollider();
        Player1 = GameObject.Find("Player");
        Player2 = GameObject.Find("Player2");
        Middle_Vec = (Player1.transform.position + Player2.transform.position) / 2;
        Camera.main.transform.position = Middle_Vec;
        MinFOV = Camera.main.fieldOfView;
    }

    // add collider to the camera
    private void AddCollider()
    {
        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.size = new Vector3(Camera.main.orthographicSize * 2, Camera.main.orthographicSize * 2, 1);
        collider.center = new Vector3(0, 0, 0);
    }

    //stop player if at the edge of the screen
    private void StopPlayer(GameObject Player)
    {
        //when player is at the right edge of the screen
        if (Player.transform.position.x > Camera.main.transform.position.x + Camera.main.orthographicSize + expand_collider_box)
        {
            //lock position of camera
            Camera.main.transform.position = Camera_Pos;
            
            //get max x position of the camera view
            float maxX = Camera.main.transform.position.x + Camera.main.orthographicSize + expand_collider_box;
            //prevent player from moving to right
            Player.transform.position = new Vector3(maxX, Player.transform.position.y, Player.transform.position.z);
            
        }
        //when player is at the left edge of the screen
        else if (Player.transform.position.x < Camera.main.transform.position.x - Camera.main.orthographicSize - expand_collider_box)
        {
            //lock position of camera
            Camera.main.transform.position = Camera_Pos;
            //get min x position of the camera view
            float minX = Camera.main.transform.position.x - Camera.main.orthographicSize - expand_collider_box;
            //prevent player from moving to left
            Player.transform.position = new Vector3(minX, Player.transform.position.y, Player.transform.position.z);
        }
        else
        {
            Camera_Pos = Camera.main.transform.position;
        }
    }


    private float calcFOV(GameObject P1, GameObject P2)
    {
        float fov = 0f;
        if (Max(P1.transform.position.y, P2.transform.position.y) > Camera.main.rect.yMax)
        {
            float GK = Abs(Abs(P1.transform.position.y) - Abs(P2.transform.position.y));
            float aspectRatio = GK / Max(P1.transform.position.x, P2.transform.position.x);
            float AK = Abs(Camera.main.transform.position.z - Max(P1.transform.position.z, P1.transform.position.z));
            double HY = Sqrt(Pow(GK, 2) + Pow(AK, 2));
            double alpha = Asin(GK / HY);
            fov = Mathf.Rad2Deg * (float) alpha * 2;
            fov = Camera.VerticalToHorizontalFieldOfView(fov, aspectRatio);
        }
        else
        {
            float GK = Abs(Abs(P1.transform.position.x) - Abs(P2.transform.position.x));
            float AK = Abs(Camera.main.transform.position.z - Max(P1.transform.position.z, P1.transform.position.z));
            double HY = Sqrt(Pow(GK, 2) + Pow(AK, 2));
            double alpha = Asin(GK / HY);
            fov = Mathf.Rad2Deg * (float) alpha * 2;
        }
        print("FOV: " + fov);
        return fov;
    }
    // Update is called once per frame
    void Update()
    {
        Player1 = GameObject.Find("Player");
        Player2 = GameObject.Find("Player2");
        //create list of players
        List<GameObject> Players = new List<GameObject>();
        Players.Add(Player1);
        Players.Add(Player2);
        
        Middle_Vec = (Player1.transform.position + Player2.transform.position) / 2;
        Middle_Vec.y += 1.5f;
        Middle_Vec.z = -68;
        Camera.main.transform.position = Middle_Vec;
        float fov = calcFOV(Player1, Player2);
        if (fov >= MinFOV && fov <= MaxFOV)
        {
            Camera.main.fieldOfView = fov;
        }
        //iterate through players
        foreach (var Player in Players)
        {
            StopPlayer(Player);
        }
    }
}
