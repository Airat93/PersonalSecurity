namespace PersonalSecurity.Database
{
    using FluentMigrator;

    [Migration(20150523164612)]
    public class CreateFileInfo : Migration
    {
        public override void Up()
        {
            Create
                .Table("File")
                .WithColumn("Id").AsInt32().PrimaryKey().Identity()
                .WithColumn("Hash").AsString().NotNullable()
                .WithColumn("FileType").AsString().NotNullable()
                .WithColumn("CloudType").AsString().NotNullable();
        }

        public override void Down()
        {
        }
    }
}
