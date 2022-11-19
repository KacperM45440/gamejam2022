using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiraffeScript : MonoBehaviour
{
    public bool isHeld = false;
    public bool isInsideBox = false;
    public bool isColliding = false;
    public float rotateSpeed = 50f;

    private SpriteRenderer rendererRef;

    private bool pickable = true;
    private List<Collider2D> collisions = new List<Collider2D>();
    private Vector2 mousePos;
    void Start()
    {
        rendererRef = GetComponentInChildren<SpriteRenderer>();
    }

    public void OnMouseDown()
    {
        if (pickable)
        {
            isHeld = true;
            transform.parent = null;
            isInsideBox = false;
            isColliding = false;
        }
    }

    public void OnMouseUp()
    {
        isHeld = false;
    }

    public void MakeUnpickable()
    {
        pickable = false;
    }

    void Update()
    {
        if (isHeld)
        {
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;
            if (Input.GetKeyDown(KeyCode.E))
            {
                transform.Rotate(0, 0, -rotateSpeed);
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                transform.Rotate(0, 0, rotateSpeed);
            }
        }
        if(collisions.Count > 0)
        {
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
        if (collision.gameObject.CompareTag("BoxInside"))
        {
            isInsideBox = true;
        }
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
