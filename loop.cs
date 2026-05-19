using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("For Loop:");

        for (int i = 1; i <= 5; i++)
            Console.WriteLine(i);

        Console.WriteLine("\nWhile Loop:");

        int j = 1;

        while (j <= 5)
        {
            Console.WriteLine(j);
            j++;
        }

        Console.WriteLine("\nForeach Loop:");

        string[] fruits = { "Apple", "Banana", "Mango" };

        foreach (string fruit in fruits)
            Console.WriteLine(fruit);
    }
}