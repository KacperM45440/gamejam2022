using Oxygenist;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    public Inventory inventory;
    public Item prefab1;
    public Item prefab2;
    // Start is called before the first frame update
    void Start()
    {
        inventory.AddItem(prefab1);
        inventory.AddItem(prefab2);
    }
}
