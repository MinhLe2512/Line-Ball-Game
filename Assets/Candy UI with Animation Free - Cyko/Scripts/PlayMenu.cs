using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayMenu : MonoBehaviour
{
    public void Play()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }
    public void Quit()
    {
        Application.Quit();
    }
}
