namespace WebAPI.Interfaces
{
    public interface IOneWayConverter<From, To>
    {
        To Convert(From from);
    }
}
