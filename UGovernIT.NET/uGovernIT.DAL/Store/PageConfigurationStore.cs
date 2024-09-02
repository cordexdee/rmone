using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using uGovernIT.Utility;
using uGovernIT.Utility.DockPanels;

namespace uGovernIT.DAL.Store
{
    public class PageConfigurationStore:StoreBase<PageConfiguration>, IPageConfigurationStore
    {
        public PageConfigurationStore(CustomDbContext context):base(context)
        {

        }
        public virtual List<PageConfiguration> Loads()
        {
            List<PageConfiguration> list = base.Load();
            list.ForEach(x => {
                XmlDocument document = new XmlDocument();
                document.LoadXml(x.ControlInfo);
                List<DockPanelSetting> listControl = new List<DockPanelSetting>();
                x.ControlInfoList =UGITUtility.DeSerializeAnObject(document, listControl) as List<DockPanelSetting>;
                x.LayoutInfoList = SerializationExtensions.JsonDeserialize<Dictionary<string, List<object>>>(x.LayoutInfo);
            });
            return list;
        }
        public PageConfiguration Gets(object ID)
        {
            PageConfiguration xx = base.Get(ID);
            XmlDocument document = new XmlDocument();
            document.LoadXml(xx.ControlInfo);
            List<DockPanelSetting> listControl = new List<DockPanelSetting>();
            xx.ControlInfoList = UGITUtility.DeSerializeAnObject(document, listControl) as List<DockPanelSetting>;
            xx.LayoutInfoList = SerializationExtensions.JsonDeserialize<Dictionary<string, List<object>>>(xx.LayoutInfo);
            return xx;
        }

        public PageConfiguration LoadsByID(long ID)
        {
            PageConfiguration xx = base.LoadByID(ID);
            XmlDocument document = new XmlDocument();
            document.LoadXml(xx.ControlInfo);
            List<DockPanelSetting> listControl = new List<DockPanelSetting>();
            xx.ControlInfoList = UGITUtility.DeSerializeAnObject(document, listControl) as List<DockPanelSetting>;
            xx.LayoutInfoList = SerializationExtensions.JsonDeserialize<Dictionary<string, List<object>>>(xx.LayoutInfo);
            return xx;
        }


        public override PageConfiguration Get(Expression<Func<PageConfiguration, bool>> where, Expression<Func<PageConfiguration, PageConfiguration>> order = null, List<Expression<Func<PageConfiguration, object>>> includeExpressions = null)
        {
            PageConfiguration config =base.Get(where,order,includeExpressions);
            if(config!=null && config.ControlInfoList == null && !string.IsNullOrWhiteSpace(config.ControlInfo))
            {
                XmlDocument document = new XmlDocument();
                document.LoadXml(config.ControlInfo);
                List<DockPanelSetting> listControl = new List<DockPanelSetting>();
                config.ControlInfoList=UGITUtility.DeSerializeAnObject(document, listControl) as List<DockPanelSetting>;
                config.LayoutInfoList = SerializationExtensions.JsonDeserialize<Dictionary<string, List<object>>>(config.LayoutInfo);
            }
            return config;
        }
        
    }
    public interface IPageConfigurationStore : IStore<PageConfiguration>
    {
       
    }
}
