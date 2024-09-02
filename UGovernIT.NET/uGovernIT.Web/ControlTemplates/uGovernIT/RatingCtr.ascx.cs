using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using uGovernIT.Utility;
using System.Linq;

namespace uGovernIT.Web
{
    public partial class RatingCtr : UserControl
    {


        public List<string> RatingOptions { get; set; }
        public int SelectedValue { get; set; }
        public string SelectedOption { get; set; }
        public Dictionary<int, string> ratings = new Dictionary<int, string>();
        public bool IsMandatory { get; set; }
        public int MaxRating
        {
            get;

            set;

        }

        public string JSOnChange { get; set; }
        public string JSOnMouseOutParam { get; set; }
        public string DisplayMode { get; set; }
        public string DefaultValue { get; set; }
        protected override void OnInit(EventArgs e)
        {
            BindRating();
            if (!string.IsNullOrEmpty(DefaultValue) && ratings.Count > 0)
            {
                int defaultSelection = ratings.Where(x => x.Value.ToLower() == DefaultValue.ToLower()).Select(y => y.Key).FirstOrDefault();
                if (defaultSelection > 0)
                {
                    txtRatingVal.Text = defaultSelection.ToString();
                    this.SelectedValue = defaultSelection;
                }
            }
          
            base.OnInit(e);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsMandatory)
                rqdrating.Visible = true;

            rRatingCtr.Visible = false;
            divCmbRating.Visible = false;
            divrdnButton.Visible = false;

            if (DisplayMode == RatingDisplayMode.RatingBar.ToString())
            {
                rRatingCtr.Visible = true;
                //rRatingCtr.DataSource = ratings;
                //rRatingCtr.DataBind();

            }
            else if (DisplayMode == RatingDisplayMode.Dropdown.ToString())
            {
                divCmbRating.Visible = true;
                BindDropdownRating();
            }
            else if (DisplayMode == RatingDisplayMode.RadioButtonsH.ToString() || DisplayMode == RatingDisplayMode.RadioButtonsV.ToString())
            {
                divrdnButton.Visible = true;
                rdnRadiobuttonH.BorderStyle = BorderStyle.None;
                if (DisplayMode == RatingDisplayMode.RadioButtonsH.ToString())
                    rdnRadiobuttonH.RepeatDirection = RepeatDirection.Horizontal;
                else
                    rdnRadiobuttonH.RepeatDirection = RepeatDirection.Vertical;

                BindRadioButtonRating();
            }

            if (IsPostBack)
                {
                    int sVal = 0;
                    int.TryParse(txtRatingVal.Text.Trim(), out sVal);
                    this.SelectedValue = sVal;
                    if (ratings.ContainsKey(sVal))
                    {
                        this.SelectedOption = ratings[sVal];
                        txtRatingVal.Text = sVal.ToString();
                    }
                }
            
        }

        protected override void OnPreRender(EventArgs e)
        {
            if (DisplayMode == RatingDisplayMode.RatingBar.ToString())
            {
                int val = 0;
                //val= rRatingCtr.Value;
                if (val <= this.SelectedValue)
                {
                    txtRatingVal.Text = this.SelectedValue.ToString();
                    rRatingCtr.Value = this.SelectedValue;

                    rRatingCtr.ItemCount = RatingOptions.Count;
                    rRatingCtr.Titles = string.Join(Constants.Separator6, RatingOptions);
                    lbSelectedOption.Text = string.Format("Value: {0}", rRatingCtr.Value);
                }
            }
            else if (DisplayMode == RatingDisplayMode.Dropdown.ToString())
            {
                txtRatingVal.Text = this.SelectedValue.ToString();
                ddlRating.SelectedIndex = ddlRating.Items.IndexOf(ddlRating.Items.FindByValue(Convert.ToString(this.SelectedValue)));
            }
            else if (DisplayMode == RatingDisplayMode.RadioButtonsH.ToString() || DisplayMode == RatingDisplayMode.RadioButtonsV.ToString())
            {
                txtRatingVal.Text = this.SelectedValue.ToString();
                rdnRadiobuttonH.SelectedIndex = rdnRadiobuttonH.Items.IndexOf(rdnRadiobuttonH.Items.FindByValue(Convert.ToString(this.SelectedValue)));
            }
            base.OnPreRender(e);
        }
        private void BindDropdownRating()
        {
            if (ratings != null && ddlRating.Items.Count == 0)
            {
                ddlRating.DataSource = ratings;
                ddlRating.DataBind();
            }
        }
        private void BindRadioButtonRating()
        {
            if (ratings != null && rdnRadiobuttonH.Items.Count == 0)
            {
                rdnRadiobuttonH.DataSource = ratings;
                rdnRadiobuttonH.DataBind();
            }
        }
        private void BindRating() 
        {
            if (MaxRating == 0)
                MaxRating = 4;
            int index = 0;
            for (var i = 1; i <= MaxRating; i++)
            {
                if (RatingOptions != null && RatingOptions.Count > index)
                {
                    ratings.Add(i, RatingOptions[index]);
                }
                else
                {
                    ratings.Add(i, string.Empty);
                }
                index += 1;
            }
        }
    }
}
