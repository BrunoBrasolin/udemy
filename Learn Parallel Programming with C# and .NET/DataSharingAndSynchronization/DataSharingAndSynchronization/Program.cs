//--------------------------------------------------------------------------------------------

BankAccount ba = new BankAccount();
List<Task> tasks = new List<Task>();

for (int i = 0; i < 10; i++)
{
    for (int j = 0; j < 1000; j++) tasks.Add(Task.Factory.StartNew(() => ba.Deposit(100)));

    for (int j = 0; j < 1000; j++) tasks.Add(Task.Factory.StartNew(() => ba.Withdraw(100)));
}

Task.WaitAll(tasks.ToArray());
Console.WriteLine(ba.Balance);


//--------------------------------------------------------------------------------------------

Console.WriteLine("\nFinished.");
Console.ReadKey();

class BankAccount
{
    //public object padlock = new object();
    private int balance;
    public int Balance { get { return balance; } set { balance = value; } }


    public void Deposit(int amount)
    {
        //+=
        // Operation 1: temp <- get_Belance() + amount
        // Operation 2: set_Balence(temp)

        //lock (padlock) //Apenas uma thread pode utilizar o código
        //{
        //    Balance += amount;
        //}
        Interlocked.Add(ref balance, amount);
    }

    public void Withdraw(int amount)
    {
        //lock (padlock)
        //{
        //    Balance -= amount;
        //}
        Interlocked.Add(ref balance, -amount);
    }
}