using Microsoft.EntityFrameworkCore;
using HvP.examify_take_exam.Common.Models;

namespace HvP.examify_take_exam.Common.Config
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
