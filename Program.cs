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
                Console.WriteLine("1 - Вывести все данные на экран");
                Console.WriteLine("2 - Заполнить данные и добавить новую запись");
                Console.WriteLine("3 - Редактировать запись по ID и сохранить файл");
                Console.WriteLine("4 - Удалить запись по ID и сохранить файл");
                Console.WriteLine("0 или q - Выход из приложения");
                switch (Console.ReadLine().ToLower())
                {
                    case "0":
                    case "q":
                    case "й":
                        break;
                    case "1":
                        manager.TitleClear();
                        manager.PrintEmployersInfo();
                        continue;
                    case "2":
                        manager.TitleClear();
                        manager.AddNewRecord();
                        continue;
                    case "3":
                        manager.TitleClear();
                        manager.EditRecord();
                        continue;
                    case "4":
                        manager.TitleClear();
                        manager.DeleteRecord();
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
