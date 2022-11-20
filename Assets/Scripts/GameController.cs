using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
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
    public Vector2 boxStartPos;

    public Animator carAnimRef;
    public TextMeshProUGUI pointsRef;
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
        if(lives > 0)
        {
            Debug.Log("tworze pudlo");
            int i = Random.Range(0, bigBoxPrefabs.Count);
            GameObject newBigBox = Instantiate(bigBoxPrefabs[i]);
            newBigBox.transform.position = boxStartPos;
        }
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
        StartCoroutine(StopDriving());
        StartCoroutine(DriveOff());
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
        if(points < howManyPointsToWin && points >= 0)
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
        Debug.Log("wygrales zatrzymuje sie");
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
            gameSpeed -= 0.5f * Time.deltaTime;
            yield return null;
        }
    }
    
    IEnumerator PumpUpTheJam()
    {
        int skokCounter = 0;
        int skokWarunek = Random.Range(12, 17) - (difficulty * 5);
        while (lives > 0 && points < howManyPointsToWin)
        {
            gameSpeed += 0.008f + (0.004f * difficulty);
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
        if (carAnimRef.GetBool("Driving") && points < howManyPointsToWin)
        {
            carAnimRef.speed = gameSpeed;
        }
        pointsRef.text = points + "/" + howManyPointsToWin;
    }
}
