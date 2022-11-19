using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private static GameController _instance;
    public static GameController Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        //Application.targetFrameRate = 60;
    }

    public int lives = 3;

    public Animator carAnimRef;

    public float gameSpeed = 0;

    private BoxScript[] boxes;

    void Start()
    {
    }

    [ContextMenu("StartGame")]
    public void StartGame()
    {
        CarStart();
        boxes = FindObjectsOfType<BoxScript>();
        foreach(BoxScript box in boxes)
        {
            box.startDriving = true;
        }
        gameSpeed = 1;
    }

    public void LoseLife()
    {
        lives--;
        Debug.Log("Lives left" + lives);
    }

    [ContextMenu("Jump")]
    public void CarJump()
    {
        carAnimRef.SetTrigger("Jump");
    }

    public void CarStart()
    {
        carAnimRef.SetBool("Driving", true);
        StartCoroutine(StartDriving());
    }

    IEnumerator StartDriving()
    {
        yield return new WaitForSeconds(1f);
        while (gameSpeed < 1f)
        {
            gameSpeed += 0.001f * Time.deltaTime;
            yield return null;
        }
    }

    [ContextMenu("Stop")]
    public void CarStop()
    {
        gameSpeed = 0;
        carAnimRef.SetBool("Driving", false);
        //foreach (BoxScript box in boxes)
        //{
        //    box.startDriving = false;
        //}
        StartCoroutine(StopDriving());
    }
    IEnumerator StopDriving()
    {
        yield return new WaitForSeconds(1f);
        while (gameSpeed > 0.001f)
        {
            gameSpeed -= 0.001f * Time.deltaTime;
            yield return null;
        }
    }

    void Update()
    {
        if (carAnimRef.GetBool("Driving"))
        {
            carAnimRef.speed = gameSpeed;
        }
    }
}
