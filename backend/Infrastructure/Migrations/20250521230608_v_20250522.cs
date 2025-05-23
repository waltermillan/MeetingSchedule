using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class v_20250522 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                table: "tags",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                table: "contacts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "user_id",
                table: "contact_tags",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_tags_user_id",
                table: "tags",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_contacts_user_id",
                table: "contacts",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_contact_tags_user_id",
                table: "contact_tags",
                column: "user_id");

            migrationBuilder.AddForeignKey(
                name: "FK_contact_tags_users_user_id",
                table: "contact_tags",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_contacts_users_user_id",
                table: "contacts",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_tags_users_user_id",
                table: "tags",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_contact_tags_users_user_id",
                table: "contact_tags");

            migrationBuilder.DropForeignKey(
                name: "FK_contacts_users_user_id",
                table: "contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_tags_users_user_id",
                table: "tags");

            migrationBuilder.DropIndex(
                name: "IX_tags_user_id",
                table: "tags");

            migrationBuilder.DropIndex(
                name: "IX_contacts_user_id",
                table: "contacts");

            migrationBuilder.DropIndex(
                name: "IX_contact_tags_user_id",
                table: "contact_tags");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "tags");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "contacts");

            migrationBuilder.DropColumn(
                name: "user_id",
                table: "contact_tags");
        }
    }
}
