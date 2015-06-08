namespace PersonalSecurity.DataAccess
{
    using PersonalSecurity.DataAccess.Domain;

    public interface IFileInfoRepository
    {
        void Save(FileInfo entity);

        FileInfo[] GetByType(string fileType);

        FileInfo GetByName(string name);
    }
}
