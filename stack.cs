using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Stack<string> stack = new Stack<string>();

        stack.Push("Alice");
        stack.Push("Bob");
        stack.Push("Kamal");

        Console.WriteLine("Top: " + stack.Peek());

        Console.WriteLine(stack.Pop());
        Console.WriteLine(stack.Pop());

        Console.WriteLine("Remaining items:");

        foreach (string item in stack)
        {
            Console.WriteLine(item);
        }
    }
}