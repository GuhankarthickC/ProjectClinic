using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Front_office_staff_Backend
{
    public class Patient
    {
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string sex { get; set; }
        public int age { get; set; }
        public string dateofbirth { get; set; }
        public Patient()
        {

        }
        public Patient(string firstname,string lastname,string sex,int age,string dateofbirth)
        {
            this.firstname = firstname;
            this.lastname = lastname;
            this.sex = sex;
            this.age = age;
            this.dateofbirth = dateofbirth;
        }
    }
}
