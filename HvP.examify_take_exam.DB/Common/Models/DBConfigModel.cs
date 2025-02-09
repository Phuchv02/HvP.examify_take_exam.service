using Sprache;

namespace HvP.examify_take_exam.DB.Common.Models
{
    public class DBConfigModel
    {
        public DBConfigModel(string Address, int Port, string Username, string Password, string DatabaseName)
        {
            this.Address = Address;
            this.Port = Port;
            this.Username = Username;
            this.Password = Password;
            this.DatabaseName = DatabaseName;
        }

        public string Address { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }

        public string GetHostName()
        {
            return $"{Address}:{Port}:{DatabaseName}";
        }
    }
}
