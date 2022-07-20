namespace WebAPI.Interfaces
{
    public interface IConverter<T1, T2>
    {
        T2 Convert(T1 item);
        T1 Convert(T2 item);
    }
}
