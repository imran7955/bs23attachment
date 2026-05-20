using System;

abstract class Animal
{
    public string name;

    public Animal(string animalName)
    {
        name = animalName;
    }

    public abstract void Sound();

    public void ShowName()
    {
        Console.WriteLine("Animal Name: " + name);
    }
}

class Dog : Animal
{
    public Dog(string animalName) : base(animalName)
    {
    }

    public override void Sound()
    {
        Console.WriteLine(name + " says Woof");
    }
}

class Cat : Animal
{
    public Cat(string animalName) : base(animalName)
    {
    }

    public override void Sound()
    {
        Console.WriteLine(name + " says Meow");
    }
}

class Program
{
    static void Main(string[] args)
    {
        Dog d1 = new Dog("Tommy");
        Cat c1 = new Cat("Kitty");

        d1.ShowName();
        d1.Sound();

        Console.WriteLine();

        c1.ShowName();
        c1.Sound();

        Console.WriteLine();

        int age = 5;

        if(age > 3)
            Console.WriteLine("Animal is adult");
        else
            Console.WriteLine("Animal is baby");

        Console.WriteLine();

        for(int i = 1; i <= 3; i++)
            Console.WriteLine("Animal Count: " + i);
    }
}