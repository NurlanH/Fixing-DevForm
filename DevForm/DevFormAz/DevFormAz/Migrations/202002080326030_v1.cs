namespace DevFormAz.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Forms", "UserDetailId", "dbo.UserDetails");
            DropIndex("dbo.Forms", new[] { "UserDetailId" });
            AddColumn("dbo.Forms", "User_Id", c => c.Int());
            CreateIndex("dbo.Forms", "User_Id");
            AddForeignKey("dbo.Forms", "User_Id", "dbo.Users", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Forms", "User_Id", "dbo.Users");
            DropIndex("dbo.Forms", new[] { "User_Id" });
            DropColumn("dbo.Forms", "User_Id");
            CreateIndex("dbo.Forms", "UserDetailId");
            AddForeignKey("dbo.Forms", "UserDetailId", "dbo.UserDetails", "Id", cascadeDelete: true);
        }
    }
}
