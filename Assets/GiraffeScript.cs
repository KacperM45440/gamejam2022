using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiraffeScript : MonoBehaviour
{
    public bool isHeld = false;
    public bool isInsideBox = false;
    public bool isColliding = false;

    private SpriteRenderer rendererRef;

    private List<Collider2D> collisions = new List<Collider2D>();
    private Vector2 mousePos;
    void Start()
    {
        rendererRef = GetComponentInChildren<SpriteRenderer>();
    }

    public void OnMouseDown()
    {
        isHeld = true;
        transform.parent = null;
        isInsideBox = false;
        isColliding = false;
        Debug.Log(name + "Game Object Click in Progress");
    }

    public void OnMouseUp()
    {
        isHeld = false;
    }

    void Update()
    {
        if (isHeld)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;
        }
        if(collisions.Count > 0)
        {
            Debug.Log(collisions.Count);
            isColliding = true;
        }
        else
        {
            isColliding = false;
        }
        if (isColliding && isInsideBox)
        {
            rendererRef.color = Color.red;
        }
        else
        {
            rendererRef.color = Color.white;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BoxInside"))
        {
            isInsideBox = true;
        }
        if (collision.gameObject.CompareTag("BoxSide") || collision.gameObject.CompareTag("Giraffe"))
        {
            collisions.Add(collision);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("BoxInside") || collision.gameObject.CompareTag("BoxSide")) && !isHeld)
        {
            transform.parent = collision.gameObject.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BoxSide") || collision.gameObject.CompareTag("Giraffe"))
        {
            collisions.Remove(collision);
        }
        if (collision.gameObject.CompareTag("BoxInside"))
        {
            isInsideBox = false;
        }
    }
}
