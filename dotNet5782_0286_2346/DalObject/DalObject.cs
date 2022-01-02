using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dal.DataSource;
using DalApi;
namespace Dal
{
    internal partial class DalObject : IDal
    { 
        /// <summary>
        /// constructor, produce the data base
        /// </summary>
        
        public DalObject()
        {
            DataSource.Initialize();
        }

    }
}
