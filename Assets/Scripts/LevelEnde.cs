using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnde : MonoBehaviour
{
    public GameObject Highscore;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player")) {
            Debug.Log("Level Ende");
            Highscore.SetActive(true);
        }
     }

}
