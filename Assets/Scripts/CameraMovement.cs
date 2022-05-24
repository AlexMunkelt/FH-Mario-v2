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
        Collider Camera_Collider = Camera.main.GetComponent<Collider>();
        Collider Player_Collider = Player.GetComponent<Collider>();
        
        
        //if player is at the edge of the screen
        if (Player_Collider.bounds.max.x > Camera_Collider.bounds.max.x || Player_Collider.bounds.min.x < Camera_Collider.bounds.min.x ||
            Player_Collider.bounds.max.y > Camera_Collider.bounds.max.y || Player_Collider.bounds.min.y < Camera_Collider.bounds.min.y)
        {
            //change player x position by -1
            Player.transform.position = new Vector3(Player.transform.position.x - 1, Player.transform.position.y, Player.transform.position.z);
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
        /*foreach (var Player in Players)
        {
            StopPlayer(Player);
        }*/
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
