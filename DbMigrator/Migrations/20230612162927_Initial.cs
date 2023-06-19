using System.Data;
using FluentMigrator;

namespace DbMigrator.Migrations;

[Migration(20230612162927)]
public class Initial : Migration
{
	public override void Up()
	{
		Create.Table("test_table")
			.WithColumn("id").AsInt32().Identity().PrimaryKey()
			.WithColumn("article_id").AsGuid().NotNullable()
			.WithColumn("file_name").AsString(255);
	}

	public override void Down()
	{
	}
}

