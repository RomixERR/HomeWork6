using System;

namespace HV7
{
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


    }

    
}
