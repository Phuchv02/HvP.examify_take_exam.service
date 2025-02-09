using Sprache;

namespace HvP.examify_take_exam.DB.Common.Models
{
    public class RabbitMqConfigModel
    {
        public RabbitMqConfigModel(string Address, int Port, string Username, string Password)
        {
            this.Address = Address;
            this.Port = Port;
            this.Username = Username;
            this.Password = Password;
        }

        public string Address { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string GetHostName()
        {
            return $"{Address}:{Port}";
        }
    }
}
