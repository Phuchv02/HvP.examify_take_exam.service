namespace HvP.examify_take_exam.DB.Constants.Errors
{
    // # COMMON ERRORS
    public static partial class ErrorMsg
    {
        public static ErrorMsgModel ErrQueryDatabase = new()
        {
            ErrorCode = "ERR_QUERY_DB",
            Message = "Query database failure"
        };

        // ## Error Migration
        public static ErrorMsgModel ErrMigration(string type, string? migrationName = null) => new()
        {
            ErrorCode = "ERR_MIGRATION_DB",
            Message = "Query migration database failure",
            LogMessage = $"Query migration database failure, type = {type}, migrationName = {migrationName}"
        };

        public static ErrorMsgModel ErrMigrationHasBeenApplied(string type, string? migrationName = null) => new()
        {
            ErrorCode = "ERR_MIGRATION_DB",
            Message = "Migration has been applied",
            LogMessage = $"Migration has been applied, type = {type}, migrationName = {migrationName}"
        };

        public static ErrorMsgModel ErrMigrationHasNotBeenApplied(string type, string? migrationName = null) => new()
        {
            ErrorCode = "ERR_MIGRATION_DB",
            Message = "Migration has not been applied yet",
            LogMessage = $"Migration has not been applied yet, type = {type}, migrationName = {migrationName}"
        };

        public static ErrorMsgModel ErrGetEnvConfig(string configKey) => new()
        {
            ErrorCode = "ERR_GET_ENV_CONFIG",
            Message = $"Get config from ENV error",
            LogMessage = $"Get config from ENV error, ConfigKey: {configKey}"
        };
    }
}
