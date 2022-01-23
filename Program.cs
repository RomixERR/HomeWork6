using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 *  "С диска загружаются записи в выбранном диапазоне дат" - считаю целесообразным сделать так, что изначально
 *  ВСЕ данные будут загружаться так-же точно, как они и находятся в базе
 *  а при просмотре можно будет сортировать данные (фильтр отображения) или упорядочивать.
 *  В противном случае возникает ряд конфликтов, которые не так просто разрешить.
 *  
 *  В частности при загрузке части базы данных (диапазон дат) и последующем сохранении
 *  теряются данные.
 *  
 *  Так - же и с сортировкой данных - Сортировка по возрастанию и убыванию даты.
 *  Проще сортировать данные уже на этапе отображения.
 *  
 *  В моей реализации ID программа всегда будет генерировать уникальный ID, согласно первой записи в базе (lastID).
 *  При удалении любой записи ID больше не занимается, т.е. каждый присвоенный ID является уникальным и не повторяется.
 */
namespace HV7
{
    class Program
    {
        static void Main(string[] args)
        {
            Manager manager = new Manager("Employeses.db");
            while (true)
            {
                Console.WriteLine("1 - Просмотр записи");
                Console.WriteLine("2 - Вывести все данные на экран");
                Console.WriteLine("3 - Создание записи");
                Console.WriteLine("4 - Редактирование записи по ID");
                Console.WriteLine("5 - Cортировка");
                Console.WriteLine("6 - Удаление записи по ID");
                Console.WriteLine("0 или q - Выход из приложения");
                switch (Console.ReadLine().ToLower())
                {
                    case "0":
                    case "q":
                    case "й":
                        break;
                    case "1":
                        manager.TitleClear();
                        manager.PrintEmployerInfo();
                        continue;
                    case "2":
                        manager.TitleClear();
                        manager.PrintEmployersInfo();
                        continue;
                    case "3":
                        manager.TitleClear();
                        manager.AddNewRecord();
                        continue;
                    case "4":
                        manager.TitleClear();
                        manager.EditRecord();
                        continue;
                    case "5":
                        manager.TitleClear();
                        manager.SetSorting();
                        continue;
                    case "6":
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
