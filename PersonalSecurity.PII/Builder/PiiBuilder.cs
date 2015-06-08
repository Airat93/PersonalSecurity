namespace PersonalSecurity.PII.Builder
{
    using System.IO;
    using System.Text;
    using PersonalSecurity.PII.PiiTemplates;
    using YamlDotNet.Serialization;

    public class PiiBuilder : IPiiBuilder
    {      
        public string BuildPii(PiiTemplate pii)
        {
            var serializer = new Serializer();
            var sb = new StringBuilder();

            serializer.Serialize(new StringWriter(sb), pii, pii.GetType());
            return sb.ToString();
        }
    }
}
