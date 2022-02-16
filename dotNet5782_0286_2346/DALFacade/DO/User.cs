using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    public struct User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public UserStatuses Status { get; set; }

    }
}
