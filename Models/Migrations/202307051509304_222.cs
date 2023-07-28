namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _222 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MenuInfo", "Desrciption", c => c.String(maxLength: 32));
            DropColumn("dbo.MenuInfo", "Desciption");
        }
        
        public override void Down()
        {
            AddColumn("dbo.MenuInfo", "Desciption", c => c.String(maxLength: 32));
            DropColumn("dbo.MenuInfo", "Desrciption");
        }
    }
}
