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
    void Start()
    {
        rbRef = GetComponent<Rigidbody2D>();

        gravity = rbRef.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        isHeld = true;
        rbRef.gravityScale = 0;
        rbRef.velocity = Vector2.zero;
        Debug.Log(name + "Game Object Click in Progress");
    }

    public void OnMouseUp()
    {
        DropBox();
    }

    public void DropBox()
    {
        isHeld = false;
        rbRef.gravityScale = gravity;
        rbRef.AddForce((mousePos - lastHeldPos) * throwForce, ForceMode2D.Force);
        Debug.Log(name + "No longer being clicked");
    }

    private void FixedUpdate()
    {
        if (isHeld)
        {
            lastHeldPos = mousePos;
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePos;
        }
        else if (startDriving && pushForce > 0)
        {
            rbRef.AddForce((Vector2.left * pushForce + rbRef.velocity ) * Time.deltaTime, ForceMode2D.Force);
        }
    }
}
