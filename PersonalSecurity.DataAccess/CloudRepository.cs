namespace PersonalSecurity.DataAccess
{
    using System.Linq;
    using Dapper;
    using PersonalSecurity.DataAccess.Domain;

    public class CloudRepository : RepositoryBase, ICloudRepository
    {
        public CloudRepository(string connectionString)
            : base(connectionString)
        {
        }

        public void Save(Cloud cloud)
        {
            WithConnection(
                connection =>
                    {
                        cloud.Id =
                            connection.ExecuteScalar<int>(
@"insert into [dbo].[Cloud] ([AccessToken], [CloudType], [CreatedAt]) values (@AccessToken, @CloudType, GETDATE())

SELECT SCOPE_IDENTITY()", new { AccessToken = cloud.AccessToken, CloudType = (int)cloud.CloudType });
                    });
        }

        public Cloud[] GetAll()
        {
            return WithConnection(connection => connection.Query<Cloud>(@"select * from [Cloud]").ToArray());
        }

        public Cloud GetByType(CloudType cloudType)
        {
            return
                WithConnection(
                    connection =>
                    connection.Query<Cloud>(
                        @"select * from [Cloud] where CloudType = @CloudType",
                        new { CloudType = cloudType }).FirstOrDefault());
        }
    }
}
