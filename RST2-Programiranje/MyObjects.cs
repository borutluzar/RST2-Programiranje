using System;
using System.Collections.Generic;
using System.Text;

namespace Uvod
{
    public class FileData
    {
        public FileData(bool checkName)
        {
            this.ContainsSensitiveInfo = checkName;
        }

        /// <summary>
        /// Stores the number of lines in the file
        /// </summary>
        public int NumberOfLines { get; set; }

        /// <summary>
        /// Is true if the file contains my name
        /// </summary>
        public bool ContainsSensitiveInfo { get; private set; }
    }

    public class Student
    {
        public Student(string fn, string ln, DateTime birthDate)
        {
            this.FirstName = fn;
            this.LastName = ln;
            this.BirthDate = birthDate;
            this.Subjects = new List<Subject>();
        }

        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        private DateTime BirthDate { get; set; }

        public List<Subject> Subjects { get; set; }

        public int GetAge()
        {
            int age = 0;

            DateTime today = DateTime.Now;
            age = today.Year - this.BirthDate.Year;

            return age;
        }
    }

    public enum Subject
    {
        Programiranje,
        DiskretnaMatematika,
        RazvojNaprednihSpletnihUporabniškihVmesnikov,
        Algoritmi
    }
}
