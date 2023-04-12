﻿using BulkyBook.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.Core
{
    public interface IUnitOfWork
    {
        ICategoryRepository Categories { get; }

        void Complete();
    }
}