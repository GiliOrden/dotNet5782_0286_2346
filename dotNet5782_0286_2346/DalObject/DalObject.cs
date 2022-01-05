﻿using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Dal.DataSource;
using DalApi;
using System.Runtime.CompilerServices;
namespace Dal
{
    sealed  partial class DalObject : IDal
    {
        static readonly IDal instance = new DalObject();
        public static IDal Instance { get => instance; }
        static DalObject() { }
        /// <summary>
        /// constructor, produce the data base
        /// </summary>        
        DalObject()
        {
         DataSource.Initialize();
        }
    }
}
