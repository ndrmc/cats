namespace Cats.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveAdminUnitNameAm : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AdminUnit", "NameAM");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AdminUnit", "NameAM", c => c.String(maxLength: 50));
        }
    }
}
