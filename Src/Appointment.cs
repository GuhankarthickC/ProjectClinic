using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Front_office_staff_Backend
{
    public class Appointment
    {
        public int appointment_no { get; set; }
        public int patientid { get; set; }
        public string specialization { get; set; }
        public string Doctor_Name { get; set; }
        public string appointment_date { get; set; }
        public string from_time { get; set; }
        public string to_time { get; set; }
        public Appointment()
        {

        }
        public Appointment(int patientid,string specialization,string Doctor_Name,string appointment_date,string from_time,string to_time)
        {
            this.patientid = patientid;
            this.specialization = specialization;
            this.Doctor_Name = Doctor_Name;
            this.appointment_date = appointment_date;
            this.from_time = from_time;
            this.to_time = to_time;
        }
        public Appointment(int appointment_no,int patientid,string Doctor_Name, string specialization, string appointment_date, string from_time, string to_time)
        {
            this.appointment_no=appointment_no;
            this.patientid = patientid;
            this.Doctor_Name = Doctor_Name;
            this.specialization = specialization;
            this.appointment_date = appointment_date;
            this.from_time = from_time;
            this.to_time = to_time;
        }
    }
}
