namespace VN.Example.Platform.Domain
{
    public interface IEntity<T>
        where T : struct
    {
        T Id { get; }
    }
}
