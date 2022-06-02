using ConsoleTables;
using Front_Office_Staff_DAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace Front_office_staff_Backend
{
    public class Users_BE
    {
        public class Login_Error : ApplicationException
        {
            public Login_Error (string msg) : base(msg)
            {

            }
        }
        public class Valid_response : ApplicationException
        {
            public Valid_response (string msg) : base(msg)
            {

            }
        }
        public class Format : ApplicationException
        {
            public Format (string msg) : base(msg)
            {

            }
        }
        Users_DAL ud = new Users_DAL();
     
        //Added enum for selecting respective specialization from the categories
        enum categories{
            General=1, 
            Internal_Medicine, 
            Pediatrics, 
            Orthopedics, 
            Ophthalmology
        }
        public bool Login()
        {
            bool flag = false;
            string username = "",password="";
            Console.WriteLine("===================================================================================================================");
            Console.WriteLine(String.Format("{0," + Console.WindowWidth / 2 + "}", "Login"));
            Console.WriteLine("===================================================================================================================");
            try
            {
                Console.WriteLine("\n\t\t\tUserName : ");
                Console.SetCursorPosition(36, 4);
                username = Console.ReadLine();
                if (username.Length > 10)
                {
                    throw new Login_Error("UserName length should be 10 or less");
                }
                Console.WriteLine("\n\t\t\tPassword : ");
                Console.SetCursorPosition(36, 6);
                password = Console.ReadLine();
                if (!password.Contains('@'))
                {
                    throw new Login_Error("Password should contain '@' as part of it");
                }
                else
                {
                    SqlDataReader d = ud.SelectLoginData(username);
                    while (d.Read())
                    {
                        //SQL Reader takes the values using their respective columns names from database for verification
                        if ((d["UserName"].Equals(username)) && (d["Password"].Equals(password)))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("\n\t\t\tLogin Successfull....");
                            Console.ForegroundColor = ConsoleColor.White;
                            d.Close();
                            Thread.Sleep(1000);
                            Console.Clear();
                            Homepage();
                            flag= true;
                            break;
                        }
                        else
                        {
                            throw new Login_Error("You’ve entered an incorrect user name or password");
                           
                        }
                    }
                    
                }
            }//User Defined Exception 
            catch(Login_Error e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\t\t\tError: "+e.Message);
                Thread.Sleep(2500);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
                flag = false;
                Login();
            }
            return flag;
        }
        public void Homepage()
        {
            int choice;
            Console.WriteLine("===================================================================================================================");
            Console.WriteLine(String.Format("{0," + Console.WindowWidth / 2 + "}", "Home"));
            Console.WriteLine("===================================================================================================================");
            Console.WriteLine("\n\t\tPlease select your choice from the menu to proceed further : ");
            Console.WriteLine("\n\t\t1.View Doctors\n\t\t2.Add Patient\n\t\t3.Schedule Appointment\n\t\t4.Cancel Appointment\n\t\t5.Logout");
            Console.WriteLine("\n\n\t\tYour Choice: "); 
            try
            {
                Console.SetCursorPosition(30, 13);
                choice= Convert.ToInt32(Console.ReadLine());
                //Using Regex to get choice within the range (1-5)
                if (!Regex.IsMatch(choice.ToString(), "^[1-5]*$"))
                {
                    throw new Format("Choice should only contain numbers (1-5)");
                }
                switch (choice)
                {
                    case 1:
                        ViewDoctors();
                        break;
                    case 2:
                        AddPatient();
                        break;
                    case 3:
                        ScheduleAppointment();
                        break;
                    case 4:
                        CancelAppointment();
                        break;
                    case 5:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n\t\tLogging Off....");
                        Console.ForegroundColor = ConsoleColor.White;
                        for (var i = 0; i <= 100; ++i)
                        {
                            progressbar(i, true);
                            Thread.Sleep(30);
                        }
                        Console.Clear();
                        Login();
                        break;
                    default:
                        Console.WriteLine("\n\t\tPlease Enter a valid option to proceed");
                        Homepage();
                        break;
                }
            }//User Defined exception
            catch(Format e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\t\tError: " + e.Message);
                Thread.Sleep(2500);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
                Homepage();
            }
            catch(Exception a)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\t\tError: " + a.Message);
                Thread.Sleep(2500);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
                Homepage();
            }

        }
        private void CancelAppointment()
        {
            Console.Clear();
            int choice,change,patientID,i=0;string date = "",output="",confirm="",visiting="",availabletill="",doc_name="";string[] a;
            List<Appointment> lappoint = new List<Appointment>();
            Appointment appoint;
            Console.WriteLine("===================================================================================================================");
            Console.WriteLine(String.Format("{0," + Console.WindowWidth / 2 + "}", "Appointment Cancellation"));
            Console.WriteLine("===================================================================================================================");
            try
            {
                Console.WriteLine("\n\tEnter Patient ID :");
                Console.SetCursorPosition(52, 4);
                patientID = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("\n\tAppointment Date (DD/MM/YYYY) :");
                Console.SetCursorPosition(52, 6);
                date = Console.ReadLine();
                SqlDataReader dr = ud.SelectAppointmentData(patientID, date);
                if (!(dr.HasRows))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\t\tNo Appointment Available for the PatientID : {0} on {1}", patientID, date);
                    Thread.Sleep(2500);
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Clear();
                    Homepage();
                }
                else
                {
                    var table = new ConsoleTable(new ConsoleTableOptions
                    {
                        EnableCount = false,
                        Columns = new[] { "Appointment_NO", dr.GetName(2), dr.GetName(3), dr.GetName(4), dr.GetName(5), dr.GetName(6) }
                    });
                    while (dr.Read())
                    {
                        table.AddRow(++i, dr.GetValue(2), dr.GetValue(3), dr.GetValue(4), dr.GetValue(5),
                                 dr.GetValue(6));
                        a = dr.GetValue(2).ToString().Split(" ");
                        doc_name = a[0];
                        availabletill = dr.GetValue(6).ToString();
                        //Creating a appointment object
                        appoint = new Appointment(Convert.ToInt32(dr.GetValue(0)), Convert.ToInt32(dr.GetValue(1)), doc_name, dr.GetValue(3).ToString(), dr.GetValue(4).ToString(), dr.GetValue(5).ToString(), dr.GetValue(6).ToString());
                        //Adding the object to the appointment list incase more appointments are available for particular user
                        lappoint.Add(appoint);
                    }
                    Console.WriteLine("\n");
                    //Table that contains patient appointment details
                    table.Write();
                    dr.Close();
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\n\t\tPress k to Cancel\t\t\tPress O to Back");
                    confirm = Console.ReadLine();
                    if (confirm.ToLower().Equals("k"))
                    {
                        if (lappoint.Count > 1)
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("\n\tSelect the Appointment to Cancel - ");
                            Console.SetCursorPosition(52, 21);
                            choice = Convert.ToInt32(Console.ReadLine());
                            if (!(choice < lappoint.Count))
                            {
                                throw new Format("Choice should only contain numbers within " + lappoint.Count);
                            }
                            //Selecting the required appointment from the appointment table using object from appointment list
                            //Used LINQ 'ElementAt' to get value of the index from list
                            output = ud.CancelAppointment(lappoint.ElementAt(choice - 1));
                            SqlDataReader dd = ud.SelectDoctor(lappoint.ElementAt(choice - 1).Doctor_Name);
                            while (dd.Read())
                            {
                                visiting = dd.GetValue(5).ToString();
                            }
                            dd.Close();
                            change = Convert.ToInt32(visiting.Substring(0, 1));
                            visiting = visiting.Replace(visiting.Substring(0, 1), (change += 1).ToString());
                            //Updating the doctors table after appointment cancellation
                            _ = ud.Appointmentupdatedata(lappoint.ElementAt(choice - 1).Doctor_Name, visiting, availabletill);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n\t\tCancelling....");
                            for (var j = 0; j <= 100; ++j)
                            {
                                progressbar(j, true);
                                Thread.Sleep(30);
                            }
                            Console.WriteLine("\n\t\t{0}", output);
                            Console.ForegroundColor = ConsoleColor.White;
                            Thread.Sleep(2000);
                            Console.Clear();
                            Homepage();
                        }
                        else
                        {
                            //If the list contains only one appointment 
                            output = ud.CancelAppointment(lappoint.ElementAt(0));
                            SqlDataReader dd = ud.SelectDoctor(lappoint.ElementAt(0).Doctor_Name);
                            while (dd.Read())
                            {
                                visiting = dd.GetValue(5).ToString();
                            }
                            dd.Close();
                            change = Convert.ToInt32(visiting.Substring(0, 1));
                            visiting = visiting.Replace(visiting.Substring(0, 1), (change += 1).ToString());
                            //Updating the doctor's table with appointment cancellation
                            _ = ud.Appointmentupdatedata(lappoint.ElementAt(0).Doctor_Name, visiting, availabletill);
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("\n\t\tCancelling the appointment....");
                            for (var j = 0; j <= 100; ++j)
                            {
                                progressbar(j, true);
                                Thread.Sleep(30);
                            }
                            Console.WriteLine("\n\t\t{0}", output);
                            Console.ForegroundColor = ConsoleColor.White;
                            Thread.Sleep(2000);
                            Console.Clear();
                            Homepage();
                        }
                    }
                    else
                    {
                        Console.Clear();
                        Console.ForegroundColor = ConsoleColor.White;
                        Homepage();

                    }
                }
            }
            catch (Format f)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\t\tError: " + f.Message);
                Thread.Sleep(2500);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
                CancelAppointment();
            }
            catch (Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\t\tError : " + e.Message);
                Thread.Sleep(2000);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
                CancelAppointment();

            }
        }

        private void ScheduleAppointment()
        {
            Console.Clear();
            categories cat;
            SqlDataReader dr,dd;
            Appointment appoint;
            int patientID,special,change,id =0;string  output,doc_name,specialization, date,availablefrom="",availabletill="",visiting="",to_time="",Doctor_Name="",choice="";
            Console.WriteLine("===================================================================================================================");
            Console.WriteLine(String.Format("{0," + Console.WindowWidth / 2 + "}", "Appointment Booking"));
            Console.WriteLine("===================================================================================================================");
            try
            {
                Console.WriteLine("\n\tEnter Patient ID :");
                Console.SetCursorPosition(52, 4);
                patientID = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("\n\t(Choices – 1.General, 2.Internal Medicine, 3.Pediatrics, 4.Orthopedics, 5.Ophthalmology) ");
                Console.WriteLine("\n\tEnter Specialization Required : ");
                Console.SetCursorPosition(52, 8);
                special = Convert.ToInt32(Console.ReadLine());
                cat = (categories)special;
                specialization = cat.ToString();
                if (!Regex.IsMatch(special.ToString(), "^[1-5]*$"))
                {
                    throw new Format("Choice should only contain numbers (1-5)");
                }
                if (!((specialization.Equals("General")) || (specialization.Equals("Internal_Medicine")) || (specialization.Equals("Pediatrics")) || (specialization.Equals("Orthopedics")) || specialization.Equals("Ophthalmology")))
                {
                    throw new Valid_response("Enter correct specialization");
                }
                //Getting Available order under given specialization
                dr = ud.SelectDoctorData();
                Console.WriteLine("\n\tDoctors Available Under Specialization ({0}) ", specialization);
                while (dr.Read())
                {
                    if (dr.GetValue(4).Equals(specialization))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("\n\t{0}. Dr. " + dr.GetValue(1) + " " + dr.GetValue(2), ++id);
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                Console.WriteLine("\n\tEnter the name of the doctor to confirm the appointment with : ");
                Console.SetCursorPosition(72, 16);
                doc_name = Console.ReadLine();
                //Replacing the first letter to uppercase in-case small letter was given
                doc_name = doc_name.Replace(doc_name.Substring(0, 1), doc_name.Substring(0, 1).ToUpper());
                Console.WriteLine("\n\tEnter Visit Date (DD/MM/YYYY) :");
                Console.SetCursorPosition(52, 18);
                date = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\tTimeslot Available For Appoinment Booking : ");
                dr.Close();
                //Getting timeslot from the selected order for appointment booking
                dd = ud.SelectDoctorData();
                var table = new ConsoleTable(new ConsoleTableOptions
                {
                    EnableCount = false,
                    Columns = new[] { dd.GetName(1), dd.GetName(2), dd.GetName(4), dd.GetName(5), dd.GetName(6), dd.GetName(7), "Appointment_Date" }
                });
                while (dd.Read())
                {
                    if (dd.GetValue(1).Equals(doc_name))
                    {
                        table.AddRow(dd.GetValue(1), dd.GetValue(2), dd.GetValue(4), dd.GetValue(5), dd.GetValue(6),
                              dd.GetValue(7), date);
                        Doctor_Name = dd.GetValue(1).ToString() + " " + dd.GetValue(2).ToString();
                        visiting = dd.GetValue(5).ToString();
                        availablefrom = dd.GetValue(6).ToString();
                        to_time = dd.GetValue(7).ToString();
                    }
                }
                change = Convert.ToInt32(visiting.Substring(0, 1));
                change -= 1;
                visiting = visiting.Replace(visiting.Substring(0, 1), change.ToString());
                change = Convert.ToInt32(to_time.Substring(0, 2));
                availabletill = to_time.Replace(to_time.Substring(0, 2), (change -= 1).ToString());
                table.Write();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\t\t\tPress k to Book\t\t\tPress X to Cancel");
                choice = Console.ReadLine();
                if (choice.ToLower().Equals("k"))
                {
                    //Updating the new appointment available time
                    _ = ud.Appointmentupdatedata(doc_name, visiting, availabletill);
                    appoint = new Appointment(patientID, specialization, Doctor_Name, date, availabletill, to_time);
                    //Inserting the data into the appointment table
                    output = ud.Appointmentdata(appoint);
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\n\t\t\tBooking....");
                    for (var i = 0; i <= 100; ++i)
                    {
                        progressbar(i, true);
                        Thread.Sleep(30);
                    }
                    Console.WriteLine("\n\t\t\t{0}", output);
                    Console.ForegroundColor = ConsoleColor.White;
                    dd.Close();
                    Thread.Sleep(2000);
                    Console.Clear();
                    Homepage();
                }
                else
                {
                    Console.WriteLine("\n\t\t\tCancelling....");
                    for (var i = 0; i <= 100; ++i)
                    {
                        progressbar(i, true);
                        Thread.Sleep(30);
                    }
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.White;
                    Homepage();
                }
            }catch(Valid_response v)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\t\tError : " + v.Message);
                Thread.Sleep(1000);
                Console.ForegroundColor = ConsoleColor.White;
                ScheduleAppointment();

            }
            catch(Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\t\tError : " + e.Message);
                Thread.Sleep(1000);
                Console.ForegroundColor = ConsoleColor.White;
                ScheduleAppointment();
            }

        }

        private void AddPatient()
        {
            Console.Clear();
            string output="", reply = "";
            Patient p = new Patient();
            Console.WriteLine("===================================================================================================================");
            Console.WriteLine(String.Format("{0," + Console.WindowWidth / 2 + "}", "Add patient"));
            Console.WriteLine("===================================================================================================================");
            try
            {
                Console.WriteLine("\n\t\t\tEnter Patient First Name :");
                Console.SetCursorPosition(52, 4);
                p.firstname = Console.ReadLine();
                if (!Regex.IsMatch(p.firstname, "^[a-zA-Z0-9]*$"))
                {
                    throw new Format("FirstName should not contain special characters");
                }
                Console.WriteLine("\n\t\t\tEnter Patient Last Name :");
                Console.SetCursorPosition(52, 6);
                p.lastname = Console.ReadLine();
                if (!Regex.IsMatch(p.lastname, "^[a-zA-Z0-9]*$"))
                {
                    throw new Format("LastName should not contain special characters");
                }
                Console.WriteLine("\n\t\t\tPatient Sex (M/F/Others) :");
                Console.SetCursorPosition(52, 8);
                p.sex = Console.ReadLine();
                if (!((p.sex.Contains('M') || p.sex.Contains('F') || p.sex.Contains("Others"))))
                {
                    throw new Format("Enter Valid Input (M/F/Others)");
                }
                Console.WriteLine("\n\t\t\tEnter Patient Age :");
                Console.SetCursorPosition(52, 10);
                p.age = Convert.ToInt32(Console.ReadLine());
                if (!((p.age >= 0) && (p.age <= 120)))
                {
                    throw new Format("Enter Valid Age (0 to 120)");
                }
                Console.WriteLine("\n\t\t\tEnter Patient Date of Birth :");
                Console.SetCursorPosition(54, 12);
                p.dateofbirth = Console.ReadLine();
                output = ud.InsertPatientData(p);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n\t\t\t" + output);
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("\n\t\t\tPress X - Back to Main Menu\t\tO - Add a New Patient");
                reply = Console.ReadLine();
                if (reply.ToLower().Equals("x"))
                {
                    Console.Clear();
                    Homepage();
                }
                else if (reply.ToLower().Equals("o"))
                {
                    Console.Clear();
                    AddPatient();
                }
                else
                {
                    throw new Valid_response("Enter 'X' to go back (or) 'O' to add new patient");
                }
            }
            catch (Format e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\t\t\tError : " + e.Message);
                Thread.Sleep(1000);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
                AddPatient();
            }
            catch(Valid_response c)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\t\t\tError : " + c.Message);
                Thread.Sleep(1000);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
                AddPatient();
            }
            catch (Exception a)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\t\t\tError : " + a.Message);
                Thread.Sleep(1000);
                Console.ForegroundColor = ConsoleColor.White;
                Console.Clear();
                AddPatient();
            }
        }
        private void ViewDoctors()
        {
            Console.Clear();
            SqlDataReader dr = ud.SelectDoctorData();
            Console.WriteLine("===================================================================================================================");
            Console.WriteLine(String.Format("{0," + Console.WindowWidth / 2 + "}", "Doctor's Schedule"));
            Console.WriteLine("===================================================================================================================");
            for (var i = 0; i <= 100; ++i)
            {
                progressbar(i, true);
                Thread.Sleep(30);
            }
            Console.WriteLine("\n");
            var table = new ConsoleTable(new ConsoleTableOptions {EnableCount = false,Columns = new[] { dr.GetName(0), dr.GetName(1) ,
            dr.GetName(2),dr.GetName(3),dr.GetName(4),dr.GetName(5),dr.GetName(6),dr.GetName(7)} });
            while (dr.Read())
            {
              table.AddRow(dr.GetValue(0),dr.GetValue(1), dr.GetValue(2), dr.GetValue(3), dr.GetValue(4), dr.GetValue(5), dr.GetValue(6),
                        dr.GetValue(7));
            }
            table.Write();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\n\t\t\t\t\tPress X - Back to Main Menu");
            Console.ForegroundColor = ConsoleColor.White;
            string reply = Console.ReadLine();
            try
            {
                if (reply.ToLower().Equals("x"))
                {
                    {
                        Console.Clear();
                        Homepage();
                    }
                }
                else
                {
                    throw new Valid_response("Enter 'X' to go back");
                }
            } catch (Valid_response e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n\t\t\t\t\tError : "+e.Message);
                Thread.Sleep(1000);
                Console.ForegroundColor = ConsoleColor.White;
                ViewDoctors();
            }
        }
        public void progressbar(int progress = 0, bool update = false)
        {
            const string _twirl = "-\\|/";
            if (update)
                Console.Write("\b");
            Console.Write(_twirl[progress % _twirl.Length]);

        }

        public bool logintest(string uname,string pass)
        {
            bool flag = false;
            SqlDataReader d = ud.SelectLoginData(uname);
            
                if (uname.Length > 10)
                {
                    throw new Login_Error("UserName length should be 10 or less");
                }
                if (!pass.Contains('@'))
                {
                    throw new Login_Error("Password should contain '@' as part of it");
                }
                while (d.Read())
                {
                    if ((d["UserName"].Equals(uname)) && (d["Password"].Equals(pass)))
                    {
                        Console.WriteLine("\n\t\t\tLogin Successfull....");
                        d.Close();
                        flag = true;
                        break;
                    }
                    else
                    {
                        throw new Login_Error("You’ve entered an incorrect user name or password");

                    }
                }
            return flag;
        }

        public string addpatienttest(Patient r)
        {

            if (!Regex.IsMatch(r.firstname, "^[a-zA-Z0-9]*$"))
            {
                throw new Format("FirstName should not contain special characters");
            }
   
            if (!Regex.IsMatch(r.lastname, "^[a-zA-Z0-9]*$"))
            {
                throw new Format("LastName should not contain special characters");
            }

            if (!((r.sex.Contains('M') || r.sex.Contains('F') || r.sex.Contains("Others"))))
            {
                throw new Format("Enter Valid Input (M/F/Others)");
            }
   
            if (!((r.age >= 0) && (r.age <= 120)))
            {
                throw new Format("Enter Valid Age (0 to 120)");
            }
       
            string output = ud.InsertPatientData(r);
            return output;
        }  

     
    }
}
 
