using System;

class Student
{
    public string Name;
    public int Age;
    public string Department;
    public double CGPA;
}

class Program
{
    static void Main()
    {
        WelcomeMessage();

        PrintSquare(5);

        int sum = AddNumbers(10, 20);
        Console.WriteLine("Sum: " + sum);

        Student student = new Student();

        student.Name = "Imran";
        student.Age = 22;
        student.Department = "CSE";
        student.CGPA = 3.75;

        PrintStudentInfo(student, "Excellent");
    }

    static void WelcomeMessage()
    {
        Console.WriteLine("Welcome to C# Functions");
    }

    static void PrintSquare(int number)
    {
        Console.WriteLine("Square: " + (number * number));
    }

    static int AddNumbers(int a, int b)
    {
        return a + b;
    }

    static void PrintStudentInfo(Student student, string remark)
    {
        Console.WriteLine("Name: " + student.Name);
        Console.WriteLine("Age: " + student.Age);
        Console.WriteLine("Department: " + student.Department);
        Console.WriteLine("CGPA: " + student.CGPA);
        Console.WriteLine("Remark: " + remark);
    }
}