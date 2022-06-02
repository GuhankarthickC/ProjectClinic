using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Front_office_staff_Backend
{
    interface Users_BE_Interface
    {
        void ViewDoctors();
        void AddPatient();
        void ScheduleAppointment();
        void CancelAppointment();
        void Homepage();
        bool Login();
    }
}
