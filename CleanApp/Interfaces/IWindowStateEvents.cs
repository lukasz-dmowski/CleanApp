namespace CleanApp.Interfaces
{
    public interface IWindowStateEvents
    {
        event Action RequestMinimize;
        event Action RequestMaximize;
        event Action RequestDrag;
        event Action RequestClose;
    }
}
