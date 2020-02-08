namespace DevFormAz.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class somechange2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "UserDetailId", "dbo.UserDetails");
            DropIndex("dbo.Users", new[] { "UserDetailId" });
            AlterColumn("dbo.Users", "UserDetailId", c => c.Int());
            CreateIndex("dbo.Users", "UserDetailId");
            AddForeignKey("dbo.Users", "UserDetailId", "dbo.UserDetails", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "UserDetailId", "dbo.UserDetails");
            DropIndex("dbo.Users", new[] { "UserDetailId" });
            AlterColumn("dbo.Users", "UserDetailId", c => c.Int(nullable: false));
            CreateIndex("dbo.Users", "UserDetailId");
            AddForeignKey("dbo.Users", "UserDetailId", "dbo.UserDetails", "Id", cascadeDelete: true);
        }
    }
}
