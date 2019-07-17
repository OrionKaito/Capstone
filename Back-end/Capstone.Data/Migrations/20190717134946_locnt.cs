using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Capstone.Data.Migrations
{
    public partial class locnt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "ActionTypes",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Data = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActionTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ConnectionTypes",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectionTypes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    EventID = table.Column<Guid>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: true, defaultValueSql: "CONVERT(date, GETDATE())"),
                    NotificationType = table.Column<int>(nullable: false),
                    IsHandled = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    EmailConfirmCode = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false, defaultValueSql: "CONVERT(date, GETDATE())"),
                    ManagerID = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermissionOfGroups",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    PermissionID = table.Column<Guid>(nullable: false),
                    GroupID = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermissionOfGroups", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PermissionOfGroups_Groups_GroupID",
                        column: x => x.GroupID,
                        principalTable: "Groups",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PermissionOfGroups_Permissions_PermissionID",
                        column: x => x.PermissionID,
                        principalTable: "Permissions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserGroups",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    UserID = table.Column<string>(nullable: true),
                    GroupID = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroups", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserGroups_Groups_GroupID",
                        column: x => x.GroupID,
                        principalTable: "Groups",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserGroups_Users_UserID",
                        column: x => x.UserID,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserNotifications",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    NotificationID = table.Column<Guid>(nullable: false),
                    UserID = table.Column<string>(nullable: true),
                    IsRead = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotifications", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserNotifications_Notifications_NotificationID",
                        column: x => x.NotificationID,
                        principalTable: "Notifications",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserNotifications_Users_UserID",
                        column: x => x.UserID,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    UserID = table.Column<string>(nullable: true),
                    RoleID = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserID",
                        column: x => x.UserID,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkFlowTemplates",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Data = table.Column<string>(nullable: true),
                    OwnerID = table.Column<string>(nullable: true),
                    PermissionToEditID = table.Column<Guid>(nullable: false),
                    PermissionToUseID = table.Column<Guid>(nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkFlowTemplates", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkFlowTemplates_Users_OwnerID",
                        column: x => x.OwnerID,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkFlowTemplates_Permissions_PermissionToEditID",
                        column: x => x.PermissionToEditID,
                        principalTable: "Permissions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkFlowTemplates_Permissions_PermissionToUseID",
                        column: x => x.PermissionToUseID,
                        principalTable: "Permissions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Requests",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: true, defaultValueSql: "CONVERT(date, GETDATE())"),
                    Description = table.Column<string>(nullable: true),
                    InitiatorID = table.Column<string>(nullable: true),
                    WorkFlowTemplateID = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requests", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Requests_Users_InitiatorID",
                        column: x => x.InitiatorID,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Requests_WorkFlowTemplates_WorkFlowTemplateID",
                        column: x => x.WorkFlowTemplateID,
                        principalTable: "WorkFlowTemplates",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkFlowTemplateActions",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsApprovedByLineManager = table.Column<bool>(nullable: false),
                    IsStart = table.Column<bool>(nullable: false),
                    IsEnd = table.Column<bool>(nullable: false),
                    WorkFlowTemplateID = table.Column<Guid>(nullable: false),
                    ActionTypeID = table.Column<Guid>(nullable: false),
                    PermissionToUseID = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkFlowTemplateActions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkFlowTemplateActions_ActionTypes_ActionTypeID",
                        column: x => x.ActionTypeID,
                        principalTable: "ActionTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkFlowTemplateActions_Permissions_PermissionToUseID",
                        column: x => x.PermissionToUseID,
                        principalTable: "Permissions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkFlowTemplateActions_WorkFlowTemplates_WorkFlowTemplateID",
                        column: x => x.WorkFlowTemplateID,
                        principalTable: "WorkFlowTemplates",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestActions",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false, defaultValueSql: "CONVERT(date, GETDATE())"),
                    RequestID = table.Column<Guid>(nullable: false),
                    ActorID = table.Column<string>(nullable: true),
                    NextStepID = table.Column<Guid>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestActions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RequestActions_Users_ActorID",
                        column: x => x.ActorID,
                        principalSchema: "dbo",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestActions_WorkFlowTemplateActions_NextStepID",
                        column: x => x.NextStepID,
                        principalTable: "WorkFlowTemplateActions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RequestActions_Requests_RequestID",
                        column: x => x.RequestID,
                        principalTable: "Requests",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkFlowTemplateActionConnections",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    FromWorkFlowTemplateActionID = table.Column<Guid>(nullable: false),
                    ToWorkFlowTemplateActionID = table.Column<Guid>(nullable: false),
                    ConnectionTypeID = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkFlowTemplateActionConnections", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkFlowTemplateActionConnections_ConnectionTypes_ConnectionTypeID",
                        column: x => x.ConnectionTypeID,
                        principalTable: "ConnectionTypes",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkFlowTemplateActionConnections_WorkFlowTemplateActions_FromWorkFlowTemplateActionID",
                        column: x => x.FromWorkFlowTemplateActionID,
                        principalTable: "WorkFlowTemplateActions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkFlowTemplateActionConnections_WorkFlowTemplateActions_ToWorkFlowTemplateActionID",
                        column: x => x.ToWorkFlowTemplateActionID,
                        principalTable: "WorkFlowTemplateActions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestFiles",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Path = table.Column<string>(nullable: true),
                    RequestActionID = table.Column<Guid>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestFiles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RequestFiles_RequestActions_RequestActionID",
                        column: x => x.RequestActionID,
                        principalTable: "RequestActions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RequestValues",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    RequestActionID = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestValues", x => x.ID);
                    table.ForeignKey(
                        name: "FK_RequestValues_RequestActions_RequestActionID",
                        column: x => x.RequestActionID,
                        principalTable: "RequestActions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PermissionOfGroups_GroupID",
                table: "PermissionOfGroups",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_PermissionOfGroups_PermissionID",
                table: "PermissionOfGroups",
                column: "PermissionID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestActions_ActorID",
                table: "RequestActions",
                column: "ActorID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestActions_NextStepID",
                table: "RequestActions",
                column: "NextStepID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestActions_RequestID",
                table: "RequestActions",
                column: "RequestID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestFiles_RequestActionID",
                table: "RequestFiles",
                column: "RequestActionID");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_InitiatorID",
                table: "Requests",
                column: "InitiatorID");

            migrationBuilder.CreateIndex(
                name: "IX_Requests_WorkFlowTemplateID",
                table: "Requests",
                column: "WorkFlowTemplateID");

            migrationBuilder.CreateIndex(
                name: "IX_RequestValues_RequestActionID",
                table: "RequestValues",
                column: "RequestActionID");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_GroupID",
                table: "UserGroups",
                column: "GroupID");

            migrationBuilder.CreateIndex(
                name: "IX_UserGroups_UserID",
                table: "UserGroups",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_NotificationID",
                table: "UserNotifications",
                column: "NotificationID");

            migrationBuilder.CreateIndex(
                name: "IX_UserNotifications_UserID",
                table: "UserNotifications",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleID",
                table: "UserRoles",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_UserID",
                table: "UserRoles",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowTemplateActionConnections_ConnectionTypeID",
                table: "WorkFlowTemplateActionConnections",
                column: "ConnectionTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowTemplateActionConnections_FromWorkFlowTemplateActionID",
                table: "WorkFlowTemplateActionConnections",
                column: "FromWorkFlowTemplateActionID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowTemplateActionConnections_ToWorkFlowTemplateActionID",
                table: "WorkFlowTemplateActionConnections",
                column: "ToWorkFlowTemplateActionID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowTemplateActions_ActionTypeID",
                table: "WorkFlowTemplateActions",
                column: "ActionTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowTemplateActions_PermissionToUseID",
                table: "WorkFlowTemplateActions",
                column: "PermissionToUseID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowTemplateActions_WorkFlowTemplateID",
                table: "WorkFlowTemplateActions",
                column: "WorkFlowTemplateID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowTemplates_OwnerID",
                table: "WorkFlowTemplates",
                column: "OwnerID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowTemplates_PermissionToEditID",
                table: "WorkFlowTemplates",
                column: "PermissionToEditID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowTemplates_PermissionToUseID",
                table: "WorkFlowTemplates",
                column: "PermissionToUseID");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                schema: "dbo",
                table: "Users",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                schema: "dbo",
                table: "Users",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PermissionOfGroups");

            migrationBuilder.DropTable(
                name: "RequestFiles");

            migrationBuilder.DropTable(
                name: "RequestValues");

            migrationBuilder.DropTable(
                name: "UserGroups");

            migrationBuilder.DropTable(
                name: "UserNotifications");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "WorkFlowTemplateActionConnections");

            migrationBuilder.DropTable(
                name: "RequestActions");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "ConnectionTypes");

            migrationBuilder.DropTable(
                name: "WorkFlowTemplateActions");

            migrationBuilder.DropTable(
                name: "Requests");

            migrationBuilder.DropTable(
                name: "ActionTypes");

            migrationBuilder.DropTable(
                name: "WorkFlowTemplates");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "Permissions");
        }
    }
}
