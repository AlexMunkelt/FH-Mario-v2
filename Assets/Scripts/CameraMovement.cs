using System;
using UnityEditor;
using static System.Math;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

/// <summary>
/// Class that handles the camera movement and zoom.
/// </summary>
public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float maxFOV = 20f;
    [SerializeField] private float expandColliderBox = 3.75f;
    private GameObject player1;

    private GameObject player2;

    private Vector3 middleVec;
    
    private Vector3 cameraPos;

    private float minFOV;

    private float cameraFix;
    
    private bool verticalfov = false;
    
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
        player1 = GameObject.Find("Player");
        if (CheckCoop())
        {
            cameraFix = 2.75f;
            player2 = GameObject.Find("Player2");
            middleVec = (player1.transform.position + player2.transform.position) / 2;
            middleVec.z = Camera.main.transform.position.z;
            middleVec.y += cameraFix;
            Camera.main.transform.position = middleVec;
            minFOV = Camera.main.fieldOfView;
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

    /// <summary>
    /// Prevents a player from moving when he is at the visible edge of the screen.
    /// </summary>
    /// <param name="player">
    /// The player to check.
    /// </param>
    private void StopPlayer(GameObject player)
    {
        Camera cam;
        Vector3 camPos, playerPos;
        
        //when player is at the right edge of the screen
        if (player.transform.position.x > Camera.main.transform.position.x + 
            Camera.main.orthographicSize + expandColliderBox)
        {
            cam = Camera.main;
            camPos = cam.transform.position;
            playerPos = player.transform.position;
            //lock position of camera
            Camera.main.transform.position = cameraPos;
            
            //get max x position of the camera view
            var maxX = camPos.x + cam.orthographicSize + expandColliderBox;
            //prevent player from moving to right
            player.transform.position = new Vector3(maxX, playerPos.y, playerPos.z);
            
        }
        //when player is at the left edge of the screen
        else if (player.transform.position.x < Camera.main.transform.position.x -
                 Camera.main.orthographicSize - expandColliderBox)
        {
            cam = Camera.main;
            camPos = cam.transform.position;
            playerPos = player.transform.position;
            //lock position of camera
            Camera.main.transform.position = cameraPos;
            //get min x position of the camera view
            var minX = camPos.x - cam.orthographicSize - expandColliderBox;
            //prevent player from moving to left
            player.transform.position = new Vector3(minX, playerPos.y, playerPos.z);
        }
        else
        {
            cameraPos = Camera.main.transform.position;
        }
    }

    /// <summary>
    /// Calculate the camera's field of view based on the distance between the players.
    /// </summary>
    /// <param name="p1">
    /// The first player.
    /// </param>
    /// <param name="p2">
    /// The second player.
    /// </param>
    /// <returns>
    /// The calculated field of view.
    /// </returns>
    private float CalcFOV(GameObject p1, GameObject p2)
    {
        float fov, gk, ak;
        double hy, alpha;
        var p1Pos = p1.transform.position;
        var p2Pos = p2.transform.position;
        var cam = Camera.main;
        var camPos = cam.transform.position;

        if (Max(p1Pos.y, p2Pos.y) > Min(p1Pos.y, p2Pos.y) + 1.9f)
        {
            gk = Max(p1Pos.y, p2Pos.y) - Min(p1Pos.y, p2Pos.y);
            ak = Abs(camPos.z - Max(p1Pos.z, p2Pos.z));
            hy = Sqrt(Pow(gk, 2) + Pow(ak, 2));
            alpha = Asin(gk / hy);
            fov = Mathf.Rad2Deg * (float) alpha * 2;
            fov = Camera.VerticalToHorizontalFieldOfView(fov, cam.aspect);
            maxFOV = 60f;
        }
        else
        {
            gk = Abs(Abs(p1Pos.x) - Abs(p2Pos.x));
            ak = Abs(camPos.z - Max(p1Pos.z, p2Pos.z));
            hy = Sqrt(Pow(gk, 2) + Pow(ak, 2));
            alpha = Asin(gk / hy);
            fov = Mathf.Rad2Deg * (float) alpha * 2;
            maxFOV = 40f;
        }

        return fov;
    }
    
    /// <summary>
    /// Ease In Sine function for camera zoom
    /// </summary>
    /// <param name="start">
    /// Actual FOV of the camera
    /// </param>
    /// <param name="end">
    /// Desired fov for the camera
    /// </param>
    /// <param name="value">
    /// Value between 0 and 1
    /// </param>
    /// <returns>
    /// FOV between start and end based on value
    /// </returns>
    private float easeInSine(float start, float end, float value)
    {
        return (end - start) * 0.5f * (1f + Mathf.Sin(value * Mathf.PI * 0.5f)) + start;
    }
    
    
    /// <summary>
    /// Ease In Sine function for camera zoom
    /// </summary>
    /// <param name="start">
    /// Actual FOV of the camera
    /// </param>
    /// <param name="end">
    /// Desired fov for the camera
    /// </param>
    /// <param name="value">
    /// Value between 0 and 1
    /// </param>
    /// <returns>
    /// FOV between start and end based on value
    /// </returns>
    private float easeOutSine(float start, float end, float value)
    {
        return (end - start) * 0.5f * (1f - Mathf.Sin(value * Mathf.PI * 0.5f)) + start;
    }
    
    /// <summary>
    /// Ease In Quad function for camera zoom
    /// </summary>
    /// <param name="start">
    /// Actual FOV of the camera
    /// </param>
    /// <param name="end">
    /// Desired fov for the camera
    /// </param>
    /// <param name="value">
    /// Value between 0 and 1
    /// </param>
    /// <returns>
    /// FOV between start and end based on value
    /// </returns>
    private float easeOutQuad(float start, float end, float value)
    {
        return -(end - start) * value * (value - 2) + start;
    }
    
    /// <summary>
    /// Ease Out Quad function for camera zoom
    /// </summary>
    /// <param name="start">
    /// Actual FOV of the camera
    /// </param>
    /// <param name="end">
    /// Desired fov for the camera
    /// </param>
    /// <param name="value">
    /// Value between 0 and 1
    /// </param>
    /// <returns>
    /// FOV between start and end based on value
    /// </returns>
    private float easeInQuad(float start, float end, float value)
    {
        return 0f;
    }

    private void FixedUpdate()
    {
        //get bottom edge of the screen
        var ground = GameObject.Find("Ground").transform.position;
        var p1Pos = player1.transform.position;
        var p2Pos = player2.transform.position;
        var camrect = Camera.main.rect;
        var campos = Camera.main.transform.position;
        p1Pos.y += 1.9f;
        p2Pos.y += 1.9f;
        //Debug.DrawLine(p1Pos, p2Pos, Color.red, 0);
        //Debug.DrawLine(p1Pos, new Vector2(p1Pos.x, Screen.height), Color.magenta, 0);
        //Debug.DrawLine(p2Pos, new Vector2(p2Pos.x, Screen.height), Color.magenta, 0);
        //Debug.DrawLine(p1Pos, new Vector3(p1Pos.x, ground.y, p1Pos.z), Color.blue, 0);
        //Debug.DrawLine(p2Pos, new Vector3(p2Pos.x, ground.y, p2Pos.z), Color.blue, 0);
        //get vector between players


        //Debug.DrawLine(p1Pos, campos, Color.magenta, 0);
    }

    // Update is called once per frame
    void Update()
    {
        player1 = GameObject.Find("Player");
        if (CheckCoop())
        {
            player2 = GameObject.Find("Player2");
            //create list of players
            var players = new []{player1, player2};
            
            //get the middle position of the players
            middleVec = (player1.transform.position + player2.transform.position) / 2;
            middleVec.y += cameraFix;
            middleVec.z = Camera.main.transform.position.z;
            Camera.main.transform.position = middleVec;
            var camfov = Camera.main.fieldOfView;
            var fov = CalcFOV(player1, player2);
            if (fov >= minFOV && fov <= maxFOV || fov > maxFOV)
            {
                if (fov > Camera.main.fieldOfView)
                {
                    Camera.main.fieldOfView = Mathf.Lerp(camfov, fov, Time.deltaTime * Max(player1.GetComponent<Player>().speed, player2.GetComponent<Player>().speed));
                    //increase camera FOV
                    /*for (var i = 0f; i <= 1f; i += 0.001f)
                    {
                        Camera.main.fieldOfView = easeInQuad(camfov, fov, i);
                    }*/
                }
                else if (fov < Camera.main.fieldOfView)
                {
                    Camera.main.fieldOfView = Mathf.Lerp(camfov, fov, Time.deltaTime);
                    //decrease camera FOV
                    /*for (var i = 1f; i >= 0f; i -= 0.001f)
                    {
                        Camera.main.fieldOfView = easeOutQuad(camfov, fov, i);
                    }*/
                }
            }

            //iterate through players
            foreach (var player in players)
            { 
                StopPlayer(player);
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
