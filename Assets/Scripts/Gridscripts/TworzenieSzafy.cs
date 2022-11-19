using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Zyrafa
{
    public partial class Szafa : MonoBehaviour
    {
        public GameObject komorkaPrefab;

        private void Awake()
        {
            Initialize();
            StworzKomorkeSzafy();
        }

        private void StworzKomorkeSzafy()
        {
            Debug.Log("odpalam sie");
            for (int y = 0; y < Wysokosc; y++)
            {
                Debug.Log("odpalam sieeeee");
                for (int x = 0; x < Szerokosc; x++)
                {
                    GameObject newKomorka = Instantiate(komorkaPrefab, transform);
                    SzafaKomorka komorka = newKomorka.GetComponent<SzafaKomorka>();
                    siatka[x, y] = komorka;
                    Debug.Log(newKomorka.name);
                    komorka.Initialize(this, x, y);
                }
            }
        }
    }
}
