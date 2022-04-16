using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject panelGameUI;
    public GameObject panelGameOverUI;

    [SerializeField] private GridGenerator grid;
    private void Awake()
    {
        gameIsPaused = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
                Resume();
            else
                Pause();
        }    

        if (grid.IsGameOver())
        {
            panelGameOverUI.SetActive(true);
            panelGameUI.SetActive(false);
            gameIsPaused = true;
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        //Time.timeScale = 1f;
        gameIsPaused = false;
        panelGameUI.SetActive(true);
    }

    public void Pause()
    {
        pauseMenuUI.SetActive(true);
        //Time.timeScale = 0f;
        gameIsPaused = true;
        panelGameUI.SetActive(false);
    }

    public void Save()
    {
        SaveSystem.SaveData(grid);
    }

    public void Load()
    {
        if (SaveSystem.LoadData() != null)
            grid = SaveSystem.LoadData();
    }

    public void Retry()
    {
        panelGameOverUI.SetActive(false);
        panelGameUI.SetActive(true);
        gameIsPaused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Home()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
