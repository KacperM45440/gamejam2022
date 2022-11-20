using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public TextMeshProUGUI recordText;

    void Start()
    {
        if (PlayerPrefs.HasKey("Speed"))
        {
            recordText.text = "Rekordowa predkosc: " + PlayerPrefs.GetFloat("Speed") + " km/h";
        }
        StartCoroutine(WaitSecond());
    }

    IEnumerator WaitSecond()
    {
        yield return new WaitForSeconds(1f);
        KurtynaController.Instance.OpenCurtains();
    }

    IEnumerator GoTo(int number)
    {
        yield return new WaitForSeconds(1f);
        if(number < 0)
        {
            Application.Quit();
        }
        SceneManager.LoadScene(number);
    }

    public void GoToMenu()
    {
        KurtynaController.Instance.CloseCurtains();
        StartCoroutine(GoTo(0));
    }

    public void GoPlay()
    {
        KurtynaController.Instance.CloseCurtains();
        StartCoroutine(GoTo(2));
    }

    public void ExitGame()
    {
        KurtynaController.Instance.CloseCurtains();
        StartCoroutine(GoTo(-1));
    }

    public void GoToAuthors()
    {
        KurtynaController.Instance.CloseCurtains();
        StartCoroutine(GoTo(1));
    }
}
