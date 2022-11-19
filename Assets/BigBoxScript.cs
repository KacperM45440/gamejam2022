using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBoxScript : MonoBehaviour
{
    public GameObject boxPrefab;
    public List<GiraffeScript> giraffes = new List<GiraffeScript>();

    private bool finished = false;
    void Start()
    {
        
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
            if (readyGiraffes >= giraffes.Count)
            {
                Debug.Log("finished box");
                foreach (GiraffeScript giraffe in giraffes)
                {
                    giraffe.MakeUnpickable();
                }
                finished = true;
                GameController.Instance.BoxCompleted();
                StartCoroutine(BecomeBox());
            }
        }
    }

    IEnumerator BecomeBox()
    {
        yield return new WaitForSeconds(1f);
        GameObject newBox = Instantiate(boxPrefab);
        newBox.transform.position = transform.position;
        yield return new WaitForSeconds(0.01f);
        Destroy(gameObject);
    }
}
