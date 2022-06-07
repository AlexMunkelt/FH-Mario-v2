using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public GameObject PauseMenu;
    public Player player1, player2;
    public GameObject levelMusic;
    public GameObject pauseMusic;
    private bool GameIsPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                doResumeGame();
            }
            else
            {
                doPauseGame();
            }
        }
    }

    public void doPauseGame()
    {
        Time.timeScale = 0;
        GameIsPaused = true;
        PauseMenu.SetActive(true);
        player1.canJump = false;
        player1.canMove = false;
        levelMusic.SetActive(false);
        pauseMusic.SetActive(true);
    }
    public void doResumeGame()
    {
        Time.timeScale = 1;
        GameIsPaused = false;
        PauseMenu.SetActive(false);
        player1.canJump = true;
        player1.canMove = true;
        levelMusic.SetActive(true);
        pauseMusic.SetActive(false);
    }

    public void Menu()
    {
        Time.timeScale = 1;
        GameIsPaused = false;
        SceneManager.UnloadSceneAsync("Level1");
        SceneManager.LoadScene("Menu");
    }
}
