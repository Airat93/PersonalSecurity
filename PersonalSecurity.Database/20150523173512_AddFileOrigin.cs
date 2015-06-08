namespace PersonalSecurity.Database
{
    using FluentMigrator;

    [Migration(20150523173512)]
    public class AddFileOrigin : Migration
    {
        public override void Up()
        {
            Create
                .Column("Origin")
                .OnTable("File")
                .AsString(50)
                .NotNullable()
                .WithDefaultValue("Unknown");
        }

        public override void Down()
        {
        }
    }
}
