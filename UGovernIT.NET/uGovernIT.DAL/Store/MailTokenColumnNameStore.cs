﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Utility;
namespace uGovernIT.DAL.Store
{
    public class MailTokenColumnNameStore: StoreBase<MailTokenColumnName>, IMailTokenColumnNameStore
    {
        public MailTokenColumnNameStore(CustomDbContext context) : base(context)
        {

        }
    }
    public interface IMailTokenColumnNameStore : IStore<MailTokenColumnName>
    {

    }
}