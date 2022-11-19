using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public int points = 0;

    public Animator carAnimRef;
    public GameObject bigBoxPrefab;
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
        BoxAppear();
    }

    public void BoxAppear()
    {
        Debug.Log("tworze pudlo");
        GameObject newBigBox = Instantiate(bigBoxPrefab);
        newBigBox.transform.position = new Vector2(0, 1f);
    }

    public void BoxCompleted()
    {
        points++;
        StartCoroutine(NewBigBox());
    }

    IEnumerator NewBigBox()
    {
        yield return new WaitForSeconds(5f);
        BoxAppear();
    }

    public void LoseLife()
    {
        lives--;
        Debug.Log("Lives left" + lives);
        if(lives <= 0)
        {
            LoseGame();
        }
    }

    public void LoseGame()
    {
        Debug.Log("You lost");
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
        StartCoroutine(PumpUpTheJam());
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
    
    IEnumerator PumpUpTheJam()
    {
        int skokCounter = 0;
        System.Random rnd = new();
        int skokWarunek = rnd.Next(12, 17);
        while (lives > 0)
        {
            gameSpeed += 0.001f;
            Debug.Log(skokCounter);
            skokCounter++;
            if (skokCounter >= skokWarunek)
            {
                CarJump();
                skokCounter = 0;
                skokWarunek = rnd.Next(12, 17);
            }
            yield return new WaitForSeconds(1f);
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
