using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBoxScript : MonoBehaviour
{
    public GameObject boxPrefab;
    public GiraffeScript[] giraffes;

    private bool finished = false;
    private Animator animatorRef;
    void Start()
    {
        animatorRef = GetComponent<Animator>();
        giraffes = FindObjectsOfType<GiraffeScript>();
    }

    void Update()
    {
        if (!finished)
        {
            int readyGiraffes = 0;
            foreach (GiraffeScript giraffe in giraffes)
            {
                if (giraffe.isInsideBox && !giraffe.isColliding && !giraffe.isHeld)
                {
                    readyGiraffes++;
                }
            }
            if (readyGiraffes >= giraffes.Length)
            {
                Debug.Log("finished box");
                foreach (GiraffeScript giraffe in giraffes)
                {
                    giraffe.MakeUnpickable();
                }
                finished = true;
                GameController.Instance.BoxCompleted();
                animatorRef.SetTrigger("Close");
            }
        }
    }

    IEnumerator BecomeBox()
    {
        yield return new WaitForSeconds(0.01f);
        GameObject newBox = Instantiate(boxPrefab);
        newBox.transform.position = transform.position;
        Destroy(gameObject);
    }
}
