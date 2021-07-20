using NUnit.Framework;
using UserManagement.ADUserManagement;
using System.DirectoryServices;

namespace UserManagement.Tests
{
    public class Tests
    {
        private string _ouLdapPath;
        private string _userLdapPath = "";
        private string _cn;
        private string _samAccountName;
        private string _givenName;
        private string _sn;
        private string _pwd;

        [SetUp]
        public void Setup()
        {
            _ouLdapPath = "LDAP://OU=DAVS_USER,OU=Services,DC=de,DC=eld,DC=extranet,DC=lab";
            _cn = "UnitTestCreateUser";
            _samAccountName = _cn;
            _givenName = "Oliver";
            _sn = "Hauck";
            _pwd = "Start,1234!";
        }

        [Test]
        public void CreateADUser_UserShouldExist()
        {
            ADUser myNewADUser = ADUser.Create(_ouLdapPath, _cn, _samAccountName, _givenName, _sn, _pwd);
            if(myNewADUser != null)
                _userLdapPath = myNewADUser.LDAPPath;
            Assert.That(myNewADUser != null,"No User object has been returned by ADUSer.Create() function...");
            Assert.That(DirectoryEntry.Exists(myNewADUser.LDAPPath));
        }

        [TearDown]
        public void TearDown()
        {
            if (_userLdapPath != "" && DirectoryEntry.Exists(_userLdapPath))
            {
                DirectoryEntry myDEOU = new DirectoryEntry(_ouLdapPath);
                DirectoryEntry myDEUser = new DirectoryEntry(_userLdapPath);
                myDEOU.Children.Remove(myDEUser);
                Assert.That(!DirectoryEntry.Exists(_userLdapPath));
            }
        }
    }
}