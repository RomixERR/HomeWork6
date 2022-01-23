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
        /// Публичный метод. Печатает информацию на экране для всех сотрудников
        /// </summary>
        public void PrintEmployersInfo()
        {
            for (int i = 0; i < Count; i++)
            {
                PrintEmployerInfo(employer[i]);
            }
        }
        /// <summary>
        /// Публичный метод. Печатает информацию на экране для сотрудника
        /// </summary>
        /// <param name="ID">ID сотрудника</param>
        public void PrintEmployersInfo(long ID)
        {
            for (int i = 0; i < Count; i++)
            {
                if (employer[i].ID==ID) PrintEmployerInfo(employer[i]);
            }
        }

        /// <summary>
        /// Очищает консоль и выводит стандартный заголовок
        /// </summary>
        public void TitleClear()
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

            try
            {
                using (StreamWriter writer = new StreamWriter(File.Open(fileName, FileMode.OpenOrCreate)))
                {
                    writer.Write(CombineData(this.Count, employer));
                }
                Console.WriteLine("Файл сохранён.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ОШИБКА. Файл не записан! {ex.Message}");
            }
        }
        public void DeleteRecord()
        {
            long ID = Input<long>("Введите ID работника:");

            if (FindEmployerByID(ID, ref employer, out int index))
            {   //Тут сдвигаем в пустое место оставшихся работников
                Console.WriteLine($"Файл базы данных {new FileInfo(fileName).FullName} будет создан.");
                Console.WriteLine("Для подтверждения введите y , для отмены n");
                if (!(Console.ReadLine().ToLower() == "y")) return;

                    
            }
            else
            {
                Console.WriteLine("Работник не найден!");
            }
            SaveFile(this.fileName);



        }

        /// <summary>
        /// Обьединяет (сериализует) данные структуры Employer в строку (одна линия), для передачи в поток 
        /// </summary>
        /// <param name="employer">Объект для сериализации</param>
        /// <returns></returns>
        string CombineData(Employer employer)
        {
            return CombineData(1, employer);
        }

        /// <summary>
        /// Обьединяет (сериализует) данные структуры Employer в строку (много линий), для передачи в поток
        /// </summary>
        /// <param name="count">Количество объектов в массиве (количество будующих линий) для сериализации</param>
        /// <param name="employers">Объект для сериализации</param>
        /// <returns></returns>
        string CombineData(int count, params Employer[] employers)
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
        /// Публичный метод. Добавление НОВОГО работника и сохранение в базу данных
        /// </summary>
        public void AddNewRecord()
        {
            Employer employer = new Employer();
            if (Count > 0)
            {
                employer.ID = this.employer[Count - 1].ID + 1;//this.Count+1;//Input<long>("Введите ID работника:");
            }
            else
            {
                employer.ID = 1;
            }
            employer.dateTime = DateTime.Now;
            employer.fio = Input<string>("Введите ФИО:");
            employer.height = Input<int>("Введите рост:");
            employer.birthDate = Input<DateTime>("Введите дату рождения:");
            employer.birthPlace = Input<string>("Введите место рождения:");
            Add(employer);
            SaveFile(this.fileName);
        }
        public void EditRecord()
        {
            Employer employer = new Employer();
            long ID = Input<long>("Введите ID работника:");
            if(FindEmployerByID(ID, ref employer,out int index))
            {
                employer.dateTime = DateTime.Now;
                employer.fio = Input<string>("Введите ФИО:", employer.fio);
                employer.height = Input<int>("Введите рост:", employer.height);
                employer.birthDate = Input<DateTime>("Введите дату рождения:", employer.birthDate);
                employer.birthPlace = Input<string>("Введите место рождения:", employer.birthPlace);
                this.employer[index] = employer;
            }
            else
            {
                Console.WriteLine("Работник не найден!");
            }
            SaveFile(this.fileName);
        }
        /// <summary>
        /// Ищет работника по параметру если находит возвращает true
        /// Возвращает первое попавшееся совпадение, если их несколько
        /// </summary>
        /// <param name="ID">Параметр для поиска</param>
        /// <param name="result">Возвращаемый объект</param>
        /// <returns></returns>
        bool FindEmployerByID(long ID, ref Employer result,out int index)
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
    }
}
