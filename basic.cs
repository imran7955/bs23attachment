using System;

namespace BasicClassExample
{
    class Student
    {
        public string name;
        public int age;

        public Student(string studentName, int studentAge)
        {
            name = studentName;
            age = studentAge;
        }

        public void ShowInfo()
        {
            Console.WriteLine("Student Name: " + name);
            Console.WriteLine("Student Age: " + age);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Student s1 = new Student("Rahim", 20);
            Student s2 = new Student("Karim", 22);

            s1.ShowInfo();

            Console.WriteLine();

            s2.ShowInfo();

            Console.WriteLine();
        }
    }
}