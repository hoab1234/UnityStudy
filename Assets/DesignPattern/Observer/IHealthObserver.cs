public interface IHealthObserver
{
    void OnHealthChanged(float currentHealth);
}

public interface ISubject
{
    void RegisterObserver(IHealthObserver observer);
    void RemoveObserver(IHealthObserver observer);
    void NotifyObservers();
}