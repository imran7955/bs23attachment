using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Dictionary<string, int> ages = new Dictionary<string, int>();

        ages["Alice"] = 25;
        ages["Bob"] = 30;
        ages["Charlie"] = 22;

        Console.WriteLine("Bob's age: " + ages["Bob"]);

        foreach (KeyValuePair<string, int> item in ages)
        {
            Console.WriteLine(item.Key + " is " + item.Value + " years old");
        }

        if (ages.ContainsKey("Alice"))
        {
            Console.WriteLine("Alice exists in the dictionary");
        }

        ages.Remove("Charlie");

        foreach (var item in ages)
        {
            Console.WriteLine(item.Key + " -> " + item.Value);
        }
    }
}