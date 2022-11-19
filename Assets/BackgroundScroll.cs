using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float backgroundSpeed;

    private Renderer rendererRef;

    private void Start()
    {
        rendererRef = GetComponent<Renderer>();
    }

    void Update()
    {
        rendererRef.material.mainTextureOffset += new Vector2(backgroundSpeed * GameController.Instance.gameSpeed * Time.deltaTime, 0);
    }
}
