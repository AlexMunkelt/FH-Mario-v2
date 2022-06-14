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
    public float timePassed = 0;
    public float highscore = 1000;

    public GameObject tutorial1;
    public GameObject tutorial2;

    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        collectables = 0;
        count_colectables = 0;

        health = maxHealth;

        StartCoroutine(ReduceHS());
    }

    // Update is called once per frame
    void Update()
    {
        if (count_colectables != 0)
        {
            note = count_colectables / collectables;
            highscore = (1000 / note - timePassed / note) * count_colectables;
        }
        if (Input.GetKey(KeyCode.C))
        {
            Debug.Log("You have: " + note + " note");
            Debug.Log("You have: " + collectables + " Collectables");
            Debug.Log("You have: " + count_colectables + " count");
        }

        textDisplay.text = "Durchschnittsnote: " + note + " Highscore: " + highscore;

        healthbarFront.fillAmount = (float) health / (float) maxHealth;

        if (health <= 0)
        {
            GameOver();
        }

        if (tutorial1.active && player.transform.position.x > 50)
        {
            tutorial1.SetActive(false);
        }
        if (!tutorial2.active && player.transform.position.x > 70)
        {
            tutorial2.SetActive(true);
        }
        if (tutorial2.active && player.transform.position.x > 85)
        {
            tutorial2.SetActive(false);
        }
    }

    public void GameOver()
    {
        SceneManager.LoadScene("Menu");
    }

    IEnumerator ReduceHS()
    {
        yield return new WaitForSeconds(1);

        if(timePassed < 1000)
        {
            timePassed++;
        }

        StartCoroutine(ReduceHS());
    }

}
