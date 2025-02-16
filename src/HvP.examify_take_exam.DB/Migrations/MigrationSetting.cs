namespace HvP.examify_take_exam.DB.Migrations
{
    public static class MigrationSetting
    {
        private static string projectStartup = "HvP.examify_take_exam";
        //* script install dotnet-ef: dotnet tool install --global dotnet-ef
        //* script update version dotnet-ef: dotnet tool update --global dotnet-ef --version 9.0.1

        /// <summary>
        /// Get Database project working directory
        /// </summary>
        /// <returns>Database Directory</returns>
        public static string GetDBWorkingDirectory()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            return currentDirectory + ".DB";
        }

        // CMD: Generate Migration
        public static string GetCMDGenerateMigration(string migrationName)
        {
            string migrationFolder = "Migrations";
            return $"dotnet ef migrations add {migrationName} --context CommonDbContext --startup-project ../{projectStartup} --project . --output-dir {migrationFolder} --no-build";
        }

        // CMD: Apply Migration
        public static string GetCMDApplyMigration(string? migrationName = null)
        {
            if (!string.IsNullOrEmpty(migrationName))
            {
                return $"dotnet ef database update {migrationName} --context CommonDbContext --startup-project ../{projectStartup} --project . --no-build";
            }
            return $"dotnet ef database update --context CommonDbContext --startup-project ../{projectStartup} --project . --no-build";
        }

        // CMD: Remove Last Migration
        public static string GetCMDRemoveMigration()
        {
            return $"dotnet ef migrations remove --context CommonDbContext --startup-project ../{projectStartup} --project . --no-build";
        }

        // CMD: Generate sql script
        public static string GetCMDGenerateDDLScript()
        {
            return $"dotnet ef migrations script --context CommonDbContext --startup-project ../{projectStartup} --project . --no-build";
        }

        // CMD: Get historical of Migration
        public static string GetCMDGetListMigration()
        {
            return $"dotnet ef migrations list --context CommonDbContext --startup-project ../{projectStartup} --project . --no-build";
        }

    }
}
