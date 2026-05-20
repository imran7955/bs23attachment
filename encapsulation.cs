using System;

class BankAccount
{
    private string accountName;
    private double balance;

    public BankAccount(string name, double amount)
    {
        accountName = name;
        balance = amount;
    }

    public void Deposit(double amount)
    {
        if(amount > 0)
        {
            balance = balance + amount;
            Console.WriteLine(amount + " deposited");
        }
        else
        {
            Console.WriteLine("Invalid deposit amount");
        }
    }

    public void Withdraw(double amount)
    {
        if(amount > 0 && amount <= balance)
        {
            balance = balance - amount;
            Console.WriteLine(amount + " withdrawn");
        }
        else
        {
            Console.WriteLine("Invalid withdraw amount");
        }
    }

    public double GetBalance()
    {
        return balance;
    }

    public void ShowAccountInfo()
    {
        Console.WriteLine("Account Holder: " + accountName);
        Console.WriteLine("Current Balance: " + balance);
    }
}

class Program
{
    static void Main(string[] args)
    {
        BankAccount account1 = new BankAccount("Rahim", 1000);

        account1.ShowAccountInfo();

        Console.WriteLine();

        account1.Deposit(500);

        Console.WriteLine();

        account1.Withdraw(300);

        Console.WriteLine();

        Console.WriteLine("Available Balance: " + account1.GetBalance());

        Console.WriteLine();

        if(account1.GetBalance() > 1000)
            Console.WriteLine("Balance is greater than 1000");
        else
            Console.WriteLine("Balance is 1000 or less");

        Console.WriteLine();

        for(int i = 1; i <= 3; i++)
            Console.WriteLine("Transaction Number: " + i);
    }
}