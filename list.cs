using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        List<string> students = new List<string>();

        students.Add("Imran");
        students.Add("Rahim");
        students.Add("Karim");

        students.Insert(1, "Nayeem");

        foreach (string student in students)
        {
            Console.WriteLine(student);
        }

        Console.WriteLine("students contains \"Rahim\" : " + students.Contains("Rahim"));

        Console.WriteLine("index of \"Karim\" : " + students.IndexOf("Karim"));

        students.Remove("Rahim");

        students.RemoveAt(0);

        Console.WriteLine("No. of elements: " + students.Count);

        students.Sort();

        foreach (string student in students)
        {
            Console.WriteLine(student);
        }

        students.Clear();

        Console.WriteLine("No. of elements: " + students.Count);
    }
}