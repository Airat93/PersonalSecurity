namespace PersonalSecurity.Database
{
    using FluentMigrator;

    [Migration(20150521175510)]
    public class CreateCloudTable : Migration
    {
        public override void Up()
        {
            Create
                .Table("Cloud")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("CloudType").AsInt32()
                .WithColumn("AccessToken").AsString(100);
        }

        public override void Down()
        {
        }
    }
}
