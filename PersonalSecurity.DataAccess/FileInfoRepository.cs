namespace PersonalSecurity.DataAccess
{
    using System.Linq;
    using Dapper;
    using PersonalSecurity.DataAccess.Domain;

    public class FileInfoRepository : RepositoryBase, IFileInfoRepository
    {
        public FileInfoRepository(string connectionString)
            : base(connectionString)
        {
        }

        public void Save(FileInfo entity)
        {
            WithConnection(
                connection =>
                    {
                        entity.Id =
                            connection.ExecuteScalar<int>(@"insert into [File] ([Name], [FileType], [CloudType], [CreatedAt], [FileForm]) 
values (@Name, @FileType, @CloudTypeCode, GETDATE(), @FileForm)
select SCOPE_IDENTITY()", entity);
                    });
        }

        public FileInfo[] GetByType(string fileType)
        {
            return
                WithConnection(
                    connection =>
                    connection.Query<FileInfo>(
                        @"select * from [File] where FileType = @FileType order by CreatedAt desc",
                        new { FileType = fileType }).ToArray());
        }

        public FileInfo GetByName(string name)
        {
            return WithConnection(connection => connection.Query<FileInfo>(
@"select * from [File] where Name = @Name", new { Name = name }).FirstOrDefault());
        }
    }
}
