﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cmt.UnitOfWorks.Interfaces
{
    public interface IUnitOfWork
    {
        void SaveChanges();
    }
}
