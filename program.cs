using System;

public interface IInternetState
{
    void DeductMonthlyFee();
    void ConnectToNetwork();
}

public class NormalState : IInternetState
{
    private readonly InternetSubscriber subscriber;

    public NormalState(InternetSubscriber subscriber)
    {
        this.subscriber = subscriber;
    }

    public void DeductMonthlyFee()
    {
        if (subscriber.Balance >= subscriber.MonthlyFee)
        {
            subscriber.Balance -= subscriber.MonthlyFee;
            Console.WriteLine($"Щомісячна плата вираховується. Поточний баланс: {subscriber.Balance}");
        }
        else
        {
            Console.WriteLine("Недостатньо коштів для відрахування щомісячної плати. Перехід до стану заборгованості.");
            subscriber.SetState(new ArrearsState(subscriber));
        }
    }

    public void ConnectToNetwork()
    {
        Console.WriteLine($"Підключення встановлено. швидкість: {subscriber.MaxSpeed} Mb/s");
    }
}

public class ArrearsState : IInternetState
{
    private readonly InternetSubscriber subscriber;

    public ArrearsState(InternetSubscriber subscriber)
    {
        this.subscriber = subscriber;
    }

    public void DeductMonthlyFee()
    {
        Console.WriteLine("Виявлена ​​заборгованість. Швидкість зменшено до 0. Будь ласка, погасіть заборгованість.");
    }

    public void ConnectToNetwork()
    {
        Console.WriteLine("Підключення призупинено через заборгованість. Будь ласка, погасіть заборгованість.");
    }
}

public class InternetSubscriber
{
    private double balance;
    public double MonthlyFee { get; private set; }
    public double MaxSpeed { get; private set; }
    private IInternetState currentState;

    public double Balance
    {
        get { return balance; }
        set
        {
            balance = value;
            currentState.DeductMonthlyFee();
        }
    }

    public InternetSubscriber(double initialBalance, double monthlyFee, double maxSpeed)
    {
        this.balance = initialBalance;
        this.MonthlyFee = monthlyFee;
        this.MaxSpeed = maxSpeed;
        this.currentState = new NormalState(this);
    }

    public void SetState(IInternetState state)
    {
        currentState = state;
    }

    public void DeductMonthlyFee()
    {
        currentState.DeductMonthlyFee();
    }

    public void ConnectToNetwork()
    {
        currentState.ConnectToNetwork();
    }
}

class Program
{
    static void Main()
    {
        InternetSubscriber subscriber = new InternetSubscriber(50, 30, 100);

        subscriber.ConnectToNetwork();
        subscriber.DeductMonthlyFee();

        
        subscriber.Balance = 20;

        subscriber.Balance = 40;

        
        subscriber.DeductMonthlyFee();
        subscriber.SetState(new NormalState(subscriber));
        subscriber.ConnectToNetwork();
    }
}

/*У цьому коді я використав паттерн "Стан" 
Кожен стан (NormalState і ArrearsState) реалізує інтерфейс IInternetState,
і контекст (InternetSubscriber) має можливість змінювати свій стан за допомогою методу SetState(). 
Кожен стан виконує свої конкретні дії при списанні абонентської плати та підключенні до мережі.
*/