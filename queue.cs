using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        Queue<string> queue = new Queue<string>();

        queue.Enqueue("Alice");
        queue.Enqueue("Bob");
        queue.Enqueue("Jamal");

        Console.WriteLine("Front: " + queue.Peek());

        Console.WriteLine(queue.Dequeue());
        Console.WriteLine(queue.Dequeue());

        Console.WriteLine("Remaining items:");

        foreach (string item in queue)
        {
            Console.WriteLine(item);
        }
    }
}