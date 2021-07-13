using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessGym_Practice.Entities
{
    public class FitnessGym
    {
        public string Name { get; set; }
        public List<Membership> Memberships { get; set; }

        public FitnessGym()
        {
            Name = "";
            Memberships = new List<Membership>();
        }

        public FitnessGym(string name)
            : this()
        {
            Name = name;
        }
        public FitnessGym(string name, List<Membership> memberships)
        {
            Name = name;
            Memberships = memberships;
        }
    }
}
