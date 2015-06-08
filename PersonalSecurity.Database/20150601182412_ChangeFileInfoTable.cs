namespace PersonalSecurity.Database
{
    using FluentMigrator;

    [Migration(20150601182412)]
    public class ChangeFileInfoTable : Migration
    {
        public override void Up()
        {
            Rename
                .Column("Hash")
                .OnTable("File")
                .To("Name");

            Create
                .Column("FileForm")
                .OnTable("File")
                .AsString()
                .Nullable();
        }

        public override void Down()
        {
        }
    }
}
