using System;
using System.Web;
using uGovernIT.Manager;
using uGovernIT.Manager.Core;
using uGovernIT.Utility;

namespace uGovernIT.Web.ControlTemplates.uGovernIT
{
    public partial class EditWikiNavigation : System.Web.UI.UserControl
    {
        public int WikiID { get; set; }
        public WikiCategory wikiCategory = new WikiCategory();
        public string ConditionUrl { get; set; }
        ApplicationContext context = HttpContext.Current.GetManagerContext();
        WikiCategoryManager wikiCategoryManager = null;
        protected void Page_Load(object sender, EventArgs e)
        {

             wikiCategoryManager = new WikiCategoryManager(context);

            wikiCategory = wikiCategoryManager.LoadByID(WikiID);

            if (!IsPostBack)
            {
                if (wikiCategory != null)
                {
                    LnkbtnDelete.Visible = true;
                    txtTitle.Text = wikiCategory.Title;
                    fileUploadIcon.SetImageUrl(wikiCategory.ImageUrl);
                    txtItemOrder.Text = (wikiCategory.ItemOrder).ToString();

                    ddlWikiType.SelectedValue = wikiCategory.ColumnType;
                    string strcondition = FormulaBuilder.GetConditionExpression(context,Convert.ToString(wikiCategory.ConditionalLogic), true);

                    lblCondition.Text = strcondition;
                    hdnSkipOnCondition.Set("SkipCondition", strcondition);
                }
                else
                {
                    LnkbtnDelete.Visible = false;
                }
            }
            if (ddlWikiType.SelectedValue == "CustomWiki")
                trExpression.Visible = true;
            else
                trExpression.Visible = false;

            ConditionUrl = UGITUtility.GetAbsoluteURL(string.Format("/Layouts/ugovernit/uGovernITConfiguration.aspx?control=modulestagerule&moduleName={0}&controlId={1}", "WIKI", lblCondition.ClientID));

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

           WikiCategory wikiCategory = new WikiCategory();
            string imageURL = !string.IsNullOrEmpty(fileUploadIcon.GetImageUrl()) ? fileUploadIcon.GetImageUrl() : string.Empty;
            //if (!imageURL.StartsWith("http") && !imageURL.StartsWith("/"))
            //    imageURL = string.Format("/{0}", imageURL);

            if (txtItemOrder.Text == null || txtItemOrder.Text=="")
                txtItemOrder.Text = "0";
            wikiCategory.Title = txtTitle.Text.Trim();
            wikiCategory.ItemOrder = Convert.ToInt32(txtItemOrder.Text);
            wikiCategory.ColumnType = ddlWikiType.SelectedItem.Text;
            wikiCategory.ImageUrl = imageURL;

            if (ddlWikiType.SelectedItem.Text == "CustomWiki")
            {
                wikiCategory.ConditionalLogic = FormulaBuilder.GetConditionExpression(context,hdnSkipOnCondition.Contains("SkipCondition") ? Convert.ToString(hdnSkipOnCondition.Get("SkipCondition")) : "", false);
            }

            if (WikiID == 0)
            {
                wikiCategoryManager.Insert(wikiCategory);

            }
            else
            {
                wikiCategory.ID = WikiID;
                wikiCategoryManager.Update(wikiCategory);
            }
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void LnkbtnDelete_Click(object sender, EventArgs e)
        {
            if (wikiCategory != null)
                wikiCategoryManager.Delete(wikiCategory);
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

        protected void ddlWikiType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlWikiType.SelectedItem.Text == "CustomWiki")
            {
                trExpression.Visible = true;
                if (wikiCategory != null)
                {
                    string strcondition = FormulaBuilder.GetConditionExpression(context,Convert.ToString(wikiCategory.ConditionalLogic), true);
                    lblCondition.Text = strcondition;
                    hdnSkipOnCondition.Set("SkipCondition", strcondition);
                }
            }

        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }

    }
}