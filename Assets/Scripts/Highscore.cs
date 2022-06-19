using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Highscore : MonoBehaviour
{
    public TMPro.TextMeshProUGUI newScore, highscore;
    public GameController gameController;
    public GameObject healthBar;
        
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0;
        healthBar.SetActive(false);
        WriteScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BackToMenu()
    {
        SceneManager.UnloadSceneAsync("Level1");
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1;
    }

    private void WriteScore()
    {
        newScore.text = gameController.highscore.ToString();
        highscore.text = PlayerPrefs.GetFloat("highscore").ToString();
        if(gameController.note > 0 && PlayerPrefs.GetFloat("highscore") < gameController.highscore)
        {
            PlayerPrefs.SetFloat("highscore", gameController.highscore);
            PlayerPrefs.SetFloat("durchschnittsnote", gameController.note);
        }
    }
}
