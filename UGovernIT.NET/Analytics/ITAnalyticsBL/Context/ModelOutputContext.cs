using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using uGovernIT.DAL.Infratructure;
using uGovernIT.DAL;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace ITAnalyticsBL.DB
{
    public class ModelOutputContext : BaseDbContext
    {
        public ModelOutputContext() : base(CustomDbContext.Create(Convert.ToString(ConfigurationManager.ConnectionStrings["analyticdb"])))
        {

        }
        public ModelOutputContext(CustomDbContext context) : base(context)
        {

        }
        public DbSet<ModelOutput> ModelOutputs { get; set; }
        public DbSet<ModelSectionOutput> ModelSectionOutputs { get; set; }
        public DbSet<ModelSubSectionOutput> ModelSubSectionOutputs { get; set; }
        public DbSet<ModelFeatureOutput> ModelFeatureOutputs { get; set; }
    }
}
