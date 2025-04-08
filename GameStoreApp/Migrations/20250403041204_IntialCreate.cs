using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GameStoreApp.Migrations
{
    /// <inheritdoc />
    public partial class IntialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShippingAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BillingAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsShipped = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                });

            migrationBuilder.CreateTable(
                name: "Rewards",
                columns: table => new
                {
                    RewardId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rewards", x => x.RewardId);
                });

            migrationBuilder.CreateTable(
                name: "Shippings",
                columns: table => new
                {
                    ShippingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsShipped = table.Column<bool>(type: "bit", nullable: false),
                    ShippedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shippings", x => x.ShippingId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Users_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId");
                });

            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Genre = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Platform = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: true),
                    ShippingInfoShippingId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Game_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId");
                    table.ForeignKey(
                        name: "FK_Game_Shippings_ShippingInfoShippingId",
                        column: x => x.ShippingInfoShippingId,
                        principalTable: "Shippings",
                        principalColumn: "ShippingId");
                });

            migrationBuilder.CreateTable(
                name: "RewardsUser",
                columns: table => new
                {
                    RewardsRewardId = table.Column<int>(type: "int", nullable: false),
                    UsersUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RewardsUser", x => new { x.RewardsRewardId, x.UsersUserId });
                    table.ForeignKey(
                        name: "FK_RewardsUser_Rewards_RewardsRewardId",
                        column: x => x.RewardsRewardId,
                        principalTable: "Rewards",
                        principalColumn: "RewardId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RewardsUser_Users_UsersUserId",
                        column: x => x.UsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShippingInfoUser",
                columns: table => new
                {
                    ShippingInfosShippingId = table.Column<int>(type: "int", nullable: false),
                    UsersUserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShippingInfoUser", x => new { x.ShippingInfosShippingId, x.UsersUserId });
                    table.ForeignKey(
                        name: "FK_ShippingInfoUser_Shippings_ShippingInfosShippingId",
                        column: x => x.ShippingInfosShippingId,
                        principalTable: "Shippings",
                        principalColumn: "ShippingId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ShippingInfoUser_Users_UsersUserId",
                        column: x => x.UsersUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Game",
                columns: new[] { "Id", "Description", "Genre", "Name", "OrderId", "Platform", "Price", "ReleaseDate", "ShippingInfoShippingId", "Stock" },
                values: new object[,]
                {
                    { 1, "Zela 2D Adventure", "Action", "Zelda Minish Cap", null, "Gameboy Advance", 29.99m, new DateTime(2004, 11, 4, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 100 },
                    { 2, "Pokemon Catch Them ALL", "Adventure", "Pokemon Emerald", null, "Gameboy Advance", 49.99m, new DateTime(2002, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 50 }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "BillingAddress", "GameId", "IsShipped", "OrderDate", "PaymentMethod", "Quantity", "ShippingAddress", "Status", "TotalAmount", "UserId" },
                values: new object[,]
                {
                    { 1, null, 1, false, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, null, null, 0m, 1 },
                    { 2, null, 2, false, new DateTime(2025, 3, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 2, null, null, 0m, 2 }
                });

            migrationBuilder.InsertData(
                table: "Rewards",
                columns: new[] { "RewardId", "Description", "IsActive", "Name", "Points", "UserId" },
                values: new object[,]
                {
                    { 1, null, false, null, 100, 1 },
                    { 2, null, false, null, 200, 2 }
                });

            migrationBuilder.InsertData(
                table: "Shippings",
                columns: new[] { "ShippingId", "Address", "City", "Country", "IsShipped", "OrderId", "ShippedDate", "State", "ZipCode" },
                values: new object[,]
                {
                    { 1, "123 Main St", null, null, false, 1, new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null },
                    { 2, "456 Elm St", null, null, false, 2, new DateTime(2025, 3, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Address", "City", "Country", "DateOfBirth", "Email", "OrderId", "Password", "State", "UserName", "ZipCode" },
                values: new object[,]
                {
                    { 1, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kat@example.com", null, null, null, "Kat", null },
                    { 2, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nick@example.com", null, null, null, "Nick", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Game_OrderId",
                table: "Game",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Game_ShippingInfoShippingId",
                table: "Game",
                column: "ShippingInfoShippingId");

            migrationBuilder.CreateIndex(
                name: "IX_RewardsUser_UsersUserId",
                table: "RewardsUser",
                column: "UsersUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ShippingInfoUser_UsersUserId",
                table: "ShippingInfoUser",
                column: "UsersUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_OrderId",
                table: "Users",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Game");

            migrationBuilder.DropTable(
                name: "RewardsUser");

            migrationBuilder.DropTable(
                name: "ShippingInfoUser");

            migrationBuilder.DropTable(
                name: "Rewards");

            migrationBuilder.DropTable(
                name: "Shippings");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
