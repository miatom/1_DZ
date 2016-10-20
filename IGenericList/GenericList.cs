using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGenericList
{
    public interface IGenericList<X>: IEnumerable<X>
    {
        void Add(X item);
        bool Remove(X item);
        void RemoveAt(int index);
        X GetElement(int index);
        int IndexOf(X item);
        int Count { get; }
        void Clear();
        bool Contains(X item);
    }

    
    

    public class GenericList<X> : IGenericList<X>
    {
        private List<X> _internalList;
        

        public GenericList()
        {
            this._internalList = new List<X>();
           
        }
           

        // interface implementation

        public IEnumerator<X> GetEnumerator()
        {
            return new GenericListEnumerator<X>(this);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(X item)
        {
            try
            {
                _internalList.Add(item);
            }
            catch (NullReferenceException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool Remove(X item)
        {
            return _internalList.Remove(item);
        }

        public void RemoveAt(int index)
        {
             _internalList.RemoveAt(index);
        }

        public X GetElement(int index)
        {
            try
            {
                return _internalList[index];
            }
            catch (ArgumentOutOfRangeException ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
                 
        }

        public int IndexOf(X item)
        {
            return _internalList.IndexOf(item);
        }

        public int Count
        {
            get
            {
                return _internalList.Count();
            }
        }

        public void Clear()
        {
            _internalList.Clear();
        }

        public bool Contains(X item)
        {
            return _internalList.Contains(item);
        }


    }

    public class GenericListEnumerator<X> : IEnumerator<X>
    {
        private GenericList<X> genericList;
        int position;
        //X curr;
        // konstruktor
        public GenericListEnumerator(GenericList<X> genericList)
        {
            this.genericList = genericList;
            position = -1;
            //curr = default(X);
        }

        public X Current
        {
           get
            {
                try
                {
                    
                   return genericList.GetElement(position);
                    
                }
                catch (IndexOutOfRangeException ex) {
                    throw new InvalidOperationException(ex.Message);
                }
                

            }
               
            
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public void Dispose()
        {
           
        }

        public bool MoveNext()
        {
            position++;
            if (position<genericList.Count) {
                
                return true;
            }
            return false;
           
        }

        public void Reset()
        {
            position = -1;
        }
    }

}
