using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iintegerlist
{
    public interface Iintegerlist
    {

        void Add(int item);
        bool Remove(int item);
        bool RemoveAt(int index);
        void GetElement(int index);
        int IndexOf(int item);
        int Count { get; }
        bool Clear();
        void print();
        bool Contains(int item);
    }

    public class Integerlist : Iintegerlist
    {
        private int[] _internalStorage;
        int Iindex = 0;
        int initialSize = 0;


        //default, prvi konstruktor
        public Integerlist()
        {
            _internalStorage = new int[4];
        }

        // drugi konstruktor
        public Integerlist(int initialSize)
        {
            this.initialSize = initialSize;

            if (this.initialSize <= 0)
            {
                Console.WriteLine("Number of elements must be larger than 0!");
                Console.ReadLine();
            }
            else
            {
                _internalStorage = new int[this.initialSize];
            }
        }

        // funkcija za proširivanje polja na novu veličinu 
        private static Array ResizeArray(Array _internalStorage, int newSize)
        {


            var temp = Array.CreateInstance(_internalStorage.GetType().GetElementType(), newSize);
            int length = _internalStorage.Length <= temp.Length ? _internalStorage.Length : temp.Length;
            Array.ConstrainedCopy(_internalStorage, 0, temp, 0, length);
            return temp;
        }



        //Iintegerlist implementation
        public void Add(int item)
        {
            try
            {
                if (Iindex >= _internalStorage.Length) //zero based array
                {
                    int newSize = _internalStorage.Length * 2;
                    //_internalStorage = new int[newSize];
                    _internalStorage = (int[])ResizeArray(_internalStorage, newSize); // dva puta veće polje
                    _internalStorage[Iindex] = item;

                    Iindex++;
                }
                else
                {
                    _internalStorage[Iindex] = item;

                    Iindex++;
                }
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public bool Remove(int item)
        {
            for (int i = 0; i < _internalStorage.Length; i++)
            {
                if (_internalStorage[i] == item)
                {
                    return RemoveAt(i);
                }

            }
            return false;

        }

        public bool RemoveAt(int index)
        {
            if (index >= _internalStorage.Length)
            {
                return false;
            }

            for (int i = index; i < _internalStorage.Length - 1; i++)
            {
                _internalStorage[i] = _internalStorage[i + 1];
            }
            _internalStorage = (int[])ResizeArray(_internalStorage,_internalStorage.Length-1);

            Iindex = Iindex - 1;
            return true;
        }

        public void GetElement(int index)
        {
            try
            {

                Console.Write(_internalStorage[index]);

            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press enter!");
            }


        }

        public int IndexOf(int item)
        {
            for (int i = 0; i < _internalStorage.Length; i++)
            {
                if (_internalStorage[i] == item)
                {
                    return i;
                }
            }

            return -1;

        }

        public int Count
        {
            get
            {
                return Iindex;
            }
        }

        public bool Clear()
        {
            for (int i = Iindex - 1; i >= 0; i--)
            {
                bool outcome = Remove(_internalStorage[i]);
                //Console.WriteLine(outcome + " " + Iindex);
            }
            if (Iindex == 0)
            {
                Console.WriteLine("\nArray is empty!");
                print();
                return true;
            }
            return false;
        }

        public void print()
        {
            for (int i = 0; i < _internalStorage.Length; i++)
            {
                Console.Write(_internalStorage[i] + " ");
            }
        }

        public bool Contains(int item)
        {
            for (int i = 0; i < _internalStorage.Length; i++)
            {
                if (_internalStorage[i] == item)
                {
                    return true;
                }
            }
            return false;
        }
    }
}


