using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessGym_Practice.Entities
{
    public enum TypeOfMembershipEnum
    {
        StudentMembership = 1,
        DayTimeMembership = 2,
        FullTimeMembership = 3

    }
    [Serializable]
    public class Membership : IComparable<Membership>
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public TypeOfMembershipEnum TypeOfMembership { get; set; }
        public long Price { get; set; }


        public Membership()
        {
            FirstName = "";
            LastName = "";
            TypeOfMembership = (TypeOfMembershipEnum)1;
            Price = 0;
        }

        public Membership(string firstName, string lastName, TypeOfMembershipEnum typeOfMembership, long price)
        {
            FirstName = firstName;
            LastName = lastName;
            TypeOfMembership = (TypeOfMembershipEnum)typeOfMembership;
            Price = price;
        }

        public Membership(long id, string firstName, string lastName, TypeOfMembershipEnum typeOfMembership, long price)
            : this(firstName, lastName, typeOfMembership, price)
        {
            Id = id;
        }
        public int CompareTo(Membership other)
        {
            if (this.Price.CompareTo(other.Price) == 0)
            {
                return this.LastName.CompareTo(other.LastName);
            }
            return this.Price.CompareTo(other.Price);
        }
    }

    public class MembershipPriceAscComparer : IComparer<Membership>
    {
        public int Compare(Membership x, Membership y)
        {
            return x.Price.CompareTo(y.Price);
        }
    }

    public class MembershipPriceDescComparer : IComparer<Membership>
    {
        public int Compare(Membership x, Membership y)
        {
            return -x.Price.CompareTo(y.Price);
        }
    }
}
