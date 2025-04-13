using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HvP.examify_take_exam.DB.Migrations;
using HvP.examify_take_exam.Common.Constants.Errors;
using HvP.examify_take_exam.Common.Exceptions;
using HvP.examify_take_exam.Common.Extentions;
using HvP.examify_take_exam.Common.Models;
using HvP.examify_take_exam.Common.Helpers;
using HvP.Database.DBContexts;

namespace HvP.examify_take_exam.Controllers
{
    [ApiController]
    [Route("api/v1/database-setting")]
    public class DatabaseSettingController : ControllerBase
    {
        private ILogger _logger;
        private CommonDBContext _dbContext;
        public DatabaseSettingController(ILogger<DatabaseSettingController> logger, CommonDBContext dbContext)
        {
            this._logger = logger;
            this._dbContext = dbContext;
        }

        /// <summary>
        /// Create Database
        /// </summary>
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateDatabase()
        {
            string funcName = nameof(CreateDatabase);
            this._logger.LogInformation(InforMsg.InfFuncStart(funcName).GetLogStr());

            try
            {
                bool result = await this._dbContext.Database.EnsureCreatedAsync();

                if (result)
                {
                    this._logger.LogInformation("Database Created Success");
                    return this.ResponseSuccess("Database Created");
                }

                this._logger.LogInformation("Create Database Failure");
                return this.ResponseSuccess("Failure");
            }
            catch (Exception ex)
            {
                throw new BaseException(ErrorMsg.ErrQueryDatabase, ex.Message);
            }
        }

        /// <summary>
        /// Delete Database
        /// </summary>
        [HttpPost]
        [Route("delete")]
        public async Task<IActionResult> DeleteDatabase()
        {
            string funcName = nameof(DeleteDatabase);
            this._logger.LogInformation(InforMsg.InfFuncStart(funcName).GetLogStr());

            try
            {
                bool result = await this._dbContext.Database.EnsureDeletedAsync();

                if (result)
                {
                    this._logger.LogInformation("Database Deleted Success");
                    return this.ResponseSuccess("Database Deleted");
                }

                this._logger.LogInformation("Delete Database Failure");
                return this.ResponseSuccess("Failure");
            }
            catch (Exception ex)
            {
                throw new BaseException(ErrorMsg.ErrQueryDatabase, ex.Message);
            }
        }

        /// <summary>
        /// Download Backup Database
        /// </summary>
        /// <returns>File</returns>
        [HttpPost]
        [Route("backup")]
        public async Task<IActionResult> BackupDatabase()
        {
            string funcName = nameof(BackupDatabase);
            this._logger.LogInformation(InforMsg.InfFuncStart(funcName).GetLogStr());

            DBConfigModel dbConfig = this._dbContext.dbconnection.GetDBConfig();

            // TODO: using dbdump download file sql backup <dbname>_<timestamp>

            return this.ResponseSuccess("Failure");
        }

        /// <summary>
        /// Generate migration changed
        /// </summary>
        /// <param name="migrationName"></param>
        [HttpPost]
        [Route("migration/generate")]
        public async Task<IActionResult> GenerateMigration([FromQuery] string migrationName)
        {
            string funcName = nameof(GenerateMigration);
            this._logger.LogInformation(InforMsg.InfFuncStart(funcName).GetLogStr());

            try
            {
                if (string.IsNullOrEmpty(migrationName))
                {
                    throw new BadRequestException("Migration name cannot be empty");
                }

                var cmd = MigrationSetting.GetCMDGenerateMigration(migrationName);
                var workingDirectory = MigrationSetting.GetDBWorkingDirectory();
                var rs = await CommandHelper.RunCommand(workingDirectory, cmd);

                return this.ResponseSuccess(rs);
            }
            catch (Exception ex)
            {
                throw new BaseException(ErrorMsg.ErrMigration(funcName), ex.Message);
            }
        }

