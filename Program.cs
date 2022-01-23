using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace HV7
{
    class Program
    {
        static void Main(string[] args)
        {
            Manager manager = new Manager("Employeses.db");
            while (true)
            {
                TitleClear();
                Console.WriteLine("1 - Вывести данные на экран");
                Console.WriteLine("2 - заполнить данные и добавить новую запись в конец файла");
                Console.WriteLine("0 или q - Выход из приложения");
                switch (Console.ReadLine().ToLower())
                {
                    case "0":
                    case "q":
                    case "й":
                        break;
                    case "1": Read();
                        continue;
                    case "2": Write();
                        continue;
                    default:
                        Console.WriteLine("Введена не распознаная команда!");
                        continue;
                }
                break;

            } 
        }

        

        
    }

    
}
