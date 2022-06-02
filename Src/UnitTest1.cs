using Front_office_staff_Backend;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MSTestUsers
{
    [TestClass]
    public class UnitTest1
    {
        Users_BE u = new Users_BE();
        Patient p;
        [TestInitialize]
        public void fix()
        {
            p = new();
            p.firstname = "Mukesh";
            p.lastname = "Bijari";
            p.sex = "M";
            p.age = 28;
            p.dateofbirth = "09/11/1992";
        }
        [TestMethod]
        [DataRow("Admin","Admin@100")]
        [DataRow("John", "J@200")]
        [DataRow("Mohan", "12*&&&$#")]
        [DataRow("GkShukla", "Shukla@123")]
        [DataRow("Testformorethan10char", "Password@")]
        public void TestMethod1(string uname,string pass)
        {
            var b=u.logintest(uname, pass);
            Assert.IsTrue(b);

        }
       
        [TestMethod]
        [DataRow("Mahesh", "passcheck")]
        public void TestMethod2(string name, string pwd)
        {
            var c = Assert.ThrowsException<Users_BE.Login_Error>(() => u.logintest(name, pwd));
            Assert.AreEqual("Password should contain '@' as part of it", c.Message);
        }
        [TestMethod]
        [DataRow("Testformorethan10char", "Password@")]
        public void TestMethod3(string name, string pwd)
        {
            var c = Assert.ThrowsException<Users_BE.Login_Error>(() => u.logintest(name, pwd));
            Assert.AreEqual("UserName length should be 10 or less", c.Message);
        }
        [TestMethod]
        public void TestMethod4()
        {
            var c = u.addpatienttest(p);
            Assert.AreEqual("Patient "+p.firstname+" "+p.lastname+" Added Successfully...", c);

        }

    }
}
