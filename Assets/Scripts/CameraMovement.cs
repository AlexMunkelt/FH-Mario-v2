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
        Player1 = GameObject.Find("Player");
        Player2 = GameObject.Find("Player2");
        Middle_Vec = (Player1.transform.position + Player2.transform.position) / 2;
        Camera.main.transform.position = Middle_Vec;
        MinFOV = Camera.main.fieldOfView;
    }

    private void StopPlayerIfNotVisible(GameObject P)
    {
        Vector3 player_pos = P.transform.position;
        Rect field = Camera.main.rect;
        float field_max_y = field.yMax;
        float field_max_x = field.xMax;

        if (player_pos.y > field_max_y)
        {
            Vector3 new_pos;
            new_pos.y = field_max_y;
            new_pos.x = player_pos.x;
            new_pos.z = player_pos.z;
            P.transform.position = new_pos;
        }

        if (player_pos.x > field_max_x)
        {
            Vector3 new_pos;
            new_pos.y = player_pos.y;
            new_pos.x = field_max_x;
            new_pos.z = player_pos.z;
            P.transform.position = new_pos;
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
        
        Middle_Vec = (Player1.transform.position + Player2.transform.position) / 2;
        Middle_Vec.y += 1.5f;
        Middle_Vec.z = -68;
        Camera.main.transform.position = Middle_Vec;
        float fov = calcFOV(Player1, Player2);
        if (fov >= MinFOV && fov <= MaxFOV)
        {
            Camera.main.fieldOfView = fov;
            if (Player1.transform.position.y > Camera.main.rect.yMax)
            {
                Player1.GetComponent<Rigidbody>().AddForce(new Vector2(0, Dropdown_speed), ForceMode.VelocityChange);
            }
            if (Player2.transform.position.y > Camera.main.rect.yMax)
            {
                Player2.GetComponent<Rigidbody>().AddForce(new Vector2(0, Dropdown_speed), ForceMode.VelocityChange);
            }
        }
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
