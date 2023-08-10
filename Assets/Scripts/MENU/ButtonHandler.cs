using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonHandler : MonoBehaviour
{
    //methods to load the different scenes in the game

    public void LoadGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
