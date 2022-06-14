using static System.Math;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private int maxFOV = 20;
    [SerializeField] private float expandColliderBox = 3.75f;
    [SerializeField] private float expandMinFOV = 5f;
    [SerializeField] private float fixFovY = 3f;
    
    private GameObject player1;

    private GameObject player2;

    private Vector3 middleVec;
    
    private Vector3 cameraPos;

    private float minFOV;

    private float cameraFix;

    bool CheckCoop()
    {
        if (ToggleCoop.instance == null)
        {
            return true;
        }

        return ToggleCoop.instance.coop;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        AddCollider();
        player1 = GameObject.Find("Player");
        if (CheckCoop())
        {
            cameraFix = 2.75f;
            player2 = GameObject.Find("Player2");
            middleVec = (player1.transform.position + player2.transform.position) / 2;
            middleVec.z = Camera.main.transform.position.z;
            middleVec.y += cameraFix;
            Camera.main.transform.position = middleVec;
            minFOV = Camera.main.fieldOfView + expandMinFOV;
        }
        else
        {
            cameraFix = 1.75f;
            var tmp = player1.transform.position;
            tmp.y += cameraFix;
            tmp.z = Camera.main.transform.position.z;
            Camera.main.transform.position = tmp;
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
            Camera.main.transform.position = cameraPos;
            
            //get max x position of the camera view
            var maxX = Camera.main.transform.position.x + Camera.main.orthographicSize + expandColliderBox;
            //prevent player from moving to right
            Player.transform.position = new Vector3(maxX, Player.transform.position.y, Player.transform.position.z);
            
        }
        //when player is at the left edge of the screen
        else if (Player.transform.position.x < Camera.main.transform.position.x -
                 Camera.main.orthographicSize - expandColliderBox)
        {
            //lock position of camera
            Camera.main.transform.position = cameraPos;
            //get min x position of the camera view
            var minX = Camera.main.transform.position.x - Camera.main.orthographicSize - expandColliderBox;
            //prevent player from moving to left
            Player.transform.position = new Vector3(minX, Player.transform.position.y, Player.transform.position.z);
        }
        else
        {
            cameraPos = Camera.main.transform.position;
        }
    }


    private float CalcFOV(GameObject p1, GameObject p2)
    {
        var fov = 0f;
        var p1Pos = p1.transform.position;
        var p2Pos = p2.transform.position;
        var gk = new float();
        var ak = new float();
        var hy = new double();
        var alpha = new double();
        
        if (Max(p1Pos.y, p2Pos.y) > Camera.main.rect.yMax)
        {
            gk = Abs(Abs(p1Pos.y) - Abs(p2Pos.y)) + fixFovY;
            var aspectRatio = gk / Max(p1Pos.x, p2Pos.x);
            ak = Abs(Camera.main.transform.position.z - Max(p1Pos.z, p2Pos.z));
            hy = Sqrt(Pow(gk, 2) + Pow(ak, 2));
            alpha = Asin(gk / hy);
            fov = Mathf.Rad2Deg * (float) alpha * 2;
            fov = Camera.VerticalToHorizontalFieldOfView(fov, aspectRatio);
        }
        else
        {
            gk = Abs(Abs(p1Pos.x) - Abs(p2Pos.x));
            ak = Abs(Camera.main.transform.position.z - Max(p1Pos.z, p2Pos.z));
            hy = Sqrt(Pow(gk, 2) + Pow(ak, 2));
            alpha = Asin(gk / hy);
            fov = Mathf.Rad2Deg * (float) alpha * 2;
        }
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
        player1 = GameObject.Find("Player");
        if (CheckCoop())
        {
            player2 = GameObject.Find("Player2");
            //create list of players
            List<GameObject> Players = new List<GameObject>();
            Players.Add(player1);
            Players.Add(player2);

            middleVec = (player1.transform.position + player2.transform.position) / 2;
            middleVec.y += cameraFix;
            middleVec.z = Camera.main.transform.position.z;
            Camera.main.transform.position = middleVec;
            var fov = CalcFOV(player1, player2);
            if (fov >= minFOV && fov <= maxFOV)
            {
                var fps = 1f / Time.deltaTime;
                var cam = Camera.main;
                if (fov > cam.fieldOfView)
                {
                    for (var i = 0f; i <= 1f; i += 0.001f)
                    {
                        cam.fieldOfView = easeInSine(cam.fieldOfView, fov, i);
                    }
                }
                else if (fov < cam.fieldOfView)
                {
                    for (var i = 1f; i >= 0f; i -= 0.001f)
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
            var pos = player1.transform.position;
            pos.z = Camera.main.transform.position.z;
            pos.y += cameraFix;
            Camera.main.transform.position = pos;
        }
    }
}
