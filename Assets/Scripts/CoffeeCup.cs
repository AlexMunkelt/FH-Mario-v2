using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoffeeCup : MonoBehaviour
{
    public int healthGain = 10;
    public float speedMult = 2f;
    public float timeUntilOldSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag.Equals("Player"))
        {
            Destroy(this.gameObject);
            GameController.instance.health += 10;
            coll.gameObject.GetComponent<Player>().SpeedPowerup(speedMult, timeUntilOldSpeed);
        }
    }
}
