using System;
using System.Security.Cryptography;

namespace Final_Year_Project.Networking
{
    class User
    {
        public String username { get; private set; }
        public Rfc2898DeriveBytes password { get; private set; }

        public User(String username)
        {
            this.username = username;
            this.password = password;
        }
    }
}
