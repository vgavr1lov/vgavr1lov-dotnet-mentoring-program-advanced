using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Catalog.Infrastructure.Migrations;

/// <inheritdoc />
public partial class OutboxMessageTableAddColumnsForRetry : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<DateTime>(
            name: "NextRetryOn",
            table: "OutboxMessage",
            type: "DATETIME2",
            nullable: false,
            defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

        migrationBuilder.AddColumn<int>(
            name: "RetryCount",
            table: "OutboxMessage",
            type: "INT",
            nullable: false,
            defaultValue: 0);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "NextRetryOn",
            table: "OutboxMessage");

        migrationBuilder.DropColumn(
            name: "RetryCount",
            table: "OutboxMessage");
    }
}