        [HttpPost]
        [Route("migration/apply-all")]
        public async Task<IActionResult> ApplyAllMigration()
        {
            string funcName = nameof(ApplyAllMigration);
            this._logger.LogInformation(InforMsg.InfFuncStart(funcName).GetLogStr());

            try
            {
                var cmd = MigrationSetting.GetCMDApplyMigration();
                var workingDirectory = MigrationSetting.GetDBWorkingDirectory();
                var rs = await CommandHelper.RunCommand(workingDirectory, cmd);

                return this.ResponseSuccess(rs);
            }
            catch (Exception ex)
            {
                throw new BaseException(ErrorMsg.ErrMigration(funcName), ex.Message);
            }
        }

        /// <summary>
        /// Apply migration
        /// </summary>
        /// <param name="MigrationName"></param>
        [HttpPost]
        [Route("migration/apply")]
        public async Task<IActionResult> ApplyMigration([FromQuery] string migrationName)
        {
            string funcName = nameof(ApplyMigration);
            this._logger.LogInformation(InforMsg.InfFuncStart(funcName).GetLogStr());

            try
            {
                if (string.IsNullOrEmpty(migrationName))
                {
                    throw new BadRequestException("Migration name cannot be empty");
                }

                var listMigrateApplied = await this._dbContext.Database.GetAppliedMigrationsAsync();

                if (listMigrateApplied.Contains(migrationName))
                {
                    throw new BadRequestException(ErrorMsg.ErrMigrationHasBeenApplied(funcName, migrationName));
                }

                var cmd = MigrationSetting.GetCMDApplyMigration(migrationName);
                var workingDirectory = MigrationSetting.GetDBWorkingDirectory();
                var rs = await CommandHelper.RunCommand(workingDirectory, cmd);

                return this.ResponseSuccess(rs);
            }
            catch (Exception ex)
            {
                throw new BaseException(ErrorMsg.ErrMigration(funcName, migrationName), ex.Message);
            }
        }

        /// <summary>
        /// Revert Migration
        /// </summary>
        /// <param name="MigrationName"></param>
        [HttpPost]
        [Route("migration/revert")]
        public async Task<IActionResult> RevertMigration([FromQuery] string? migrationName)
        {
            string funcName = nameof(RevertMigration);
            this._logger.LogInformation(InforMsg.InfFuncStart(funcName).GetLogStr());

            try
            {
                if (string.IsNullOrEmpty(migrationName))
                {
                    throw new BadRequestException("Migration name cannot be empty");
                }

                var listMigrateApplied = await this._dbContext.Database.GetAppliedMigrationsAsync();

                if (!listMigrateApplied.Contains(migrationName))
                {
                    throw new BadRequestException(ErrorMsg.ErrMigrationHasNotBeenApplied(funcName, migrationName));
                }

                var cmd = MigrationSetting.GetCMDApplyMigration(migrationName);
                var workingDirectory = MigrationSetting.GetDBWorkingDirectory();
                var rs = await CommandHelper.RunCommand(workingDirectory, cmd);

                return this.ResponseSuccess(rs);
            }
            catch (Exception ex)
            {
                throw new BaseException(ErrorMsg.ErrMigration(funcName, migrationName), ex.Message);
            }
        }

        /// <summary>
        /// Get List Migration History
        /// </summary>
        [HttpGet]
        [Route("migration/history")]
        public async Task<IActionResult> GetListMigrationName()
        {

            string funcName = nameof(GetListMigrationName);
            this._logger.LogInformation(InforMsg.InfFuncStart(funcName).GetLogStr());
            try
            {
                var cmd = MigrationSetting.GetCMDGetListMigration();
                var workingDirectory = MigrationSetting.GetDBWorkingDirectory();
                var rs = await CommandHelper.RunCommand(workingDirectory, cmd);

                return this.ResponseSuccess(rs);
            }
            catch (Exception ex)
            {
                throw new BaseException(ErrorMsg.ErrMigration(funcName), ex.Message);
            }
        }

    }
}
