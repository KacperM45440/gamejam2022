using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public AudioSource carAudioRef;
    public TextMeshProUGUI pointsRef;
    public GameObject life1;
    public GameObject life2;
    public GameObject life3;
    public List<GameObject> bigBoxPrefabs = new List<GameObject>();
    public float gameSpeed = 0;
    [HideInInspector] public AudioSource audioRef;

    public AudioClip carStart;
    public AudioClip carLoop;
    public AudioClip boxLost;
    public AudioClip boxCompleted;
    public AudioClip wonGame;
    public AudioClip lostGame;

    private BoxScript[] boxes;

    void Start()
    {
        audioRef = GetComponent<AudioSource>();
        //TYMCZASOWO
        KurtynaController.Instance.transform.position = new Vector2(KurtynaController.Instance.transform.position.x, -0.3f);
        KurtynaController.Instance.OpenCurtains();
        StartCoroutine(WaitToStart());
    }

    IEnumerator WaitToStart()
    {
        yield return new WaitForSeconds(2f);
        audioRef.PlayOneShot(carStart);
        yield return new WaitForSeconds(1f);
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
        audioRef.PlayOneShot(boxCompleted);
        if (lives > 0)
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
        audioRef.PlayOneShot(wonGame);
    }

    IEnumerator DriveOff()
    {
        yield return new WaitForSeconds(3f);
        carAnimRef.SetTrigger("DriveOff");
        yield return new WaitForSeconds(5f);
        KurtynaController.Instance.CloseCurtains();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(3);
    }

    IEnumerator NewBigBox()
    {
        yield return new WaitForSeconds(3f);
        BoxAppear();
    }

    public void LoseLife()
    {
        if(points < howManyPointsToWin && points > 0)
        {
            lives--;
            points--;
            audioRef.PlayOneShot(boxLost);
            if (lives == 2)
            {
                life1.SetActive(true);
            }
            else if (lives == 1)
            {
                life2.SetActive(true);
            }
            else
            {
                life3.SetActive(true);
            }
            Debug.Log("Lives left" + lives);
            if (lives == 0)
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
        audioRef.PlayOneShot(lostGame);
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
        StartCoroutine(CarSound());
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

    IEnumerator CarSound()
    {
        yield return new WaitForSeconds(2f);
        audioRef.clip = carLoop;
        audioRef.loop = true;
        audioRef.Play();
    }

    [ContextMenu("Stop")]
    public void CarStop()
    {
        if(!PlayerPrefs.HasKey("Speed") || PlayerPrefs.GetFloat("Speed") < gameSpeed)
        {
            PlayerPrefs.SetFloat("Speed", gameSpeed);
            PlayerPrefs.Save();
        }
        gameSpeed = 0;
        carAnimRef.SetBool("Driving", false);
        //foreach (BoxScript box in boxes)
        //{
        //    box.startDriving = false;
        //}
        StartCoroutine(StopDriving());
        audioRef.Stop();
    }
    IEnumerator StopDriving()
    {
        yield return new WaitForSeconds(1f);
        while (gameSpeed > 0.001f)
        {
            gameSpeed -= 0.5f * Time.deltaTime;
            yield return null;
        }
        if(lives <= 0)
        {
            yield return new WaitForSeconds(2f);
            KurtynaController.Instance.CloseCurtains();
            yield return new WaitForSeconds(2f);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    
    IEnumerator PumpUpTheJam()
    {
        int skokCounter = 0;
        int skokWarunek = Random.Range(12, 17) - (difficulty * 5);
        while (lives > 0 && points < howManyPointsToWin)
        {
            gameSpeed += 0.005f + (0.004f * difficulty);
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
        Debug.Log("Game speed is: " + gameSpeed);
        if (carAnimRef.GetBool("Driving") && points < howManyPointsToWin)
        {
            carAnimRef.speed = gameSpeed;
        }
        pointsRef.text = points + "/" + howManyPointsToWin;
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(ExitToMenu());
        }
    }

    IEnumerator ExitToMenu()
    {
        KurtynaController.Instance.CloseCurtains();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0);
    }
}
