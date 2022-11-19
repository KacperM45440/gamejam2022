using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

namespace Zyrafa
{
    public partial class Szafa : MonoBehaviour
    {
        //sprawdzic vary

        public Miejsce rozmiar;
        private SzafaKomorka[,] _siatka;
        private bool[,] invalidGrid;

        public SzafaKomorka[,] siatka
        {
            get
            {
                return _siatka;
            }

            set
            {
                _siatka = value;
            }
        }

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

        public void Initialize()
        {
            siatka = new SzafaKomorka[Szerokosc, Wysokosc];
            invalidGrid = new bool[Szerokosc, Wysokosc];
        }

        public void DodajZyrafe(Zyrafa zyrafa)
        {
            Miejsce pozycja = WezPozycjeZyrafy(zyrafa);

            if (pozycja != null)
            {
                siatka[pozycja.posiadanyX, pozycja.posiadanyY].DodajZyrafe(zyrafa);
                ZaktualizujSzafe(pozycja, zyrafa.rozmiar, true);
            }
            else
            {
                Debug.LogError("Mamy wszystkie zyrafy!");
            }
        }

        public Miejsce WezPozycjeZyrafy(Zyrafa zyrafa)
        {
            for (int y = 0; y < Wysokosc - (zyrafa.Wysokosc - 1); y++)
            {
                for (int x = 0; x < Szerokosc - (zyrafa.Szerokosc - 1); x++)
                {
                    if (!invalidGrid[x, y])
                    {
                        if (SprawdzRozmiarZyrafy(invalidGrid, zyrafa.rozmiar, x, y))
                        {
                            return new Miejsce(x, y);
                        }
                    }
                }
            }
            return null;
        }

        public bool RuszZyrafe(Miejsce pozycja, Miejsce nowaPozycja, Miejsce rozmiarZyrafy)
        {
            bool[,] placeholder = invalidGrid;
            ZaktualizujSzafe(placeholder, pozycja, rozmiarZyrafy, false);

            if (SprawdzRozmiarZyrafy(placeholder, rozmiarZyrafy, nowaPozycja.posiadanyX, nowaPozycja.posiadanyY))
            {
                ZaktualizujSzafe(pozycja, rozmiarZyrafy, false);
                ZaktualizujSzafe(nowaPozycja, rozmiarZyrafy, true);
                return true;
            }
            return false;
        }

        private bool SprawdzRozmiarZyrafy(bool[,] siatka, Miejsce rozmiar, int x, int y)
        {
            int maxX = x + rozmiar.posiadanyX;
            int maxY = y + rozmiar.posiadanyY;

            if (maxX > Szerokosc || maxY > Wysokosc)
            {
                return false;
            }

            for (int _y = y; _y < maxY; _y++)
            {
                for (int _x = x; _x < maxX; _x++)
                {
                    if (siatka[_x, _y]) return false;
                }
            }
            return true;
        }

        private void ZaktualizujSzafe(Miejsce pozycja, Miejsce rozmiar, bool tf)
        {
            ZaktualizujSzafe(invalidGrid, pozycja, rozmiar, tf);
        }

        private void ZaktualizujSzafe(bool[,] grid, Miejsce pozycja, Miejsce rozmiar, bool tf)
        {
            int maxX = pozycja.posiadanyX + rozmiar.posiadanyX;
            int maxY = pozycja.posiadanyY + rozmiar.posiadanyY;

            for (int y = pozycja.posiadanyY; y < maxY; y++)
            {
                for (int x = pozycja.posiadanyX; x < maxX; x++)
                {
                    grid[x, y] = tf;
                }
            }
        }
    }
}
