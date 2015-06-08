namespace PersonalSecurity.PII.Builder
{
    using PersonalSecurity.PII.PiiTemplates;

    public interface IPiiBuilder
    {
        string BuildPii(PiiTemplate pii);
    }
}
