using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HV7
{
    public struct Manager
    {
        string fileName;
        char separator;
        int Count;
        Employer[] employer;
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="fileName">Обязательный параметр</param>
        /// <param name="separator">Сепаратор разделитель каждой строки-записи</param>
        /// <param name="startBuferSize">Начальный буфер (количество строк-записей в файле)</param>
        public Manager(string fileName, char separator,int startBuferSize)
        {
            this.fileName = fileName;
            this.separator = separator;
            this.employer = new Employer[startBuferSize];
            this.Count = 0;
            LoadFile(this.fileName);
        }
        public Manager(string fileName, char separator):this(fileName, separator,100)
        {
        }
        public Manager(string fileName):this(fileName,'#')
        {
        }


        /// <summary>
        /// Парсит одну строку данных используя сепаратор и возвращает один новый Employer
        /// </summary>
        /// <param name="line">Строка для парсинга</param>
        /// <returns></returns>
        Employer SeparateAndFill(string line)
        {
            Employer employer = new Employer();
            string[] words; //= new string[7];
            words = line.Split(separator);
            employer.ID = Convert.ToInt64(words[0]);
            employer.dateTime = Convert.ToDateTime(words[1]);
            employer.fio = words[2];
            //employer.Age = Convert.ToInt32(words[3]);
            employer.height = Convert.ToInt32(words[4]);
            employer.birthDate = Convert.ToDateTime(words[5]);
            employer.birthPlace = words[6];
            return employer;
        }

        /// <summary>
        /// Выводит на печать информацию по одному заданному Employer
        /// </summary>
        /// <param name="employer">Объект для вывода информации</param>
        void PrintEmployerInfo(Employer employer)
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

        /// <summary>
        /// Очищает консоль и выводит стандартный заголовок
        /// </summary>
        void TitleClear()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Справочник «Сотрудники»");
            Console.WriteLine("=======================");
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// Добавляет в массив нового члена (с ресайзом данного массива, при необходимости в два раза от предыдущего значения)
        /// </summary>
        /// <param name="employer">Член для добавления</param>
        void Add(Employer employer)
        {
            if (this.employer.Length <= Count)
            {
                Array.Resize(ref this.employer, this.employer.Length * 2);
            }
            this.employer[Count] = employer; //Count должен быть меньше на единицу чем [index]
            Count++;
        }
        /// <summary>
        /// Чтение всех записей из файла базы данных
        /// </summary>
        void LoadFile(string fileName)
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
            string line = "";

            using (StreamReader stream = new StreamReader(File.Open(fileName, FileMode.Open)))
            {
                while (!stream.EndOfStream)
                {
                    line = stream.ReadLine();
                    if (line != "")
                        Add(SeparateAndFill(line));
                }
            }
            Console.WriteLine($"Файл {fileName} успешно загружен.");
        }



        /// <summary>
        /// Сохранение файла базы данных
        /// </summary>
        /// <param name="fileName"></param>
        void SaveFile(string fileName)
        {
            TitleClear();
            Console.WriteLine("Добавление записи в файл базы данных...");
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"Файл базы данных {new FileInfo(fileName).FullName} будет создан.");
                Console.WriteLine("Для подтверждения введите y , для отмены n");
                if (!(Console.ReadLine().ToLower() == "y")) return;
            }


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
        /// <summary>
        /// Публичный метод. Добавление НОВОГО работника и сохранение в базу данных
        /// </summary>
        public void AddNewRecord()
        {
            Employer employer = new Employer();
            employer.ID = Input<long>("Введите ID работника:");
            employer.dateTime = DateTime.Now;
            employer.fio = Input<string>("Введите ФИО:");
            employer.height = Input<int>("Введите рост:");
            employer.birthDate = Input<DateTime>("Введите дату рождения:");
            employer.birthPlace = Input<string>("Введите место рождения:");
            Add(employer);
            SaveFile(this.fileName);
        }

        /// <summary>
        /// Мой личный улучшенный метод пользовательского ввода
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <returns></returns>
        public T Input<T>(string msg = "")
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
}
