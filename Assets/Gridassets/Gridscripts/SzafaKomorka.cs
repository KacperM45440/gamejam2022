using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Zyrafa
{
    public class SzafaKomorka : MonoBehaviour
    {
        //anchoredposition, jakies cyferki moze do zmiany

        public Miejsce miejsceWSzafie;
        private RectTransform _rectTransform;
        private Szafa _szafa;
        public GameObject NowaZyrafa;
        public void Initialize(Szafa szafa, int x, int y)
        {
            _rectTransform = GetComponent<RectTransform>();
            _szafa = szafa;

            miejsceWSzafie = new Miejsce(x, y);
            _rectTransform.anchoredPosition = new Vector2(100 * x, 100 * -y);
        }

        public void DodajZyrafe(Zyrafa zyrafa)
        {
            //GameObject newZyrafa = Instantiate(NowaZyrafa, transform);
            //SzafaKomorka komorka = newZyrafa.GetComponent<SzafaKomorka>();
            GameObject newZyrafa = Resources.Load($"ItemCell{zyrafa.Szerokosc}x{zyrafa.Wysokosc}") as GameObject;
            ZyrafaKomorka _zyrafaKomorka = Instantiate(newZyrafa, _szafa.transform).GetComponent<ZyrafaKomorka>();
            _zyrafaKomorka.Initialize(zyrafa, miejsceWSzafie, this);
            _zyrafaKomorka.transform.position = transform.position;
        }

        public bool RuszZyrafe(Zyrafa zyrafa, Miejsce poprzedniaPozycja)
        {
            bool tf = _szafa.RuszZyrafe(poprzedniaPozycja, miejsceWSzafie, zyrafa.rozmiar);
            return tf;
        }
    }
}
