namespace Blog.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _ini : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        MessageId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Messages = c.String(),
                    })
                .PrimaryKey(t => t.MessageId);
            
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostsId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                        Outline = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        Click = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PostsId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Replies",
                c => new
                    {
                        ReplyId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        ReplyContent = c.String(),
                        CreateDate = c.DateTime(nullable: false),
                        SecondReply = c.Int(nullable: false),
                        PostsId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ReplyId)
                .ForeignKey("dbo.Posts", t => t.PostsId, cascadeDelete: true)
                .Index(t => t.PostsId);
            
            CreateTable(
                "dbo.Tags",
                c => new
                    {
                        TagId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.TagId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        UserName = c.String(),
                        PassWord = c.String(),
                        Power = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId);
            
            CreateTable(
                "dbo.TagPosts",
                c => new
                    {
                        Tag_TagId = c.Int(nullable: false),
                        Posts_PostsId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Tag_TagId, t.Posts_PostsId })
                .ForeignKey("dbo.Tags", t => t.Tag_TagId, cascadeDelete: true)
                .ForeignKey("dbo.Posts", t => t.Posts_PostsId, cascadeDelete: true)
                .Index(t => t.Tag_TagId)
                .Index(t => t.Posts_PostsId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Posts", "UserId", "dbo.Users");
            DropForeignKey("dbo.TagPosts", "Posts_PostsId", "dbo.Posts");
            DropForeignKey("dbo.TagPosts", "Tag_TagId", "dbo.Tags");
            DropForeignKey("dbo.Replies", "PostsId", "dbo.Posts");
            DropIndex("dbo.TagPosts", new[] { "Posts_PostsId" });
            DropIndex("dbo.TagPosts", new[] { "Tag_TagId" });
            DropIndex("dbo.Replies", new[] { "PostsId" });
            DropIndex("dbo.Posts", new[] { "UserId" });
            DropTable("dbo.TagPosts");
            DropTable("dbo.Users");
            DropTable("dbo.Tags");
            DropTable("dbo.Replies");
            DropTable("dbo.Posts");
            DropTable("dbo.Messages");
        }
    }
}
