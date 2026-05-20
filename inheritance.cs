using System;

class Animal
{
    public void Eat()
    {
        Console.WriteLine("Animal is eating");
    }
}

class Dog : Animal
{
    public void Bark()
    {
        Console.WriteLine("Dog is barking");
    }
}

class BabyDog : Dog
{
    public void Weep()
    {
        Console.WriteLine("Baby Dog is weeping");
    }
}

class Cat : Animal
{
    public void Meow()
    {
        Console.WriteLine("Cat is meowing");
    }
}

class Cow : Animal
{
    public void Moo()
    {
        Console.WriteLine("Cow says Moo");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Single Inheritance");

        Dog d1 = new Dog();

        d1.Eat();
        d1.Bark();

        Console.WriteLine();

        Console.WriteLine("Multilevel Inheritance");

        BabyDog bd1 = new BabyDog();

        bd1.Eat();
        bd1.Bark();
        bd1.Weep();

        Console.WriteLine();

        Console.WriteLine("Hierarchical Inheritance");

        Cat c1 = new Cat();
        Cow c2 = new Cow();

        c1.Eat();
        c1.Meow();

        Console.WriteLine();

        c2.Eat();
        c2.Moo();

        Console.WriteLine();

        for(int i = 1; i <= 3; i++)
        {
            Console.WriteLine("Loop Number: " + i);
        }

        Console.WriteLine();

        int number = 10;

        if(number > 5)
        {
            Console.WriteLine("Number is greater than 5");
        }
        else
        {
            Console.WriteLine("Number is smaller than or equal to 5");
        }
    }
}