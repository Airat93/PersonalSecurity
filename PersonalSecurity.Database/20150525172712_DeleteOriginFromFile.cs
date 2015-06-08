namespace PersonalSecurity.Database
{
    using FluentMigrator;

    [Migration(20150525172712)]
    public class DeleteOriginFromFile : Migration
    {
        public override void Up()
        {
            Delete
                .Column("Origin")
                .FromTable("File");
        }

        public override void Down()
        {
        }
    }
}
