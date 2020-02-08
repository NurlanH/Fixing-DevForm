namespace DevFormAz.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class somechange : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserDetails", "UserId", "dbo.Users");
            DropIndex("dbo.UserDetails", new[] { "UserId" });
            AddColumn("dbo.Users", "UserDetailId", c => c.Int(nullable: false));
            CreateIndex("dbo.Users", "UserDetailId");
            AddForeignKey("dbo.Users", "UserDetailId", "dbo.UserDetails", "Id", cascadeDelete: true);
            DropColumn("dbo.UserDetails", "UserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserDetails", "UserId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Users", "UserDetailId", "dbo.UserDetails");
            DropIndex("dbo.Users", new[] { "UserDetailId" });
            DropColumn("dbo.Users", "UserDetailId");
            CreateIndex("dbo.UserDetails", "UserId");
            AddForeignKey("dbo.UserDetails", "UserId", "dbo.Users", "Id", cascadeDelete: true);
        }
    }
}
