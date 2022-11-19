using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Oxygenist
{
    public class ZyrafaKomorka : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField]
        private Image _tlo;
        
        [SerializeField]
        private Image _ikona;
        
        private GameObject przeciagany;
        private SzafaKomorka _szafaKomorka;
        private Miejsce _pozycja;
        private Zyrafa _zyrafa;

        public void Initialize(Zyrafa zyrafa, Miejsce pozycja, SzafaKomorka szafaKomorka)
        {
            _ikona.sprite = zyrafa.obrazek;
            _pozycja = pozycja;

            _szafaKomorka = szafaKomorka;
            _zyrafa = zyrafa;

            ZresetujPozycje();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _tlo.raycastTarget = false;
            przeciagany = StworzDucha();
        }

        public void OnDrag(PointerEventData eventData)
        {
            przeciagany.transform.position += (Vector3)eventData.delta;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _tlo.raycastTarget = true;

            SzafaKomorka szafaKomorka = eventData.pointerCurrentRaycast.gameObject?.GetComponent<SzafaKomorka>();

            if (szafaKomorka != null)
            {
                bool tf = szafaKomorka.RuszZyrafe(_zyrafa, _pozycja);
                if (tf)
                {
                    ZaktualizujPozycje(szafaKomorka);
                }
            }

            ZresetujPozycje();
            Destroy(przeciagany);
        }

        private GameObject StworzDucha()
        {
            GameObject duch = Instantiate(gameObject, transform.parent);
            duch.GetComponent<CanvasGroup>().alpha = 0.5f;

            return duch;
        }

        private void ZaktualizujPozycje(SzafaKomorka komorka)
        {
            if (komorka != null)
            {
                _szafaKomorka = komorka;
            }
        }

        private void ZresetujPozycje()
        {
            transform.position = _szafaKomorka.transform.position;
            _pozycja = _szafaKomorka.miejsceWSzafie;
        }
    }
}
