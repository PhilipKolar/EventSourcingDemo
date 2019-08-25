using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EventSourcingDemo.Data.Domain;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EventSourcingDemo.Seeding
{
    public static class InitialSetup
    {
        public static void Apply(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData("OrderStatuses", new string[] {"Id", "Name"}, new object[] { 1, "Requested" });
            migrationBuilder.InsertData("OrderStatuses", new string[] { "Id", "Name" }, new object[] { 2, "Completed" });
            migrationBuilder.InsertData("OrderStatuses", new string[] { "Id", "Name" }, new object[] { 3, "Cancelled" });

            migrationBuilder.InsertData("Products", new string[] { "Id", "DateCreated", "Name", "Price" },
                new object[] { 1, DateTimeOffset.UtcNow, "Playstation 4", 350M });
            migrationBuilder.InsertData("Products", new string[] { "Id", "DateCreated", "Name", "Price" },
                new object[] { 2, DateTimeOffset.UtcNow, "Playstation 4 Pro", 500M });
            migrationBuilder.InsertData("Products", new string[] { "Id", "DateCreated", "Name", "Price" },
                new object[] { 3, DateTimeOffset.UtcNow, "Xbox One", 300M });

            migrationBuilder.InsertData("Addons", new string[] { "Id", "DateCreated", "Name", "Price" },
                new object[] { 1, DateTimeOffset.UtcNow, "Dualshock 4", 50M });
            migrationBuilder.InsertData("Addons", new string[] { "Id", "DateCreated", "Name", "Price" },
                new object[] { 2, DateTimeOffset.UtcNow, "Xbox One Controller", 40M });
            migrationBuilder.InsertData("Addons", new string[] { "Id", "DateCreated", "Name", "Price" },
                new object[] { 3, DateTimeOffset.UtcNow, "Playstation VR", 350M });
        }
    }
}
