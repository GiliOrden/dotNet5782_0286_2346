using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BO.EnumsBL;

namespace BO
{
    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public UserStatuses Status { get; set; }
    }
}
