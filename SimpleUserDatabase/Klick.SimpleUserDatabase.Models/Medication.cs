using Klick.SimpleUserDatabase.Foundation.Model;

namespace Klick.SimpleUserDatabase.Models
{
    public class Medication : IModel<int>
    {
        public int ID { get; set; }

        public string Name { get; set; }
    }
}
