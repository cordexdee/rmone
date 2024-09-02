using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using uGovernIT.Utility;

namespace ITAnalyticsBL.DB
{
    [Table(DatabaseObjects.Tables.Analytic_SideLinks)]
    public class SideLink: DBBaseEntity
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter title")]
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string NavigationLink { get; set; }

        [Range(-100, 100, ErrorMessage = "Order must be between -100 to 100")]
        public Int16 Order { get; set; }

    }
}
