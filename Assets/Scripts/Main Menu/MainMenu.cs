using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Button _play;
    [SerializeField] Button _exit;
    public void PlayGame()
    {
        SceneManager.LoadScene("Gamescene");
    }
    public void QuitGame()
    {
        Debug.Log("QUIT");
        SceneManager.LoadScene("Quit");
    }
}
