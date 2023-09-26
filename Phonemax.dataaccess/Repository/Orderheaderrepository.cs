﻿using Phonemax.dataaccess.Data;
using Phonemax.dataaccess.Repository.Irepository;
using Phonemax.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phonemax.dataaccess.Repository
{
    public class Orderheaderrepository:Repository<Orderheader>,Iorderheader
    {
        private readonly ApplicationDbContext _context;
        public Orderheaderrepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
