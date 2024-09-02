using System;
using uGovernIT.Utility;
using System.ComponentModel.DataAnnotations;

namespace uGovernIT.Manager.PMM
{
    public class NewProjectRequest
    {
        [DataType(DataType.Text)]
        [Required(ErrorMessage ="Please enter Title.")]
        public string Title { get; set; }
        public DateTime? StartDate { get; set; }
        public PMMMode Mode { get; set; }
        public string SelectedItem { get; set; }
        public string[] ImportDataOption { get; set; }
        public bool IscreateDocumentPortal { get; set; }
        public string NPRTemplateID { get; set; }
        public bool IsDefaultFolder { get; set; }
        public string LifeCycle { get; set; }
    }
}
