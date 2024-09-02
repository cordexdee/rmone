using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace uGovernIT.Utility.Entities.DMSDB
{
    public class TreeViewModel
    {
        //public const string TooltipFormat = "{0} [{1}]";
        public int ID { get; set; }
        public int? ParentID { get; set; }
        public string title { get; set; }
        public bool isFolder { get; set; }
        public bool isSubFolder { get; set; }
        public bool hasItems { get; set; }
        public string key { get; set; }
        public bool activate { get; set; }
        public string folderName { get; set; }
        public string Class { get; set; }
        public bool isUpload { get; set; }
        public bool isTabSelected { get; set; }
        public IEnumerable<TreeViewModel> children { get; set; }

        [Required(ErrorMessage = "Please select file.")]
        [Display(Name = "Browse File")]
        public HttpPostedFileBase[] files { get; set; }
        // public bool select { get; set; }
    }
}