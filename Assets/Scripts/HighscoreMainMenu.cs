using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreMainMenu : MonoBehaviour
{
    public TMPro.TextMeshProUGUI highscore;
    // Start is called before the first frame update
    void Start()
    {
        highscore.text = "Highscore: " + PlayerPrefs.GetFloat("highscore").ToString();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
