using Klick.SimpleUserDatabase.Foundation.Model;
using System.Collections.Generic;

namespace Klick.SimpleUserDatabase.Models
{
    public class User : BaseModel<int>
    {
        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public Province Province { get; set; }

        public UserState State { get; set; }

        public ICollection<Medication> Medications { get; set; }
    }
}
