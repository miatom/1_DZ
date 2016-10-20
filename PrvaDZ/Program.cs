using IGenericList;
using Iintegerlist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrvaDZ
{
    class Program
    {
        static void Main(string[] args)
        {

            var listOfIntegers = new Integerlist(5);

            ListExample(listOfIntegers);

            /*
            var polje = new Integerlist(4);
            polje.Add(3);
            polje.Add(4);
            polje.Add(7);
            polje.Add(6);
            polje.Remove(7);
            polje.Add(2);
            polje.Add(8);
            polje.Remove(4);

            Console.WriteLine(polje.Contains(1));  //false
            polje.GetElement(4);       //0
            Console.WriteLine("\nNumber of elements" + " " + polje.Count); //4
            polje.print();  //3 6 2 8 0 0 0 0 
            Console.WriteLine(polje.Clear()); //True
            
*/
            IGenericList<string> stringList = new GenericList<string>();
            stringList.Add(" Hello ");
            stringList.Add(" World ");
            stringList.Add("!");

            foreach (string value in stringList)
            {
                Console.WriteLine(value);
            }

            IEnumerator<string> enumerator = stringList.GetEnumerator();
            while (enumerator.MoveNext())
            {
                string value = (string)enumerator.Current;
                Console.WriteLine(value);
            }
            Console.ReadLine();

        }

          public static void ListExample(Integerlist listOfIntegers)
        {
            listOfIntegers.Add(1); // [1]
            listOfIntegers.Add(2); // [1 ,2]
            listOfIntegers.Add(3); // [1 ,2 ,3]
            listOfIntegers.Add(4); // [1 ,2 ,3 ,4]
            listOfIntegers.Add(5); // [1 ,2 ,3 ,4 ,5]
            listOfIntegers.print();
            Console.WriteLine();
            listOfIntegers.RemoveAt(0); // [2 ,3 ,4 ,5]
            listOfIntegers.print();
            Console.WriteLine();
            listOfIntegers.Remove(5); //[2 ,3 ,4]
            listOfIntegers.print();
            Console.WriteLine();
            Console.WriteLine(listOfIntegers.Count); // 3
            Console.WriteLine();
            Console.WriteLine(listOfIntegers.Remove(100)); // false
            Console.WriteLine(listOfIntegers.RemoveAt(5)); // false
            listOfIntegers.Clear(); // []
            Console.WriteLine(listOfIntegers.Count); // 0
        }
    
    }
   }

    


       





    