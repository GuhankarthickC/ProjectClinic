﻿using ConsoleTables;
using Front_office_staff_Backend;
using System;

namespace Users
{
    class MainProgram
    {
        static void Main(string[] args)
        {
            Users_BE u = new Users_BE();
            u.Login();
        }
    }
}
