
using System;
using System.Web.UI;
using System.Xml;
using System.Collections.Generic;
using DevExpress.Web;
using uGovernIT.Utility;
using uGovernIT.Manager;
using uGovernIT.Core;
using System.Web;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class SetUgitTheme : UserControl
    {
        ConfigurationVariableManager configHelper = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        protected override void OnInit(EventArgs e)
        {
           
            BindTheme();
            BindFont();
            if (!IsPostBack)
            {
                try
                {
                    int focusedCardIndex = 0;
                    if (!string.IsNullOrEmpty(configHelper.GetValue(ConfigConstants.UgitTheme)))
                    {
                        string uGitTheme = configHelper.GetValue(ConfigConstants.UgitTheme);
                        if (uGitTheme != null)
                        {
                            XmlDocument obj = new XmlDocument();
                            obj.LoadXml(uGitTheme);
                            UGITTheme objUgitTheme = new UGITTheme();
                            objUgitTheme = (UGITTheme)uHelper.DeSerializeAnObject(obj, objUgitTheme);
                            if (objUgitTheme != null)
                            {
                                focusedCardIndex = ugitThemesCardView.FindVisibleIndexByKeyValue(objUgitTheme.ThemeName);
                                cmbFonts.Value = objUgitTheme.FontName;

                            }
                        }
                    }
                    else
                    {
                        focusedCardIndex = ugitThemesCardView.FindVisibleIndexByKeyValue(Constants.BlackTheme);
                    }
                    if (focusedCardIndex > -1)
                        ugitThemesCardView.FocusedCardIndex = focusedCardIndex;
                }
                catch (Exception ex)
                {
                    Util.Log.ULog.WriteException(ex);
                }
            }

            base.OnInit(e);
        }
        protected void Page_Load(object sender, EventArgs e)
        {
           


        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            UGITTheme.SetThemeName(string.Empty);
            UGITTheme.SetFontName(string.Empty);
            string s = string.Empty;
            if (!String.IsNullOrEmpty(Convert.ToString(hdnFont.Value)))
                s = Convert.ToString(hdnFont.Value);
            //Changing DevEx Theme
            #region Change DevEx Theme
            UGITTheme selectedTheme = (UGITTheme)ugitThemesCardView.GetRow(ugitThemesCardView.FocusedCardIndex);
            if (selectedTheme != null)
            {
                ConfigurationVariable cnfigVariable = configHelper.LoadVaribale(ConfigConstants.UgitTheme);
                if(cnfigVariable == null)
                    cnfigVariable = new ConfigurationVariable();
               
                selectedTheme.FontName = cmbFonts.Text;
                cnfigVariable.KeyName = ConfigConstants.UgitTheme;
                cnfigVariable.KeyValue = uHelper.SerializeObject(selectedTheme).OuterXml;
                cnfigVariable.Title = ConfigConstants.UgitTheme;
                cnfigVariable.CategoryName = ConfigConstants.General;
                cnfigVariable.Description = "uGovernIT Theme (will be auto-set by Theme selector)";
                cnfigVariable.Type = Constants.ConfigVariableType.Text;
                cnfigVariable.Internal = true;

                if (cnfigVariable.ID > 0)
                    configHelper.Update(cnfigVariable);
                else
                    configHelper.Insert(cnfigVariable);
            }
            #endregion
            uHelper.ClosePopUpAndEndResponse(Context, true);
        }
        private void BindTheme()
        {
            UGITTheme ugitTheme = new UGITTheme();
            List<UGITTheme> lstThemes = ugitTheme.GetThemes();
            ugitThemesCardView.DataSource = lstThemes;
            ugitThemesCardView.DataBind();
        }
        private void BindFont()
        {
            cmbFonts.Items.Clear();
            cmbFonts.Items.Insert(0, new ListEditItem("Angsana New", "0"));
            cmbFonts.Items.Insert(1, new ListEditItem("Impact", "1"));
            cmbFonts.Items.Insert(2, new ListEditItem("Georgia", "2"));
            cmbFonts.Items.Insert(3,new ListEditItem("Courier New","3"));
            cmbFonts.Items.Insert(4, new ListEditItem("Verdana", "4"));
            cmbFonts.Items.Insert(5, new ListEditItem("Comic Sans MS", "5"));
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            uHelper.ClosePopUpAndEndResponse(Context, true);

        }
    }
}
