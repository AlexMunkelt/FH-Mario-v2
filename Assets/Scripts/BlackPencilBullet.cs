using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackPencilBullet : MonoBehaviour
{
    [HideInInspector]
    public int speed = 10;
    [HideInInspector]
    public int damage = 10;
    [HideInInspector]
    public bool shootRight = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DeathTimer());
    }

    // Update is called once per frame
    void Update()
    {
        if (shootRight)
        {
            this.transform.Translate(transform.right * speed * Time.deltaTime);
        } else
        {
            this.transform.Translate(-transform.right * speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision coll)
    {
        GameObject go = coll.gameObject;

        if (go.tag.Equals("Player"))
        {
            if (go.transform.position.y > this.transform.position.y)
            {
                go.GetComponent<Player>().Jump(go.GetComponent<Player>().jumpOnEnemyMult);
            }
            else
            {
                go.GetComponent<Player>().GetHit(damage);
            }
        }

        Death();
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }

    IEnumerator DeathTimer()
    {
        yield return new WaitForSeconds(10f);

        Death();
    }
}
