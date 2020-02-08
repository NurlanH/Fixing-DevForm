namespace DevFormAz.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class added_new_table_tagformlist : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TagFormLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TagListsId = c.Int(nullable: false),
                        FormsId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Forms", "TagFormList_Id", c => c.Int());
            AddColumn("dbo.TagLists", "TagFormList_Id", c => c.Int());
            CreateIndex("dbo.Forms", "TagFormList_Id");
            CreateIndex("dbo.TagLists", "TagFormList_Id");
            AddForeignKey("dbo.Forms", "TagFormList_Id", "dbo.TagFormLists", "Id");
            AddForeignKey("dbo.TagLists", "TagFormList_Id", "dbo.TagFormLists", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TagLists", "TagFormList_Id", "dbo.TagFormLists");
            DropForeignKey("dbo.Forms", "TagFormList_Id", "dbo.TagFormLists");
            DropIndex("dbo.TagLists", new[] { "TagFormList_Id" });
            DropIndex("dbo.Forms", new[] { "TagFormList_Id" });
            DropColumn("dbo.TagLists", "TagFormList_Id");
            DropColumn("dbo.Forms", "TagFormList_Id");
            DropTable("dbo.TagFormLists");
        }
    }
}
