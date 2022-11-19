using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zyrafa
{
    [CreateAssetMenu(menuName = "Create New Item")]
    public class Zyrafa : ScriptableObject
    {
        public Miejsce rozmiar;
        
        public int Szerokosc
        {
            get
            {
                return rozmiar.posiadanyX;
            }
        }
        public int Wysokosc
        {
            get
            {
                return rozmiar.posiadanyY;
            }
        }
        public Sprite obrazek;
    }
}
