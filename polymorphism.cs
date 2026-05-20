using System;

class Animal
{
    public virtual void Sound()
    {
        Console.WriteLine("Animal makes sound");
    }

    public void Show()
    {
        Console.WriteLine("This is Animal class");
    }
}

class Dog : Animal
{
    public override void Sound()
    {
        Console.WriteLine("Dog says Woof");
    }

    public new void Show()
    {
        Console.WriteLine("This is Dog class");
    }

    public void Print()
    {
        Console.WriteLine("Dog Print Method");
    }

    public void Print(string name)
    {
        Console.WriteLine("Dog Name: " + name);
    }

    public void Print(string name, int age)
    {
        Console.WriteLine("Dog Name: " + name);
        Console.WriteLine("Dog Age: " + age);
    }
}

class Cat : Animal
{
    public override void Sound()
    {
        Console.WriteLine("Cat says Meow");
    }
}

class Box
{
    public int length;
    public int width;

    public Box(int l, int w)
    {
        length = l;
        width = w;
    }

    public static Box operator +(Box b1, Box b2)
    {
        Box b3 = new Box(0, 0);

        b3.length = b1.length + b2.length;
        b3.width = b1.width + b2.width;

        return b3;
    }

    public void ShowBox()
    {
        Console.WriteLine("Length: " + length);
        Console.WriteLine("Width: " + width);
    }
}

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Runtime Polymorphism");

        Animal a1;

        a1 = new Dog();
        a1.Sound();

        a1 = new Cat();
        a1.Sound();

        Console.WriteLine();

        Console.WriteLine("Compile Time Polymorphism");

        Dog d1 = new Dog();

        d1.Print();
        d1.Print("Tommy");
        d1.Print("Tommy", 3);

        Console.WriteLine();

        Console.WriteLine("Method Hiding");

        Animal a2 = new Dog();

        a2.Show();

        Dog d2 = new Dog();

        d2.Show();

        Console.WriteLine();

        Console.WriteLine("Operator Overloading");

        Box b1 = new Box(10, 20);
        Box b2 = new Box(5, 15);

        Box b3 = b1 + b2;

        b3.ShowBox();

        Console.WriteLine();

        int marks = 80;

        if(marks >= 50)
            Console.WriteLine("Pass");
        else
            Console.WriteLine("Fail");

        Console.WriteLine();

    }
}