namespace Rogue;
public interface IObserver
{
    void Update(ISubject subject);
}
public interface ISubject
{
    void Attach(IObserver observer);
    void Detach(IObserver observer);
    void Notify();
}
public class Subject : ISubject
{
    private List<IObserver> _observers = new List<IObserver>();

    public void Attach(IObserver observer)
    {
        this._observers.Add(observer);
    }

    public void Detach(IObserver observer)
    {
        this._observers.Remove(observer);
    }
    public void Notify()
    {
        foreach (var observer in _observers)
        {
            observer.Update(this);
        }
    }

}