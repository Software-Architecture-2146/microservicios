using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Company.Migrations
{
    /// <inheritdoc />
    public partial class Baseline_ExistingSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Vacío a propósito: la BD ya está en línea con el modelo.
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Vacío a propósito
        }

    }
}
