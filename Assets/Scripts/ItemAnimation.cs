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
    public float moveLength = 0.1f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(0, Time.deltaTime * rotationSpeed, 0);

        float y = Mathf.PingPong(Time.time * speed / 100, moveLength / 100) * 6;
        this.transform.position = new Vector3(this.transform.position.x, y, this.transform.position.z);
    }
}
