using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HV7
{
    class Program
    {
        const string fileName = "Employeses.db";
        const char separator = '#';
        static void Main(string[] args)
        {
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

        static void TitleClear()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Справочник «Сотрудники»");
            Console.WriteLine("=======================");
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void Read()
        {
            TitleClear();
            Console.WriteLine("Чтение записей из файла базы данных...");
            if (!File.Exists(fileName))
            {
                Console.WriteLine("Файл базы {0} всецело не найден! Нажмите Enter.", new FileInfo(fileName).FullName);
                Console.ReadLine();
                return;
            }

            //List <Employer> employers = new List<Employer>();
            string line="";

            using (StreamReader stream = new StreamReader(File.Open(fileName,FileMode.Open)))
            {
                while (!stream.EndOfStream)
                {
                    //employers.Add (Employer.SeparateAndFill(stream.ReadLine()));
                    line = stream.ReadLine();
                    if (line != "") 
                        Employer.PrintEmployerInfo(Employer.SeparateAndFill(line));
                }
            }
            Console.WriteLine("=======================");
            Console.WriteLine("Для продолжения нажмите Enter");
            Console.ReadKey();
        }




        static void Write()
        {
            TitleClear();
            Console.WriteLine("Добавление записи в файл базы данных...");
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"Файл базы данных {new FileInfo(fileName).FullName} будет создан.");
                Console.WriteLine("Для подтверждения введите y , для отмены n");
                if (!(Console.ReadLine().ToLower() == "y")) return;
            }
            Employer employer = new Employer();
            employer.ID = Input<long>("Введите ID работника:");
            employer.dateTime = DateTime.Now;
            employer.fio = Input<string>("Введите ФИО:");
            employer.height = Input<int>("Введите рост:");
            employer.birthDate = Input<DateTime>("Введите дату рождения:");
            employer.birthPlace = Input<string>("Введите место рождения:");

            using (StreamWriter stream = new StreamWriter(File.Open(fileName, FileMode.Append)))
            {
                stream.WriteLine();
                stream.Write(employer.ID);
                stream.Write(separator);
                stream.Write(employer.dateTime);
                stream.Write(separator);
                stream.Write(employer.fio);
                stream.Write(separator);
                stream.Write(employer.Age);
                stream.Write(separator);
                stream.Write(employer.height);
                stream.Write(separator);
                stream.Write(employer.birthDate.ToShortDateString());
                stream.Write(separator);
                stream.Write(employer.birthPlace);
            }
            Console.WriteLine("Запись сохранена.");
            Console.WriteLine("Нажмите Enter для продолжения");
            Console.ReadKey();

        }

        


        static T Input<T>(string msg="")
        {
            T res = default(T);
            bool flag = true;
            do
            {
                try
                {
                    Console.WriteLine(msg);
                    //Object.ReferenceEquals(typeof(T), typeof(int)))
                    res = (T)Convert.ChangeType(Console.ReadLine(), typeof(T));
                    flag = false;
                }
                catch
                {
                    Console.WriteLine("НЕ КОРРЕКТНЫЙ ВВОД!!!");
                }

            } while (flag);
            return res;
        }

        
    }
    public struct Employer
    {
        public long ID;
        public DateTime dateTime;
        public string fio;
        public int Age
        {
            get
            {
               var age = DateTime.Now.Year - birthDate.Year;
               if (DateTime.Now.DayOfYear < birthDate.DayOfYear) age++;
                return Convert.ToInt32( age);
            }
        }
        public int height;
        public DateTime birthDate;
        public string birthPlace;
        public static Employer SeparateAndFill(string line)
        {
            Employer employer = new Employer();
            string[] words; //= new string[7];
            words = line.Split('#');
            employer.ID = Convert.ToInt64(words[0]);
            employer.dateTime = Convert.ToDateTime(words[1]);
            employer.fio = words[2];
            //employer.Age = Convert.ToInt32(words[3]);
            employer.height = Convert.ToInt32(words[4]);
            employer.birthDate = Convert.ToDateTime(words[5]);
            employer.birthPlace = words[6];
            return employer;
        }

        public static void PrintEmployerInfo(Employer employer)
        {
            Console.WriteLine();
            Console.WriteLine($"{"ID:",40} {employer.ID}");
            Console.WriteLine($"{"Дата и время добавления записи:",40} {employer.dateTime}");
            Console.WriteLine($"{"Ф. И. О.",40} {employer.fio}");
            Console.WriteLine($"{"Возраст:",40} {employer.Age}");
            Console.WriteLine($"{"Рост:",40} {employer.height}");
            Console.WriteLine($"{"Дата рождения:",40} {employer.birthDate.ToShortDateString()}");
            Console.WriteLine($"{"Место рождения:",40} { employer.birthPlace}");
        }
    }

    
}
