using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Rigidbody rb;

    public int damageOnCollision = 10;

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
                go.GetComponent<Player>().Jump(go.GetComponent<Player>().jumpOnEnemyMult);
                Death();
            } else
            {
                go.GetComponent<Player>().GetHit(damageOnCollision);
                Death();
            }
        }
    }

    void Death()
    {
        Destroy(this.gameObject);
    }
}
