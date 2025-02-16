using Microsoft.EntityFrameworkCore;
using System.Data;
using HvP.examify_take_exam.DB.Common.Models;

namespace HvP.DB.Common.Config
{
    public interface IDBConnection
    {
        string GetConnectionString();
        DBConfigModel GetDBConfig();
        void SetOptionBuilder(ref DbContextOptionsBuilder optionsBuilder);
        //IDbConnection GetConnection();

        //EnumDBProvider GetDBProvider();

        //IDbCommand AddParam(IDbCommand command, string key, object value);

        //IDbCommand AddParam(IDbCommand command, string key, object value, string type = "");
    }
}
