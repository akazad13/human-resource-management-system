﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRMS.Infrastructure.Migrations
{
    public partial class Update_Emplopyee_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Taxzone",
                table: "Employees",
                newName: "TaxZone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateofBirth",
                table: "Employees",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaxZone",
                table: "Employees",
                newName: "Taxzone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateofBirth",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
