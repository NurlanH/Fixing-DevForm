namespace DevFormAz.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v11 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Comments", "UserDetailId", "dbo.UserDetails");
            DropForeignKey("dbo.Forms", "User_Id", "dbo.Users");
            DropIndex("dbo.Comments", new[] { "UserDetailId" });
            DropIndex("dbo.Forms", new[] { "User_Id" });
            RenameColumn(table: "dbo.Forms", name: "User_Id", newName: "UserId");
            AddColumn("dbo.Comments", "UserId", c => c.Int(nullable: false));
            AlterColumn("dbo.Forms", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Comments", "UserId");
            CreateIndex("dbo.Forms", "UserId");
            AddForeignKey("dbo.Comments", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Forms", "UserId", "dbo.Users", "Id", cascadeDelete: true);
            DropColumn("dbo.Comments", "UserDetailId");
            DropColumn("dbo.Forms", "UserDetailId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Forms", "UserDetailId", c => c.Int(nullable: false));
            AddColumn("dbo.Comments", "UserDetailId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Forms", "UserId", "dbo.Users");
            DropForeignKey("dbo.Comments", "UserId", "dbo.Users");
            DropIndex("dbo.Forms", new[] { "UserId" });
            DropIndex("dbo.Comments", new[] { "UserId" });
            AlterColumn("dbo.Forms", "UserId", c => c.Int());
            DropColumn("dbo.Comments", "UserId");
            RenameColumn(table: "dbo.Forms", name: "UserId", newName: "User_Id");
            CreateIndex("dbo.Forms", "User_Id");
            CreateIndex("dbo.Comments", "UserDetailId");
            AddForeignKey("dbo.Forms", "User_Id", "dbo.Users", "Id");
            AddForeignKey("dbo.Comments", "UserDetailId", "dbo.UserDetails", "Id", cascadeDelete: true);
        }
    }
}
