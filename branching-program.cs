using System;

class Program
{
    static void Main()
    {
        int marks = 75;

        if (marks >= 80)
        {
            Console.WriteLine("Grade A+");
        }
        else if (marks >= 70)
        {
            Console.WriteLine("Grade A");
        }
        else
        {
            Console.WriteLine("Grade B");
        }
    }
}