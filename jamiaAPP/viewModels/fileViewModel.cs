using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jamiaAPP.viewModels
{
    public class fileViewModel
    {
        public HttpPostedFileBase file { get; set; }
        public string description { get; set; }
        public int courseID { get; set; }

    }
}