using System.Collections.Generic;
using static System.Math;
using UnityEngine;

/// <summary>
/// Class that handles the camera movement and zoom.
/// </summary>
public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float maxFOV = 20f;
    [SerializeField] private float expandColliderBox = 3.75f;
    [SerializeField] private float expandMinFOV = 5f;
    [SerializeField] private float fixFovY = 3f;
    
    private static Camera _cam = Camera.main;
    
    private Transform camTransform = _cam.transform;
    
    private GameObject player1;

    private GameObject player2;

    private Vector3 middleVec;
    
    private Vector3 cameraPos;

    private float minFOV;

    private float cameraFix;
    
    bool CheckCoop()
    {

        return ToggleCoop.instance.Equals(null);
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
            middleVec.z = _cam.transform.position.z;
            middleVec.y += cameraFix;
            camTransform.position = middleVec;
            minFOV = _cam.fieldOfView + expandMinFOV;
        }
        else
        {
            cameraFix = 1.75f;
            var tmp = player1.transform.position;
            tmp.y += cameraFix;
            tmp.z = _cam.transform.position.z;
            camTransform.position = tmp;
        }
    }

    // add collider to the camera
    private void AddCollider()
    {
        var boxCollider = gameObject.AddComponent<BoxCollider>();
        var orthographicSize = _cam.orthographicSize;
        boxCollider.isTrigger = true;
        boxCollider.size = new Vector3(orthographicSize * 2, orthographicSize * 2, 1);
        boxCollider.center = new Vector3(0, 0, 0);
    }

    /// <summary>
    /// Prevents a player from moving when he is at the visible edge of the screen.
    /// </summary>
    /// <param name="player">
    /// The player to check.
    /// </param>
    private void StopPlayer(GameObject player)
    {
        var playerTransform = player.transform;
        //when player is at the right edge of the screen
        if (player.transform.position.x > _cam.transform.position.x + 
            _cam.orthographicSize + expandColliderBox)
        {
            //lock position of camera
            _cam.transform.position = cameraPos;
            
            //get max x position of the camera view
            var maxX = camTransform.position.x + _cam.orthographicSize + expandColliderBox;
            //prevent player from moving to right
            playerTransform.position = new Vector3(maxX, player.transform.position.y, playerTransform.position.z);
            
        }
        //when player is at the left edge of the screen
        else if (player.transform.position.x < _cam.transform.position.x -
                 _cam.orthographicSize - expandColliderBox)
        {
            //lock position of camera
            _cam.transform.position = cameraPos;
            //get min x position of the camera view
            var minX = camTransform.position.x - _cam.orthographicSize - expandColliderBox;
            //prevent player from moving to left
            playerTransform.position = new Vector3(minX, player.transform.position.y, playerTransform.position.z);
        }
        else
        {
            cameraPos = _cam.transform.position;
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

        if (Max(p1Pos.y, p2Pos.y) > _cam.rect.yMax)
        {
            gk = Abs(Abs(p1Pos.y) - Abs(p2Pos.y)) + fixFovY;
            var aspectRatio = gk / Max(p1Pos.x, p2Pos.x);
            ak = Abs(_cam.transform.position.z - Max(p1Pos.z, p2Pos.z));
            hy = Sqrt(Pow(gk, 2) + Pow(ak, 2));
            alpha = Asin(gk / hy);
            fov = Mathf.Rad2Deg * (float) alpha * 2;
            fov = Camera.VerticalToHorizontalFieldOfView(fov, aspectRatio);
        }
        else
        {
            gk = Abs(Abs(p1Pos.x) - Abs(p2Pos.x));
            ak = Abs(_cam.transform.position.z - Max(p1Pos.z, p2Pos.z));
            hy = Sqrt(Pow(gk, 2) + Pow(ak, 2));
            alpha = Asin(gk / hy);
            fov = Mathf.Rad2Deg * (float) alpha * 2;
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
            middleVec.z = _cam.transform.position.z;
            camTransform.position = middleVec;
            var fov = CalcFOV(player1, player2);
            if (fov >= minFOV && fov <= maxFOV)
            {
                if (fov > _cam.fieldOfView)
                {
                    //increase camera FOV
                    for (var i = 0f; i <= 1f; i += 0.001f)
                    {
                        _cam.fieldOfView = easeInSine(_cam.fieldOfView, fov, i);
                    }
                }
                else if (fov < _cam.fieldOfView)
                {
                    //decrease camera FOV
                    for (var i = 1f; i >= 0f; i -= 0.001f)
                    {
                        _cam.fieldOfView = easeOutSine(_cam.fieldOfView, fov, i);
                    }
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
            pos.z = _cam.transform.position.z;
            pos.y += cameraFix;
            camTransform.position = pos;
        }
    }
}
