using Klick.SimpleUserDatabase.Foundation.Model;

namespace Klick.SimpleUserDatabase.Models
{
    public class Province : IModel<int>
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string Code { get; set; }

        public string CountryCode { get; set; }
    }
}
