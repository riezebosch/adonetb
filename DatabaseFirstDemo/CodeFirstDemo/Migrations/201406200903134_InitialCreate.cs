namespace CodeFirstDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbl_Demo",
                c => new
                    {
                        DemoIdentifier = c.Int(nullable: false, identity: true),
                        Cursus = c.String(nullable: false, maxLength: 40),
                        Omschrijving = c.String(maxLength: 250),
                        DummyData = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.DemoIdentifier)
                .Index(t => new { t.Omschrijving, t.DummyData }, unique: true, name: "OmschrijvingIndex");
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.tbl_Demo", "OmschrijvingIndex");
            DropTable("dbo.tbl_Demo");
        }
    }
}
