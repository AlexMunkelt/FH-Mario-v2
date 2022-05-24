using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody rb;

    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    private void OnCollisionEnter(Collision coll)
    {
        GameObject go = coll.gameObject;

        if (go.tag.Equals("Player"))
        {
            if (go.transform.position.y > this.transform.position.y + 1)
            {
                go.GetComponent<Player>().Jump(3f);
                Death();
            }
        }
    }

    void Death()
    {
        Destroy(this.gameObject);
    }
}
