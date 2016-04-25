namespace NoDb
{
    public interface IStringSerializer<T> where T : class
    {
        string Serialize(T obj);
        T Deserialize(string serializedObject, string key = "");
        string ExpectedFileExtension { get; }
    }
}
