Create database Clinic_Management_System_DB;

use Clinic_Management_System_DB;

--1 Users table

Create table Users (UserName varchar(20) Unique,FirstName varchar(20),LastName varchar(20),Password Varchar(20));

ALTER TABLE Users ADD CONSTRAINT ck_No_Special_Char CHECK (UserName NOT LIKE '%[^A-Z0-9]%') 

Insert into Users Values('Admin','Nakul','Varma','Admin');
Insert into Users Values('GkShukla','Gokul','Shukla','Shukla@123');
Insert into Users Values('Nandy','Nandhiya','Rupesh','Nrp@1997');
Insert into Users Values('Bojan','Bhubesh','Janveer','BhuJan@098');

--2 Doctors table with constraints

create table Doctors(Doctor_Id int identity(1,1),FirstName varchar(20) CHECK (FirstName NOT LIKE '%[^A-Z0-9]%') ,LastName varchar(20) CHECK (LastName NOT LIKE '%[^A-Z0-9]%') ,Sex varchar(10),Specialization varchar(50),Visiting_Hours varchar(30)); 

alter table Doctors add Availablefrom varchar(10),Availabletill varchar(10);

insert into Doctors values('Radha','Krishnan','M','General','3 Hr','12:00','15:00');
insert into Doctors values('Shamugha','Vadivel','M','General','2 Hr','14:00','16:00');
insert into Doctors values('Mahesh','Kannan','M','Internal Medicine','1 Hr','14:00','15:00');
insert into Doctors values('Monika','Vasudevan','F','Orthopedics','2 Hr','18:00','20:00');
insert into Doctors values('Nithya','Ramesh','F','Orthopedics','1 Hr','16:00','17:00');
insert into Doctors values('Sanchitha','Singh','F','Pediatrics','3 Hr','18:00','21:00');
insert into Doctors values('Mukhil','Varma','M','Ophthalmology','1 Hr','13:00','14:00');
insert into Doctors values('Rachel','Varghis','F','Ophthalmology','3 Hr','13:00','16:00');
insert into Doctors values('Rache@l','Varghis','F','Ophthalmology','3 Hr','13:00','16:00');

--3 AddPatient table with Constraints

Create table Addpatient (Patient_ID int identity(100,1),FirstName varchar(20) CHECK (FirstName NOT LIKE '%[^A-Z0-9]%'),LastName varchar(20) CHECK (LastName NOT LIKE '%[^A-Z0-9]%'),Sex varchar(10),Age int check(Age between 0 and 120),Date_of_Birth date);

--4 AppointmentDetails

create table AppointmentDetails(AppointmentID int identity(10,1),PatientID int,Doctor_Name varchar(20),Specialization varchar(20),Visiting_Date varchar(20),Appointment_from varchar(20),Appointment_till varchar(20));

select * from AppointmentDetails where PatientID=100 and Visiting_Date='03/06/2022';



