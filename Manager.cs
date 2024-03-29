﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HV7
{
    public struct Manager
    {
        public enum ESort
        {
            Default,
            SortUp,
            SortDown,
        }
        public ESort sort;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="fileName">Обязательный параметр</param>
        /// <param name="separator">Сепаратор разделитель каждой строки-записи</param>
        /// <param name="startBuferSize">Начальный буфер (количество строк-записей в файле)</param>
        public Manager(string fileName, char separator, int startBuferSize)
        {
            this.fileName = fileName;
            this.separator = separator;
            this.employer = new Employer[startBuferSize];
            this.Count = 0;
            this.lastID = 0;
            this.start = DateTime.MinValue;
            this.end = DateTime.MaxValue;
            this.sort = ESort.Default;
            LoadFile(this.fileName);
        }
        public Manager(string fileName, char separator) : this(fileName, separator, 100)
        {
        }
        public Manager(string fileName) : this(fileName, '#')
        {
        }

        private string fileName;
        private char separator;
        private int Count;
        private long lastID;
        private DateTime start;
        private DateTime end;
        private Employer[] employer;

        /// <summary>
        /// Парсит одну строку данных используя сепаратор и возвращает один новый Employer
        /// </summary>
        /// <param name="line">Строка для парсинга</param>
        /// <returns></returns>
        private Employer SeparateAndFill(string line)
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
        private void PrintEmployerInfo(Employer employer)
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
        /// Подметод печати. Печатает что дали
        /// </summary>
        /// <param name="employer">Дали</param>
        private void PrintEmployersInfo(Employer[] employer)
        {
            for (int i = 0; i < Count; i++)
            {
                if (start <= employer[i].dateTime && employer[i].dateTime <= end)
                {
                    PrintEmployerInfo(employer[i]);
                }
            }
            Console.ReadKey();
        }
        /// <summary>
        /// Установка диапазона дат для сортировки по датам
        /// </summary>
        private void SetSortingDateRange()
        {
            start = Input<DateTime>("Введите нижний интервал дат для отображения", DateTime.Now.Date);
            end  = Input<DateTime>("Введите верхний интервал дат для отображения", DateTime.Now);
            Console.WriteLine("Диапазон дат устанолвлен!");
        }
        /// <summary>
        /// Сброс диапазона дат для сортировки по датам
        /// </summary>
        private void SetSortingDataRangeDefault()
        {
            Console.WriteLine("Диапазон дат сброшен!");
            start = DateTime.MinValue;
            end = DateTime.MaxValue;
        }
        /// <summary>
        /// Добавляет в массив нового члена (с ресайзом данного массива, при необходимости в два раза от предыдущего значения)
        /// </summary>
        /// <param name="employer">Член для добавления</param>
        private void Add(Employer employer)
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
        private void LoadFile(string fileName)
        {
            TitleClear();
            Console.WriteLine("Чтение записей из файла базы данных...");
            if (!File.Exists(fileName))
            {
                Console.WriteLine("Файл базы {0} всецело не найден! Нажмите Enter.", new FileInfo(fileName).FullName);
                Console.ReadLine();
                return;
            }
            string line = "";

            using (StreamReader stream = new StreamReader(File.Open(fileName, FileMode.Open)))
            {
                lastID = Convert.ToInt64(stream.ReadLine());
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
        private void SaveFile(string fileName)
        {
            TitleClear();
            Console.WriteLine("Добавление записи в файл базы данных...");
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"Файл базы данных {new FileInfo(fileName).FullName} будет создан.");
                Console.WriteLine("Для подтверждения введите y , для отмены n");
                if (!(Console.ReadLine().ToLower() == "y")) return;
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(File.Open(fileName, FileMode.Create)))
                {
                    writer.WriteLine(lastID); //Последний ID
                    writer.Write(CombineData(this.Count, employer)); //запись основных данных
                }
                Console.WriteLine("Файл сохранён.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ОШИБКА. Файл не записан! {ex.Message}");
            }
            Console.ReadKey();
        }
        /// <summary>
        /// Обьединяет (сериализует) данные структуры Employer в строку (одна линия), для передачи в поток 
        /// </summary>
        /// <param name="employer">Объект для сериализации</param>
        /// <returns></returns>
        private string CombineData(Employer employer)
        {
            return CombineData(1, employer);
        }
        /// <summary>
        /// Обьединяет (сериализует) данные структуры Employer в строку (много линий), для передачи в поток
        /// </summary>
        /// <param name="count">Количество объектов в массиве (количество будующих линий) для сериализации</param>
        /// <param name="employers">Объект для сериализации</param>
        /// <returns></returns>
        private string CombineData(int count, params Employer[] employers)
        {
            string res="";
            for(int i = 0; i < count; i++)
            {
                res+=employers[i].ID.ToString();
                res+=separator;
                res+=employers[i].dateTime.ToString();
                res+=separator;
                res+=employers[i].fio.ToString();
                res+=separator;
                res+=employers[i].Age.ToString();
                res+=separator;
                res+=employers[i].height.ToString();
                res+=separator;
                res+=employers[i].birthDate.ToShortDateString();
                res+=separator;
                res+=employers[i].birthPlace.ToString();
                res += Environment.NewLine;
            }
            return res;
        }
        /// <summary>
        /// Ищет работника по параметру если находит возвращает true
        /// Возвращает первое попавшееся совпадение, если их несколько
        /// </summary>
        /// <param name="ID">Параметр для поиска</param>
        /// <param name="result">Возвращаемый объект</param>
        /// <returns></returns>
        private bool FindEmployerByID(long ID, out Employer result,out int index)
        {
            index= 0;
            for (int i = 0; i < Count; i++)
            {
                if (employer[i].ID == ID)
                {
                    result = employer[i];
                    index  = i;
                    return true;
                }
            }
            result = new Employer();
            return false;
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
        /// <summary>
        /// Мой личный улучшенный метод пользовательского ввода со значением по умолчанию
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="msg"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T Input<T>(string msg,T defaultValue)
        {
            T res = default(T);
            bool flag = true;
            string rl;
            do
            {
                try
                {
                    Console.WriteLine(msg);
                    Console.Write("Пустое поле заменится на -> ");
                    Console.WriteLine(defaultValue);
                    rl = Console.ReadLine();
                    if(rl == "") {
                        res = defaultValue;
                    }
                    else
                    {
                        res = (T)Convert.ChangeType(rl, typeof(T));
                    }
                    flag = false;
                }
                catch
                {
                    Console.WriteLine("НЕ КОРРЕКТНЫЙ ВВОД!!!");
                }

            } while (flag);
            return res;
        }
        /// <summary>
        /// Публичный метод. Добавление НОВОГО работника и сохранение в базу данных
        /// </summary>
        public void AddNewRecord()
        {
            Employer employer = new Employer();
            //if (Count > 0)
            //{
            //    employer.ID = this.employer[Count - 1].ID + 1;//this.Count+1;//Input<long>("Введите ID работника:");
            //}
            //else
            //{
            //    employer.ID = 1;
            //}
            lastID++;
            employer.ID = lastID;    
            employer.dateTime = DateTime.Now;
            employer.fio = Input<string>("Введите ФИО:");
            employer.height = Input<int>("Введите рост:");
            employer.birthDate = Input<DateTime>("Введите дату рождения:");
            employer.birthPlace = Input<string>("Введите место рождения:");
            Add(employer);
            SaveFile(this.fileName);
        }
        /// <summary>
        /// Редакторование записи
        /// </summary>
        public void EditRecord()
        {
            long ID = Input<long>("Введите ID работника:");
            if(FindEmployerByID(ID, out Employer employer,out int index))
            {
                employer.dateTime = DateTime.Now;
                employer.fio = Input<string>("Введите ФИО:", employer.fio);
                employer.height = Input<int>("Введите рост:", employer.height);
                employer.birthDate = Input<DateTime>("Введите дату рождения:", employer.birthDate);
                employer.birthPlace = Input<string>("Введите место рождения:", employer.birthPlace);
                this.employer[index] = employer;
                SaveFile(this.fileName);
            }
            else
            {
                Console.WriteLine("Работник не найден!");
            }
            Console.ReadKey();
        }
        /// <summary>
        /// Удаляет запись
        /// </summary>
        public void DeleteRecord()
        {
            long ID = Input<long>("Введите ID работника:");

            if (FindEmployerByID(ID, out Employer employer, out int index))
            {   
                Console.WriteLine($"Работник {this.employer[index].fio} будет удалён.");
                Console.WriteLine("Для подтверждения введите y , для отмены n");
                if (!(Console.ReadLine().ToLower() == "y")) return; //Если передумал то return
                //Тут сдвигаем на текущего рабочего следующего -1
                Count--;
                for (int i = index; i < Count; i++)
                {
                    this.employer[i] = this.employer[i + 1];
                }
                SaveFile(this.fileName);
                Console.WriteLine($"Работник {ID} крякнул.");
            }
            else
            {
                Console.WriteLine("Работник не найден!");
            }
            Console.ReadKey();
        }
        /// <summary>
        /// Устанавливает сортировки (вызывает подменю)
        /// </summary>
        public void SetSorting()
        {
            while (true)
            {
                Console.WriteLine("Меню сортировки данных");
                Console.WriteLine("1 - сброс диапазона дат");
                Console.WriteLine("2 - установка диапазона дат");
                Console.WriteLine("3 - сортировка по умолчанию");
                Console.WriteLine("4 - сортировка по дате с самой ранней");
                Console.WriteLine("5 - сортировка по дате с самой поздней");
                Console.WriteLine("0 - ничо не надо");
                switch (Console.ReadLine().ToLower())
                {
                    case "0": break;
                    case "1":
                        SetSortingDataRangeDefault();
                        break;
                    case "2":
                        SetSortingDateRange();
                        break;
                    case "3":
                        sort = ESort.Default;
                        break;
                    case "4":
                        sort = ESort.SortUp;
                        break;
                    case "5":
                        sort = ESort.SortDown;
                        break;
                    default:
                        Console.WriteLine("Команда не распознана!");
                        continue;
                }
                TitleClear();
                break;
            }
        }
        /// <summary>
        /// Публичный метод. Печатает информацию на экране для сотрудника
        /// </summary>
        /// <param name="ID">ID сотрудника</param>
        public void PrintEmployerInfo()
        {
            long ID = Input<long>("Введите ID работника:");
            if (FindEmployerByID(ID, out Employer employer, out int index))
            {
                PrintEmployerInfo(this.employer[index]);
            }
            else
            {
                Console.WriteLine("Работник не найден!");
            }
            Console.ReadKey();
        }
        /// <summary>
        /// Очищает консоль и выводит стандартный заголовок
        /// </summary>
        public void TitleClear()
        {
            string info = $"СОРТИРОВКА: {sort.ToString()}, ДИАПАЗОН ДАТ: {start.ToShortDateString()} - {end.ToShortDateString()}\n";
            info += $"КОЛИЧЕСТВО ЗАПИСЕЙ В БАЗЕ: {Count}, ПОСЛЕДНИЙ ID: {lastID}";
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Справочник «Сотрудники»");
            Console.WriteLine("=======================");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(info);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("=======================");
            Console.ForegroundColor = ConsoleColor.White;
        }
        /// <summary>
        /// Публичный метод. Печатает информацию о всех работниках на экране
        /// Применяет все сортировки
        /// </summary>
        public void PrintEmployersInfo()
        {
            IEnumerable<Employer> employers;

            switch (sort) //сортировка
            {
                case ESort.SortUp:
                    Array.Resize(ref this.employer, Count);
                    employers = from word in this.employer
                                orderby word.dateTime
                                select word;
                    break;
                case ESort.SortDown:
                    Array.Resize(ref this.employer, Count);
                    employers = from word in this.employer
                                orderby word.dateTime descending
                                select word;
                    break;
                default:
                    employers = this.employer;
                    break;
            }

            PrintEmployersInfo(employers.ToArray());

        }
    }
}
