using Front_office_staff_Backend;
using System;
using System.Data.SqlClient;

namespace Front_Office_Staff_DAL
{
    public class Users_DAL
    {
        public static SqlConnection con;
        public static SqlCommand cmd;
        public static SqlDataReader dr;
        string output;
        public SqlDataReader SelectLoginData(string uname)
        {
            try
            {
                con = getcon();
                cmd = new SqlCommand("User_Pwd_Check", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@uname", uname);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dr;
        }
        public string Appointmentdata(Appointment d)
        {

            try
            {
                con = getcon();
                cmd = new SqlCommand("insert into AppointmentDetails values (@pid,@doc_name,@specialization,@visiting,@from,@to)");
                cmd.Parameters.AddWithValue("@pid", d.patientid);
                cmd.Parameters.AddWithValue("@doc_name", d.Doctor_Name);
                cmd.Parameters.AddWithValue("@specialization", d.specialization);
                cmd.Parameters.AddWithValue("@visiting", d.appointment_date);
                cmd.Parameters.AddWithValue("@from", d.from_time);
                cmd.Parameters.AddWithValue("@to", d.to_time);
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                output = "Appointment Booked Successfully for Patient ID - " + d.patientid;
            }
            catch (Exception e)
            {
                output = "Error : Unable to book appointment";
            }
            
            return output;
        }
        public string Appointmentupdatedata(string doc_name,string visiting,string availabletill)
        {
            try
            {
                con = getcon();
                cmd = new SqlCommand("Updateappointment", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                /*cmd = new SqlCommand("update Doctors set Visiting_Hours=@visiting,Availablefrom=@availablefrom,Availabletill=@availabletill where FirstName=@doc_name");*/
                cmd.Parameters.AddWithValue("@visiting", visiting);
                cmd.Parameters.AddWithValue("@availabletill", availabletill);
                cmd.Parameters.AddWithValue("@doc_name", doc_name);
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return "Appointment Booked Successfully";
        }
        public SqlDataReader SelectAppointmentData(int patientid,string visiting_date)
        {
            try
            {
                con = getcon();
                cmd = new SqlCommand("appointmentdata", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@pid", patientid);
                cmd.Parameters.AddWithValue("@visit", visiting_date);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dr;
        }
        public string CancelAppointment(Appointment a)
        {

            try
            {
                con = getcon();
                cmd = new SqlCommand("appointmentdelete", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@aid", a.appointment_no);
                cmd.Parameters.AddWithValue("@visit", a.appointment_date);
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                output= "Appointment Cancelled Successfully for PatientID - " + a.patientid;
            }
            catch (Exception e)
            {
                output="Error: Unable to cancel your appointment, please try later!";
            }
            
            return output;
        }
        private static SqlConnection getcon()
        {
            con = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Clinic_Management_System_DB;Integrated Security=true");
            con.Open();
            return con;
        }
        public SqlDataReader SelectDoctorData()
        {
            try
            {
                con = getcon();
                cmd = new SqlCommand("Doctors_proc", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Connection = con;
                dr = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dr;
        }
        public SqlDataReader SelectDoctor(string doc_name)
        {
            try
            {
                con = getcon();
                cmd = new SqlCommand("doctorselection", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@doc_name", doc_name);
                cmd.Connection = con;
                dr = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return dr;
        }
        public string InsertPatientData(Patient p)
        {
            try
            {
                con = getcon();
                cmd = new SqlCommand("addpatient_proc", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@fname", p.firstname);
                cmd.Parameters.AddWithValue("@lname", p.lastname);
                cmd.Parameters.AddWithValue("@sex", p.sex);
                cmd.Parameters.AddWithValue("@age", p.age);
                cmd.Parameters.AddWithValue("@dob", p.dateofbirth);
                cmd.ExecuteNonQuery();
                output = "Patient " + p.firstname + " " + p.lastname + " Added Successfully...";
            }
            catch (Exception e)
            {
                output="Error : Unable to add patient data, please try later";

            }
             return output;
        }
    }
}
