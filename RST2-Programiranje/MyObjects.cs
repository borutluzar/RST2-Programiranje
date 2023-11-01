using System;
using System.Collections.Generic;
using System.Text;

namespace Uvod
{
    /// <summary>
    /// Object for saving the read file's attibutes
    /// </summary>
    public class FileData
    {
        /// <summary>
        /// Constructors takes a bool value denoting whether file's
        /// name contains some value or not
        /// </summary>
        public FileData(bool checkName)
        {
            this.ContainsSensitiveInfo = checkName;
        }

        public FileData()
        {
            this.ContainsSensitiveInfo = default;
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

    /// <summary>
    /// Object that carries properties of a student
    /// </summary>
    public class Student
    {
        private int id;

        /// <summary>
        /// Constructor that does not need the first name
        /// </summary>
        /// <param name="lastName">Last name</param>
        /// <param name="birthDate">Birth date</param>
        public Student(string lastName, DateTime birthDate)
        {
            this.LastName = lastName;
            this.BirthDate = birthDate;
            this.Subjects = new List<Subject>();
        }

        /// <summary>
        /// Constructor that needs the first and the last name
        /// </summary>
        /// <param name="fn">First name</param>
        /// <param name="ln">Last name</param>
        /// <param name="birthDate">Birth date</param>
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
            DateTime today = DateTime.Now;
            int age = today.Year - this.BirthDate.Year;

            return age;
        }
    }

    /// <summary>
    /// Enumeration of possible subjects
    /// </summary>
    public enum Subject
    {
        Programiranje,
        DiskretnaMatematika,
        RazvojNaprednihSpletnihUporabniškihVmesnikov,
        Algoritmi
    }
}
