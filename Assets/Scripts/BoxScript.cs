using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoxScript : MonoBehaviour
{
    [HideInInspector] public Rigidbody2D rbRef;

    public bool startDriving = false;
    public float pushForce = 3000f;
    public float throwForce = 3000f;
    public bool isHeld = false;

    private float gravity = 1;
    private Vector2 lastHeldPos;
    private Vector2 mousePos;
    private bool alive = true;
    void Start()
    {
        rbRef = GetComponent<Rigidbody2D>();

        gravity = rbRef.gravityScale;
        if(GameController.Instance.gameSpeed > 0.01f)
        {
            startDriving = true;
        }
    }

    void Update()
    {
    }

    public void OnMouseDown()
    {
        if (alive)
        {
            isHeld = true;
            rbRef.gravityScale = 0;
            rbRef.velocity = Vector2.zero;
        }
    }

    public void OnMouseUp()
    {
        if (alive)
        {
            DropBox();
        }
    }

    public void DropBox()
    {
        isHeld = false;
        rbRef.gravityScale = gravity;
        rbRef.AddForce((mousePos - lastHeldPos) * throwForce, ForceMode2D.Force);
    }

    private void FixedUpdate()
    {
        if (isHeld)
        {
            lastHeldPos = mousePos;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rbRef.MovePosition(mousePos);
            //transform.position = mousePos;
            if (Vector2.Distance(transform.position, mousePos) > 3f)
            {
                DropBox();
            }
        }
        else if (startDriving)
        {
            rbRef.AddForce((Vector2.left * pushForce * GameController.Instance.gameSpeed + rbRef.velocity ) * Time.deltaTime, ForceMode2D.Force);
        }
    }

    public void DeleteBox()
    {
        DropBox();
        if (alive)
        {
            GameController.Instance.LoseLife();
        }
        alive = false;
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Dropzone") && alive)
        {
            DropBox();
            alive = false;
            pushForce = pushForce * 5;
            GameController.Instance.LoseLife();
        }
    }
}
