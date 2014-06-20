namespace CodeFirstDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DuurKolom : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbl_Demo", "Duur", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbl_Demo", "Duur");
        }
    }
}
