namespace PersonalSecurity.PII.Deserializing
{
    using PersonalSecurity.PII.PiiTemplates;

    public interface IPiiDeserializer
    {
        T Deserialize<T>(string content) where T : PiiTemplate;
    }
}
