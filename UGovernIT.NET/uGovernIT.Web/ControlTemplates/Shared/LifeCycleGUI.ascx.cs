using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using uGovernIT.Utility;
using System.Linq;
using uGovernIT.Manager;
using System.Web;

namespace uGovernIT.Web
{
    public partial class LifeCycleGUI : UserControl
    {
        public LifeCycle ModuleLifeCycle { get; set; }
        public int CurrentStep { get; set; }
        public bool IsOnHold { get; set; }
        double totalModuleWeight;
        bool setAlternateStageGraphicLabel;
        bool ShowPointer { get; set; }
        public bool fromAdmin { get; set; }
        ConfigurationVariableManager objConfigurationVariableHelper = new ConfigurationVariableManager(HttpContext.Current.GetManagerContext());
        protected void Page_Load(object sender, EventArgs e)
        {
            //set whether to set alternate graphic label or not
            setAlternateStageGraphicLabel = objConfigurationVariableHelper.GetValueAsBool("ModuleStageAlternateLabel");

            totalModuleWeight = ModuleLifeCycle.Stages.Sum(x => x.StageWeight);
            if (totalModuleWeight <= 0)
            {
                totalModuleWeight = ModuleLifeCycle.Stages.Count();
            }

            stageRepeater.DataSource = ModuleLifeCycle.Stages;
            stageRepeater.DataBind();
        }

        protected override void OnInit(EventArgs e)
        {
            if (HttpContext.Current.Request.Browser.IsMobileDevice) //Mobile
            {
                topGraphicdiv.Visible = false;
            }

            base.OnInit(e);
        }



        protected void stageRepeater_ItemDataBound1(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LifeCycleStage stage = (LifeCycleStage)e.Item.DataItem;
                Repeater repeator = (Repeater)sender;
                List<LifeCycleStage> allStages = (List<LifeCycleStage>)repeator.DataSource;
                int stageIdex = allStages.IndexOf(stage);

                HtmlTableCell tdStage = (HtmlTableCell)e.Item.FindControl("tdStage");
                Literal lbStageNumber = (Literal)e.Item.FindControl("lbStageNumber");
                HtmlGenericControl stageTitleContainer = (HtmlGenericControl)e.Item.FindControl("stageTitleContainer");
                Literal stageTitle = (Literal)e.Item.FindControl("stageTitle");
                HtmlGenericControl activeStageArrow = (HtmlGenericControl)e.Item.FindControl("activeStageArrow");
                HtmlTableCell tdStepLine = (HtmlTableCell)e.Item.FindControl("tdStepLine");
                if (fromAdmin)
                {
                    lbStageNumber.Visible = true;
                }
                else
                {
                    lbStageNumber.Visible = false;
                }
                if (stage.StageStep > 9)
                {
                    HtmlGenericControl stageNo = (HtmlGenericControl)e.Item.FindControl("stageNo");
                    if (stageNo != null)
                        stageNo.Style.Add("left", "6px");
                }

                if (stage.StageStep < CurrentStep)
                {
                    if (fromAdmin)
                    {
                    tdStage.Style.Add(HtmlTextWriterStyle.BackgroundImage, "/Content/images/stepcirclebg_visited.gif");
                    }
                    else
                    {
                        tdStage.Attributes.Add("class", "node nodeComplete");
                    }
                }
                else if (stage.StageStep == CurrentStep)
                {
                    if (!IsOnHold)
                    {
                        if (fromAdmin)
                        {
                        tdStage.Style.Add(HtmlTextWriterStyle.BackgroundImage, "/Content/images/stepcirclebg_active.gif");
                        }
                        else
                        {
                            tdStage.Attributes.Add("class", "node nodeInprogress");
                        }
                    }
                    else
                    {
                        if (fromAdmin)
                        {
                        tdStage.Style.Add(HtmlTextWriterStyle.BackgroundImage, "/Content/images/stepcirclebg-red.gif");
                        }
                        else
                        {
                            tdStage.Attributes.Add("class", "node nodeComplete");
                        }
                    }
                }
                else
                {
                    if (fromAdmin)
                    {
                        tdStage.Style.Add(HtmlTextWriterStyle.BackgroundImage, "/Content/images/stepcirclebg.gif");
                    }
                    else
                    {
                        tdStage.Attributes.Add("class", "node");                        
                    }
                }

                tdStage.Attributes.Add("title", stage.StageTitle);
                tdStage.Attributes.Add("aStage", stage.ApprovedStage != null ? stage.ApprovedStage.StageStep.ToString() : "0");
                tdStage.Attributes.Add("rStage", stage.ReturnStage != null ? stage.ReturnStage.StageStep.ToString() : "0");
                tdStage.Attributes.Add("rjStage", stage.RejectStage != null ? stage.RejectStage.StageStep.ToString() : "0");

                lbStageNumber.Text = Convert.ToString(stage.StageStep);

                if (CurrentStep % 2 != 0)
                {
                    if ((stage.StageStep % 2 == 0 && setAlternateStageGraphicLabel))
                    {
                        stageTitleContainer.Attributes.Add("class", "stage-titlecontainer alternategraphiclabel");
                        stageTitleContainer.Style.Add(HtmlTextWriterStyle.Top, "-28px");
                    }
                    else
                    {
                        stageTitleContainer.Style.Add(HtmlTextWriterStyle.Top, "15px");
                    }
                }
                else
                {
                    if (stage.StageStep % 2 != 0 && setAlternateStageGraphicLabel)
                    {
                        stageTitleContainer.Attributes.Add("class", "stage-titlecontainer alternategraphiclabel");
                        stageTitleContainer.Style.Add(HtmlTextWriterStyle.Top, "-28px");
                    }
                    else
                    {
                        stageTitleContainer.Style.Add(HtmlTextWriterStyle.Top, "15px");
                    }
                }

                stageTitle.Text = stage.StageTitle;

                if (stage.StageStep == CurrentStep && ShowPointer)
                {
                    activeStageArrow.Attributes.Add("class", "activestagearrow");
                }

                if (stage.StageStep < CurrentStep)
                {
                    if (stage.StageStep == 1)
                    {
                        if (fromAdmin)
                        {
                        tdStepLine.Style.Add(HtmlTextWriterStyle.BackgroundImage, "/Content/images/stepline_active.gif");
                        }
                        else
                        {
                            tdStepLine.Attributes.Add("class", "step complete");
                        }
                    }
                    else
                    {
                        if (fromAdmin)
                        {
                        tdStepLine.Style.Add(HtmlTextWriterStyle.BackgroundImage, "/Content/images/stepline_active.gif");
                        }
                        else
                        {
                            tdStepLine.Attributes.Add("class", "step complete");
                        }
                    }
                }
                else
                {
                    if (fromAdmin)
                    {
                        tdStepLine.Style.Add(HtmlTextWriterStyle.BackgroundImage, "/Content/images/stepline.gif");
                    }
                    else
                    {
                        tdStepLine.Attributes.Add("class", "step inprogress");                        
                    }

                }

                if (totalModuleWeight > 0)
                {
                    tdStepLine.Style.Add(HtmlTextWriterStyle.Width, string.Format("{0}px", (((Convert.ToInt32(stage.StageWeight) / totalModuleWeight) * 1000))));
                }
                else
                {
                    tdStepLine.Style.Add(HtmlTextWriterStyle.Width, "100px");
                }

                if (stageIdex == allStages.Count - 1)
                {
                    tdStepLine.Visible = false;
                }
            }
        }
    }
}
