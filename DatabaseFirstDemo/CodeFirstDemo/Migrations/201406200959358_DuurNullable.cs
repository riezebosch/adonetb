namespace CodeFirstDemo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DuurNullable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbl_Demo", "Duur", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbl_Demo", "Duur", c => c.Int(nullable: false));
        }
    }
}
