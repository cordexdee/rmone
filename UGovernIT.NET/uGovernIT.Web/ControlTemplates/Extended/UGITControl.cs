using System.Web.UI.WebControls;

namespace uGovernIT.Web
{
    public class UGITControl: WebControl
    {
        public string FieldName { get; set; }
        public string DefaultValue { get; set; }
        public UGITControl()
        {            
        }
    }
}