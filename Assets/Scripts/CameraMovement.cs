using static System.Math;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private int MaxFOV = 40;
    [SerializeField] private float expandColliderBox = 3.75f;
    
    private GameObject Player1;

    private GameObject Player2;

    private Vector3 Middle_Vec;
    
    private Vector3 Camera_Pos;

    private float MinFOV;

    bool checkCoop()
    {
        return ToggleCoop.instance.coop;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        AddCollider();
        Player1 = GameObject.Find("Player");
        if (checkCoop())
        {
            Player2 = GameObject.Find("Player2");
            Middle_Vec = (Player1.transform.position + Player2.transform.position) / 2;
            Camera.main.transform.position = Middle_Vec;
            MinFOV = Camera.main.fieldOfView;
        }
        else
        {
            Camera.main.transform.position = Player1.transform.position;
        }
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
        if (Player.transform.position.x > Camera.main.transform.position.x + 
            Camera.main.orthographicSize + expandColliderBox)
        {
            //lock position of camera
            Camera.main.transform.position = Camera_Pos;
            
            //get max x position of the camera view
            float maxX = Camera.main.transform.position.x + Camera.main.orthographicSize + expandColliderBox;
            //prevent player from moving to right
            Player.transform.position = new Vector3(maxX, Player.transform.position.y, Player.transform.position.z);
            
        }
        //when player is at the left edge of the screen
        else if (Player.transform.position.x < Camera.main.transform.position.x -
                 Camera.main.orthographicSize - expandColliderBox)
        {
            //lock position of camera
            Camera.main.transform.position = Camera_Pos;
            //get min x position of the camera view
            float minX = Camera.main.transform.position.x - Camera.main.orthographicSize - expandColliderBox;
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
        var p1Pos = P1.transform.position;
        var p2Pos = P2.transform.position;
        if (Max(p1Pos.y, p2Pos.y) > Camera.main.rect.yMax)
        {
            float GK = Abs(Abs(p1Pos.y) - Abs(p2Pos.y));
            float aspectRatio = GK / Max(p1Pos.x, p2Pos.x);
            float AK = Abs(Camera.main.transform.position.z - Max(p1Pos.z, p2Pos.z));
            double HY = Sqrt(Pow(GK, 2) + Pow(AK, 2));
            double alpha = Asin(GK / HY);
            fov = Mathf.Rad2Deg * (float) alpha * 2;
            fov = Camera.VerticalToHorizontalFieldOfView(fov, aspectRatio);
        }
        else
        {
            float GK = Abs(Abs(p1Pos.x) - Abs(p2Pos.x));
            float AK = Abs(Camera.main.transform.position.z - Max(p1Pos.z, p2Pos.z));
            double HY = Sqrt(Pow(GK, 2) + Pow(AK, 2));
            double alpha = Asin(GK / HY);
            fov = Mathf.Rad2Deg * (float) alpha * 2;
        }
        print("FOV: " + fov);
        print("Camera FOV: " + Camera.main.fieldOfView);
        return fov;
    }

    private float easeInSine(float start, float end, float value)
    {
        return (end - start) * 0.5f * (1f + Mathf.Sin(value * Mathf.PI * 0.5f)) + start;
    }
    
    private float easeOutSine(float start, float end, float value)
    {
        return (end - start) * 0.5f * (1f - Mathf.Sin(value * Mathf.PI * 0.5f)) + start;
    }
    
    // Update is called once per frame
    void Update()
    {
        Player1 = GameObject.Find("Player");
        if (checkCoop())
        {
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
                var fps = 1f / Time.deltaTime;
                var cam = Camera.main;
                if (fov > cam.fieldOfView)
                {
                    for (var i = 0f; i <= 1f; i += 0.01f)
                    {
                        cam.fieldOfView = easeInSine(cam.fieldOfView, fov, i);
                    }
                }
                else if (fov < cam.fieldOfView)
                {
                    for (var i = 1f; i >= 0f; i -= 0.01f)
                    {
                        cam.fieldOfView = easeOutSine(cam.fieldOfView, fov, i);
                    }
                }
            }

            //iterate through players
            foreach (var Player in Players)
            {
                StopPlayer(Player);
            }
        }
        else
        {
            Camera.main.transform.position = Player1.transform.position;
        }
    }
}
