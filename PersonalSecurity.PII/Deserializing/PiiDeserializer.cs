namespace PersonalSecurity.PII.Deserializing
{
    using System.IO;
    using PersonalSecurity.PII.PiiTemplates;
    using YamlDotNet.Serialization;

    public class PiiDeserializer : IPiiDeserializer
    {
        public T Deserialize<T>(string content) where T : PiiTemplate
        {
            var deserializer = new Deserializer();
            return deserializer.Deserialize<T>(new StringReader(content));
        }
    }
}
