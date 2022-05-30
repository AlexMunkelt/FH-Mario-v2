using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectLevel : MonoBehaviour
{
    public GameObject MainMenu, LevelMenu;
    public Button SelectedButtonMain, SelectedButtonLevel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Level1()
    {
        SceneManager.UnloadSceneAsync("Menu");
        SceneManager.LoadScene("Level1");
    }

    public void Level2()
    {
        SceneManager.UnloadSceneAsync("Menu");
        SceneManager.LoadScene("Level2");
    }

    public void Level3()
    {
        SceneManager.UnloadSceneAsync("Menu");
        SceneManager.LoadScene("Level3");
    }

    public void ToLevelMenu()
    {
        MainMenu.SetActive(false);
        LevelMenu.SetActive(true);
        SelectedButtonLevel.Select();
    }

    public void ToMainMenu()
    {
        LevelMenu.SetActive(false);
        MainMenu.SetActive(true);
        SelectedButtonMain.Select();
    }

}
