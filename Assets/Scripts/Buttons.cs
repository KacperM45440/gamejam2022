using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    [SerializeField] private string clickPlay = "Play";
    //[SerializeField] private string clickQuit = "Quit";
    [SerializeField] private string clickAutors = "Autorzy";
    [SerializeField] private string clickBack = "Menu";

    public void NewBackButton()
    {
        SceneManager.LoadScene(clickBack);

    }
    public void NewPlayButton()
    {
        SceneManager.LoadScene(clickPlay);

    }
    public void NewAutorsButton()
    {
        SceneManager.LoadScene(clickAutors);

    }
    public void NewQuitButton()
    {
        Application.Quit();
    }
}
