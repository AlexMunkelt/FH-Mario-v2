using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnde : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals("Player")) {
            Debug.Log("Level Ende");
            SceneManager.LoadScene("Menu");
                }
     }

}
