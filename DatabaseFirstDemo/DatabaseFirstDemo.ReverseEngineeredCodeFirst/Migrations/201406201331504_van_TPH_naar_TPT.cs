namespace DatabaseFirstDemo.ReverseEngineeredCodeFirst.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class van_TPH_naar_TPT : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Instructor",
                c => new
                    {
                        PersonID = c.Int(nullable: false),
                        HireDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.PersonID)
                .ForeignKey("dbo.Person", t => t.PersonID)
                .Index(t => t.PersonID);

            Sql("INSERT INTO Instructor (PersonID, HireDate) SELECT PersonID, HireDate FROM Person WHERE HireDate IS NOT NULL");

            CreateTable(
                "dbo.Student",
                c => new
                    {
                        PersonID = c.Int(nullable: false),
                        EnrollmentDate = c.DateTime(),
                    })
                .PrimaryKey(t => t.PersonID)
                .ForeignKey("dbo.Person", t => t.PersonID)
                .Index(t => t.PersonID);

            Sql("INSERT INTO Student (PersonID, EnrollmentDate) SELECT PersonID, EnrollmentDate FROM Person WHERE EnrollmentDate IS NOT NULL");

            DropColumn("dbo.Person", "HireDate");
            DropColumn("dbo.Person", "EnrollmentDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Person", "EnrollmentDate", c => c.DateTime());
            AddColumn("dbo.Person", "HireDate", c => c.DateTime());

            //Sql("UPDATE Person SET (EnrollmentDate) VALUES (EnrollmentDate) FROM Person WHERE EnrollmentDate <> NULL");
            //Sql("UPDATE Person SET (EnrollmentDate) VALUES (EnrollmentDate) FROM Person WHERE EnrollmentDate <> NULL");

            DropForeignKey("dbo.Student", "PersonID", "dbo.Person");
            DropForeignKey("dbo.Instructor", "PersonID", "dbo.Person");
            DropIndex("dbo.Student", new[] { "PersonID" });
            DropIndex("dbo.Instructor", new[] { "PersonID" });
            DropTable("dbo.Student");
            DropTable("dbo.Instructor");
        }
    }
}
