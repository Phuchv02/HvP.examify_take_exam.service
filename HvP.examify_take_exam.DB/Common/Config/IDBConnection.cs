using Microsoft.EntityFrameworkCore;
using System.Data;
using HvP.examify_take_exam.DB.Common.Models;

namespace HvP.DB.Common.Config
{
    public interface IDBConnection
    {
        string GetConnectionString();
        IDbConnection GetConnection();
        void SetOptionBuilder(ref DbContextOptionsBuilder optionsBuilder);
        DBConfigModel GetDBConfig();

        //EnumDBProvider GetDBProvider();

        //IDbCommand AddParam(IDbCommand command, string key, object value);

        //IDbCommand AddParam(IDbCommand command, string key, object value, string type = "");
    }
}
