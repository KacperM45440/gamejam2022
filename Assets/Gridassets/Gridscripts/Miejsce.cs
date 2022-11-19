using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zyrafa
{
    [System.Serializable]
    public class Miejsce
    {

        public int posiadanyX;
        public int posiadanyY;

        public Miejsce(int danyX, int danyY)
        {
            Debug.Log(danyX);
            Debug.Log(danyY);
            posiadanyX = danyX;
            posiadanyY = danyY;
        }

        public override string ToString()
        {
            return $"x : {posiadanyX}, y : {posiadanyY}";
        }
    }
}


