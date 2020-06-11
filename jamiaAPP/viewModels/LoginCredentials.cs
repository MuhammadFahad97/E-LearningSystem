using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jamiaAPP.viewModels
{
    public class LoginCredentials
    {

        public string userName { get; set; }
        public string userPassword { get; set; }
        public bool is_remember { get; set; }

    }
}