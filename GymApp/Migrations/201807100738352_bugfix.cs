namespace GymApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bugfix : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.GymClassApplicationUsers", newName: "ApplicationUserGymClasses");
            DropPrimaryKey("dbo.ApplicationUserGymClasses");
            AddPrimaryKey("dbo.ApplicationUserGymClasses", new[] { "ApplicationUser_Id", "GymClass_Id" });
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.ApplicationUserGymClasses");
            AddPrimaryKey("dbo.ApplicationUserGymClasses", new[] { "GymClass_Id", "ApplicationUser_Id" });
            RenameTable(name: "dbo.ApplicationUserGymClasses", newName: "GymClassApplicationUsers");
        }
    }
}
