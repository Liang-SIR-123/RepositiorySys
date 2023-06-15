namespace Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _111 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.Categories", newName: "Category");
            RenameTable(name: "dbo.ConsumableInfoes", newName: "ConsumableInfo");
            RenameTable(name: "dbo.ConsumableRecords", newName: "ConsumableRecord");
            RenameTable(name: "dbo.DepartmentInfoes", newName: "DepartmentInfo");
            RenameTable(name: "dbo.FileInfoes", newName: "FileInfo");
            RenameTable(name: "dbo.MenuInfoes", newName: "MenuInfo");
            RenameTable(name: "dbo.RoleInfoes", newName: "RoleInfo");
            RenameTable(name: "dbo.UserInfoes", newName: "UserInfo");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.UserInfo", newName: "UserInfoes");
            RenameTable(name: "dbo.RoleInfo", newName: "RoleInfoes");
            RenameTable(name: "dbo.MenuInfo", newName: "MenuInfoes");
            RenameTable(name: "dbo.FileInfo", newName: "FileInfoes");
            RenameTable(name: "dbo.DepartmentInfo", newName: "DepartmentInfoes");
            RenameTable(name: "dbo.ConsumableRecord", newName: "ConsumableRecords");
            RenameTable(name: "dbo.ConsumableInfo", newName: "ConsumableInfoes");
            RenameTable(name: "dbo.Category", newName: "Categories");
        }
    }
}
