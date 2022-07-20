namespace WebAPI.Interfaces
{
    public interface IIdentifiable<T>
    {
        T Id { get; }
    }
}
