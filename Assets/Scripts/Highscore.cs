using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Highscore : MonoBehaviour
{
    public TMPro.TextMeshProUGUI newScore, highscore;
    // Start is called before the first frame update
    void Start()
    {
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
    }

    private void WriteScore()
    {
        newScore.text = "3.7";
        highscore.text = "2.3";
    }
}
