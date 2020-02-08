using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DevFormAz.ViewModels
{
    public class LoginUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class RegisterUser
    {
        public string Email { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Password { get; set; }
        public Guid UserControl { get; set; } = Guid.NewGuid();
    }
}