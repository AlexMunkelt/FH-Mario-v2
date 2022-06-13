using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController instance; 

    private void Awake()
    {
        instance = this;
    }

    public TextMeshProUGUI textDisplay;
    public int maxHealth;
   
    public Image healthbarFront;
    public Image healthbarBack;

    [HideInInspector]
    public int health;

    public int collectables;
    public int count_colectables;
    public float note;

    public GameObject tutorial1;
    public GameObject tutorial2;

    // Start is called before the first frame update
    void Start()
    {
        collectables = 0;
        count_colectables = 0;

        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (count_colectables != 0)
        {
            note = count_colectables / collectables;
        }
        if (Input.GetKey(KeyCode.C))
        {
            Debug.Log("You have: " + note + " note");
            Debug.Log("You have: " + collectables + " Collectables");
            Debug.Log("You have: " + count_colectables + " count");
        }

        textDisplay.text = "Durchschnittsnote: " + note;

        healthbarFront.fillAmount = (float) health / (float) maxHealth;

        if (health <= 0)
        {
            GameOver();
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            tutorial1.SetActive(false);
            tutorial2.SetActive(true);
        }
    }

    public void GameOver()
    {
        SceneManager.LoadScene("Menu");
    }

    
}
