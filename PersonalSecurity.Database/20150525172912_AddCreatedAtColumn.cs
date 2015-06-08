namespace PersonalSecurity.Database
{
    using FluentMigrator;

    [Migration(20150525172912)]
    public class AddCreatedAtColumn : Migration
    {
        public override void Up()
        {
            Create
                .Column("CreatedAt")
                .OnTable("Cloud")
                .AsDateTime()
                .NotNullable()
                .WithDefaultValue("GETDATE()");

            Create
                .Column("CreatedAt")
                .OnTable("File")
                .AsDateTime()
                .NotNullable()
                .WithDefaultValue("GETDATE()");
        }

        public override void Down()
        {
        }
    }
}
