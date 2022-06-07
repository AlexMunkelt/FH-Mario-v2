using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnimation : MonoBehaviour
{
    [SerializeField]
    public float rotationSpeed = 10f;
    [SerializeField]
    public float speed = 10f;
    [SerializeField]
    public float moveHeight = 1f;

    private Vector3 startPos;
    private Vector3 endPos;

    bool goUp = true;

    // Start is called before the first frame update
    void Start()
    {
        startPos = this.transform.position;
        endPos = startPos + new Vector3(0, moveHeight, 0);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);

        //float y = Mathf.PingPong(Time.time * speed / 100, moveLength / 100) * 6;
        //this.transform.position = new Vector3(this.transform.position.x, y, this.transform.position.z);

        if (this.transform.position.y >= endPos.y - 0.01f && goUp)
        {
            goUp = false;
        } else if (this.transform.position.y <= startPos.y + 0.01f && !goUp)
        {
            goUp = true;
        }

        if (goUp)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, endPos, speed * Time.deltaTime);
        }
        else
        {
            this.transform.position = Vector3.Lerp(this.transform.position, startPos, speed * Time.deltaTime);
        }
    }
}
