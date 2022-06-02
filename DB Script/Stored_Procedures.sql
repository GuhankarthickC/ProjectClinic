--1
create proc User_Pwd_Check (@uname varchar(20))
as
begin
select * from Users where UserName=@uname
end

--2
Create proc Doctors_proc
as 
begin
select * from Doctors
end

exec Doctors_proc

--3
create proc addpatient_proc (@fname varchar(20),@lname varchar(20),@sex varchar(10),@age int,@dob varchar(20))
as
begin
insert into Addpatient values(@fname,@lname,@sex,@age,@dob)
end

--4
alter proc Updateappointment (@doc_name varchar(20),@availabletill varchar(20),@visiting varchar(20))
as
begin
update Doctors set Visiting_Hours=@visiting,Availabletill=@availabletill where FirstName=@doc_name
end

--5
create proc appointmentdata (@pid int,@visit varchar(20))
as begin
select * from AppointmentDetails where PatientID=@pid and Visiting_Date=@visit
end

--6
alter proc appointmentdelete (@aid int,@visit varchar(20))
as 
begin
delete from AppointmentDetails where AppointmentID=@aid and Visiting_Date=@visit
end

--7
create proc doctorselection (@doc_name varchar(20))
as
begin
select * from Doctors where FirstName=@doc_name
end