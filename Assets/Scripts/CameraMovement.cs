using static System.Math;
using UnityEngine;

/// <summary>
/// Class that handles the camera movement and zoom.
/// </summary>
public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float maxFOV = 25f;
    [SerializeField] private float expandColliderBox = 3.75f;
    private GameObject player1;

    private GameObject player2;

    private Vector3 middleVec;
    
    private Vector3 cameraPos;

    private float minFOV;

    private float cameraFix;
    
    bool CheckCoop()
    {

        return !ToggleCoop.instance.Equals(null);
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

    // add collider to the camera
    private void AddCollider()
    {
        var collider = gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.size = new Vector3(Camera.main.orthographicSize * 2, Camera.main.orthographicSize * 2, 1);
        collider.center = new Vector3(0, 0, 0);
    }

    /// <summary>
    /// Prevents a player from moving when he is at the visible edge of the screen.
    /// </summary>
    /// <param name="player">
    /// The player to check.
    /// </param>
    private void StopPlayer(GameObject player)
    {
        //when player is at the right edge of the screen
        if (player.transform.position.x > Camera.main.transform.position.x + 
            Camera.main.orthographicSize + expandColliderBox)
        {
            //lock position of camera
            Camera.main.transform.position = cameraPos;
            
            //get max x position of the camera view
            var maxX = Camera.main.transform.position.x + Camera.main.orthographicSize + expandColliderBox;
            //prevent player from moving to right
            player.transform.position = new Vector3(maxX, player.transform.position.y, player.transform.position.z);
            
        }
        //when player is at the left edge of the screen
        else if (player.transform.position.x < Camera.main.transform.position.x -
                 Camera.main.orthographicSize - expandColliderBox)
        {
            //lock position of camera
            Camera.main.transform.position = cameraPos;
            //get min x position of the camera view
            var minX = Camera.main.transform.position.x - Camera.main.orthographicSize - expandColliderBox;
            //prevent player from moving to left
            player.transform.position = new Vector3(minX, player.transform.position.y, player.transform.position.z);
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

        if (Max(p1Pos.y, p2Pos.y) > Camera.main.rect.yMax)
        {
            gk = Abs(Abs(p1Pos.y) - Abs(p2Pos.y)) + 2f;
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
            middleVec.z = Camera.main.transform.position.z;
            Camera.main.transform.position = middleVec;
            var fov = CalcFOV(player1, player2);
            if (fov >= minFOV && fov <= maxFOV)
            {
                var fps = 1f / Time.deltaTime;
                if (fov > Camera.main.fieldOfView)
                {
                    //increase camera FOV
                    for (var i = 0f; i <= 1f; i += 0.001f)
                    {
                        Camera.main.fieldOfView = easeInSine(Camera.main.fieldOfView, fov, i);
                    }
                }
                else if (fov < Camera.main.fieldOfView)
                {
                    //decrease camera FOV
                    for (var i = 1f; i >= 0f; i -= 0.001f)
                    {
                        Camera.main.fieldOfView = easeOutSine(Camera.main.fieldOfView, fov, i);
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
            pos.z = Camera.main.transform.position.z;
            pos.y += cameraFix;
            Camera.main.transform.position = pos;
        }
    }
}
