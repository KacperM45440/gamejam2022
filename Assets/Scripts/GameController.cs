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
    public int points = 3;
    public int howManyPointsToWin = 8;
    public int difficulty = 0;

    public Animator carAnimRef;
    public List<GameObject> bigBoxPrefabs = new List<GameObject>();
    public float gameSpeed = 0;

    private BoxScript[] boxes;

    void Start()
    {
        //TYMCZASOWO
        KurtynaController.Instance.transform.position = new Vector2(KurtynaController.Instance.transform.position.x, -0.3f);
        KurtynaController.Instance.OpenCurtains();
        StartCoroutine(WaitToStart());
    }

    IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(3f);
        StartGame();
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
        int i = Random.Range(0, bigBoxPrefabs.Count);
        GameObject newBigBox = Instantiate(bigBoxPrefabs[i]);
        newBigBox.transform.position = new Vector2(0, 1f);
    }

    public void BoxCompleted()
    {
        if(lives > 0)
        {
            points++;
            if(points >= howManyPointsToWin)
            {
                WinLevel();
            }
            else
            {
                StartCoroutine(NewBigBox());
            }
        }
    }

    public void WinLevel()
    {
        StartCoroutine(DriveOff());
        StartCoroutine(StopDriving());
    }

    IEnumerator DriveOff()
    {
        yield return new WaitForSeconds(3f);
        carAnimRef.SetTrigger("DriveOff");
    }

    IEnumerator NewBigBox()
    {
        yield return new WaitForSeconds(3f);
        BoxAppear();
    }

    public void LoseLife()
    {
        if(points < howManyPointsToWin)
        {
            lives--;
            points--;
            Debug.Log("Lives left" + lives);
            if (lives <= 0)
            {
                LoseGame();
            }
        }
    }

    public void LoseGame()
    {
        Destroy(FindObjectOfType<BigBoxScript>().gameObject);
        GiraffeScript[] giraffes = FindObjectsOfType<GiraffeScript>();
        foreach(GiraffeScript g in giraffes)
        {
            Destroy(g.gameObject);
        }
        Destroy(FindObjectOfType<BigBoxScript>().gameObject);
        CarStop();
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
        int skokWarunek = Random.Range(12, 17) - (difficulty * 5);
        while (lives > 0)
        {
            gameSpeed += 0.01f + (0.005f * difficulty);
            skokCounter++;
            if (skokCounter >= skokWarunek)
            {
                CarJump();
                skokCounter = 0;
                skokWarunek = Random.Range(12, 17) - (difficulty * 5);
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
