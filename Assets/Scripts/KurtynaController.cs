using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KurtynaController : MonoBehaviour
{
    private static KurtynaController _instance;
    public static KurtynaController Instance { get { return _instance; } }

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
        //DontDestroyOnLoad(gameObject);
    }

    public Animator animatorRef;

    private void Start()
    {
    }

    public void OpenCurtains()
    {
        animatorRef.SetTrigger("Open");
    }

    public void CloseCurtains()
    {
        animatorRef.SetTrigger("Close");
    }
}
