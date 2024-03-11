using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace _1.Migrations
{
    /// <inheritdoc />
    public partial class newMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BlackListReasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ShortName = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false),
                    Descryption = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Department",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DepartmentName = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    idGroups = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idGroups);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MeetingStatus",
                columns: table => new
                {
                    idStatus = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    StatusName = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idStatus);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Subdepartment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    SubdepartmentName = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    idUser = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Email = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    Login = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    Password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    Salt = table.Column<string>(type: "varchar(225)", maxLength: 225, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idUser);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "VisitPurpose",
                columns: table => new
                {
                    idVisitPurpose = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idVisitPurpose);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    idEmployees = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    FullName = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    Department = table.Column<int>(type: "int", nullable: true),
                    Subdepartment = table.Column<int>(type: "int", nullable: true),
                    Code = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idEmployees);
                    table.ForeignKey(
                        name: "FK_EmployeesDepartment",
                        column: x => x.Department,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeesSubdepartment",
                        column: x => x.Subdepartment,
                        principalTable: "Subdepartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Guests",
                columns: table => new
                {
                    idGuests = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    LastName = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    Name = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    Patronymic = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    Phone = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    Email = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    Organization = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: true),
                    Note = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    Birthday = table.Column<DateTime>(type: "date", nullable: false),
                    PasssportSeries = table.Column<int>(type: "int", nullable: false),
                    PassportNumber = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: true),
                    Avatar = table.Column<byte[]>(type: "longblob", nullable: true),
                    Passport = table.Column<byte[]>(type: "longblob", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.idGuests);
                    table.ForeignKey(
                        name: "FK_PGUserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "idUser",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GroupMeeting",
                columns: table => new
                {
                    GroupMeetingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateFrom = table.Column<DateTime>(type: "date", nullable: false),
                    DateTo = table.Column<DateTime>(type: "date", nullable: false),
                    DateVisit = table.Column<DateTime>(type: "date", nullable: true),
                    Time = table.Column<TimeSpan>(type: "time", nullable: true),
                    DeprtmentId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    VisitPurposeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.GroupMeetingId);
                    table.ForeignKey(
                        name: "FK_GMDepartment",
                        column: x => x.DeprtmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GMEmployee",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "idEmployees");
                    table.ForeignKey(
                        name: "FK_GMGroup",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "idGroups");
                    table.ForeignKey(
                        name: "FK_GMStatus",
                        column: x => x.StatusId,
                        principalTable: "MeetingStatus",
                        principalColumn: "idStatus");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PrivateMeeting",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    DateFrom = table.Column<DateTime>(type: "date", nullable: false),
                    DateTo = table.Column<DateTime>(type: "date", nullable: false),
                    DateVisit = table.Column<DateTime>(type: "date", nullable: true),
                    Time = table.Column<TimeSpan>(type: "time", nullable: true),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: false),
                    VisitPurposeId = table.Column<int>(type: "int", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PMDepartment",
                        column: x => x.DepartmentId,
                        principalTable: "Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrivateMEmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "idEmployees",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PrivateStatus",
                        column: x => x.StatusId,
                        principalTable: "MeetingStatus",
                        principalColumn: "idStatus");
                    table.ForeignKey(
                        name: "FK_VisitPurposeId",
                        column: x => x.VisitPurposeId,
                        principalTable: "VisitPurpose",
                        principalColumn: "idVisitPurpose",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BlackListGuests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    GuestId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BlackListGuests_GuestId",
                        column: x => x.GuestId,
                        principalTable: "Guests",
                        principalColumn: "idGuests",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "GroupMeetingsGuest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    GuestId = table.Column<int>(type: "int", nullable: false),
                    GroupMeetingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GMGuests",
                        column: x => x.GuestId,
                        principalTable: "Guests",
                        principalColumn: "idGuests");
                    table.ForeignKey(
                        name: "FK_UGMK",
                        column: x => x.GroupMeetingId,
                        principalTable: "GroupMeeting",
                        principalColumn: "GroupMeetingId");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "PrivateMeetingsGuests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    GuestId = table.Column<int>(type: "int", nullable: false),
                    PrivateMeetingId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuestsPrivate",
                        column: x => x.GuestId,
                        principalTable: "Guests",
                        principalColumn: "idGuests",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UPM",
                        column: x => x.PrivateMeetingId,
                        principalTable: "PrivateMeeting",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "FK_BlackListGuests_GuestId_idx",
                table: "BlackListGuests",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "FK_EmployeesDepartment_idx",
                table: "Employees",
                column: "Department");

            migrationBuilder.CreateIndex(
                name: "FK_EmployeesSubdepartment_idx",
                table: "Employees",
                column: "Subdepartment");

            migrationBuilder.CreateIndex(
                name: "FK_DepartentGM_idx",
                table: "GroupMeeting",
                column: "DeprtmentId");

            migrationBuilder.CreateIndex(
                name: "FK_GMEmployee_idx",
                table: "GroupMeeting",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "FK_GMGroup_idx",
                table: "GroupMeeting",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "FK_GroupStatus_idx",
                table: "GroupMeeting",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "FK_GMGuests_idx",
                table: "GroupMeetingsGuest",
                column: "GuestId");

            migrationBuilder.CreateIndex(
                name: "FK_UGMK_idx",
                table: "GroupMeetingsGuest",
                column: "GroupMeetingId");

            migrationBuilder.CreateIndex(
                name: "FK_PGUserId_idx",
                table: "Guests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "FK_PMDeprtment_idx",
                table: "PrivateMeeting",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "FK_PrivateMeetingStatus_idx",
                table: "PrivateMeeting",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "FK_PrivateMEmployeeId_idx",
                table: "PrivateMeeting",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "FK_VisitPurposeId_idx",
                table: "PrivateMeeting",
                column: "VisitPurposeId");

            migrationBuilder.CreateIndex(
                name: "FK__idx",
                table: "PrivateMeetingsGuests",
                column: "PrivateMeetingId");

            migrationBuilder.CreateIndex(
                name: "fk_User_idx",
                table: "PrivateMeetingsGuests",
                column: "GuestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlackListGuests");

            migrationBuilder.DropTable(
                name: "BlackListReasons");

            migrationBuilder.DropTable(
                name: "GroupMeetingsGuest");

            migrationBuilder.DropTable(
                name: "PrivateMeetingsGuests");

            migrationBuilder.DropTable(
                name: "GroupMeeting");

            migrationBuilder.DropTable(
                name: "Guests");

            migrationBuilder.DropTable(
                name: "PrivateMeeting");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "MeetingStatus");

            migrationBuilder.DropTable(
                name: "VisitPurpose");

            migrationBuilder.DropTable(
                name: "Department");

            migrationBuilder.DropTable(
                name: "Subdepartment");
        }
    }
}
