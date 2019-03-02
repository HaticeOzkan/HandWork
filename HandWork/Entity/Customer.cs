using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity
{

    public enum Gender
    {
        Male,
        Female
    }

    public class Customer : IdentityUser
    {
        //useridentityden gelen email telno password id
        public int TC { get; set; }
        public string NameSurname { get; set; }
        public Gender Gender { get; set; }
        public string Adress { get; set; }
        public virtual ShoppingCart ShoppingCart { get; set; }
        public virtual List<Order> Orders { get; set; }
        public int Age { get; set; }
        public DateTime RegisteryDate { get; set; }
        public bool HasPhoto { get; set; }
        public string ProfilPhoto { get; set; }
        public List<Product> Products { get; set; }
        public int Complaint { get; set; }
        public Customer()
        {
            RegisteryDate = DateTime.Today;
            HasPhoto = false;
        }

    }
    public class Employee : Customer
    {
        public string SendToEmail { get; set; }
    }
}
