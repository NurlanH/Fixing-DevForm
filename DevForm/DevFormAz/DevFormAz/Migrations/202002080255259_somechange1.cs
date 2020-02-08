namespace DevFormAz.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class somechange1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UserDetails", "Role", c => c.String());
            DropColumn("dbo.Users", "Role");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "Role", c => c.String());
            DropColumn("dbo.UserDetails", "Role");
        }
    }
}
