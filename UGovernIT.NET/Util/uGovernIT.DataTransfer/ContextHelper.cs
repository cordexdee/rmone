using Microsoft.SharePoint.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using uGovernIT.Util.Log;
using ClientOM = Microsoft.SharePoint.Client;

namespace uGovernIT.DataTransfer
{
    class ContextHelper
    {

        public static ClientOM.ClientContext CreateContext(SiteAuthentication auth)
        {
            ClientOM.ClientContext context = new ClientOM.ClientContext(auth.SiteUrl);
            if (!string.IsNullOrWhiteSpace(auth.UserName))
            {
                context.Credentials = new System.Net.NetworkCredential(auth.UserName, auth.Password,auth.Domain);
            }
            //context.RequestTimeout = 600000; // 10 min

            // Enable only if FBA auth is enable on site
            context.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(clientContext_ExecutingWebRequest);
            return context;
        }
        static void clientContext_ExecutingWebRequest(object sender, WebRequestEventArgs e)
        {
            e.WebRequestExecutor.WebRequest.Headers.Add("X-FORMS_BASED_AUTH_ACCEPTED", "f");
        }
        public static ClientOM.ListItemCollection GetDataFromList(ClientOM.ClientContext context, string listName, string moduleName, ClientOM.ListItemCollectionPosition position)
        {
            ClientOM.Web web = context.Web;
            ClientOM.List spList = web.Lists.GetByTitle(listName);
            return GetDataFromList(context, spList, moduleName, position);
        }

        public static ClientOM.ListItemCollection GetItemCollectionList(ClientOM.ClientContext context, ClientOM.List spList, string where, string viewFields, ClientOM.ListItemCollectionPosition position)
        {
            ClientOM.Web web = context.Web;
            ClientOM.CamlQuery query = new ClientOM.CamlQuery();
            string whereQuery = string.Empty;
            if (!string.IsNullOrWhiteSpace(where))
                whereQuery = string.Format("<Query><Where>{0}</Where>{1}</Query>", where, viewFields);
            query.ViewXml = string.Format("<View><RowLimit>{0}</RowLimit>{1}</View>", JsonConfig.Config.Global.permission.spsource.batchfetchsize, whereQuery);

            return GetDataFromList(context, spList, query, position);
        }

        public static ClientOM.ListItemCollection GetDataFromList(ClientOM.ClientContext context, string listName, ClientOM.CamlQuery query, ClientOM.ListItemCollectionPosition position)
        {
            ClientOM.Web web = context.Web;
            ClientOM.List spList = web.Lists.GetByTitle(listName);
            return GetDataFromList(context, spList, query, position);
        }

        public static ClientOM.ListItemCollection GetDataFromList(ClientOM.ClientContext context, ClientOM.List spList, string moduleName, ClientOM.ListItemCollectionPosition position)
        {
            context.Load(spList);
            ClientOM.FieldCollection fCollection = spList.Fields;
            context.Load(fCollection);
            context.Load(spList.ContentTypes);
            context.ExecuteQuery();
            ClientOM.CamlQuery query = new ClientOM.CamlQuery();
            string whereQuery = string.Empty;
            if (!string.IsNullOrWhiteSpace(moduleName) && spList.Fields.FirstOrDefault(x=>x.InternalName == "ModuleNameLookup") != null)
            {
                whereQuery = string.Format("<Query><Where><Eq><FieldRef Name='{0}'/><Value Type='Lookup'>{1}</Value></Eq></Where></Query>", "ModuleNameLookup", moduleName);
            }
            if (!string.IsNullOrWhiteSpace(moduleName) && spList.Fields.FirstOrDefault(x => x.InternalName == "ModuleName") != null)
            {
                whereQuery = string.Format("<Query><Where><Eq><FieldRef Name='{0}'/><Value Type='Lookup'>{1}</Value></Eq></Where></Query>", "ModuleName", moduleName);
            }
            if (!string.IsNullOrWhiteSpace(moduleName) && spList.Fields.FirstOrDefault(x => x.InternalName == "ModuleName") != null && (spList.Title== "TicketCountTrends" || spList.Title== "TicketEvents" || spList.Title == "TicketWorkflowSLASummary"))
            {
                whereQuery = string.Format("<Query><Where><Eq><FieldRef Name='{0}'/><Value Type='Text'>{1}</Value></Eq></Where></Query>", "ModuleName", moduleName);
            }
            query.ViewXml = string.Format("<View><RowLimit>{0}</RowLimit>{1}</View>", JsonConfig.Config.Global.permission.spsource.batchfetchsize, whereQuery);
            if (position != null && !string.IsNullOrWhiteSpace(position.PagingInfo))
                query.ListItemCollectionPosition = position;

            ClientOM.ListItemCollection collection = spList.GetItems(query);

            try
            {
                context.Load(collection);
                context.ExecuteQuery();

                if (collection.ListItemCollectionPosition != null)
                    position.PagingInfo = collection.ListItemCollectionPosition.PagingInfo;
                else
                    position.PagingInfo = string.Empty;

                return collection;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "GetDataFromList");
                return null;
            }
        }


        public static ClientOM.ListItemCollection GetDataFromList(ClientOM.ClientContext context, ClientOM.List spList, ClientOM.CamlQuery query, ClientOM.ListItemCollectionPosition position)
        {
            context.Load(spList);
            context.Load(spList.Fields);
            if (position != null && !string.IsNullOrWhiteSpace(position.PagingInfo))
                query.ListItemCollectionPosition = position;
            ClientOM.ListItemCollection collection = spList.GetItems(query);

            try
            {
                context.Load(collection);
                context.ExecuteQuery();

                if (position != null)
                {
                    if (collection.ListItemCollectionPosition != null)
                        position.PagingInfo = collection.ListItemCollectionPosition.PagingInfo;
                    else
                        position.PagingInfo = string.Empty;
                }

                return collection;
            }
            catch (Exception ex)
            {
                ULog.WriteException(ex, "GetDataFromList");
                return null;
            }
        }

        public static ClientOM.FieldLookupValue GetLookupValue(ClientOM.ClientContext context, string module, ClientOM.FieldLookup field, string value)
        {
            ClientOM.FieldLookupValue lookup = null;
            using (ClientOM.ClientContext cContext = new ClientOM.ClientContext(context.Url))
            {
                cContext.Credentials = context.Credentials;
                if (string.IsNullOrWhiteSpace(field.LookupList))
                    return lookup;

                Guid listID = Guid.Empty;
                Guid.TryParse(field.LookupList, out listID);

                if (listID == null)
                    return lookup;

                try
                {
                    ClientOM.List list = cContext.Web.Lists.GetById(listID);
                    cContext.Load(list.Fields);
                    cContext.ExecuteQuery();

                    string whereQuery = string.Empty;
                    if (!string.IsNullOrWhiteSpace(module) && list.Fields.FirstOrDefault(x=>x.InternalName =="ModuleNameLookup") != null)
                        whereQuery = string.Format("<Query><Where><And><Eq><FieldRef Name='{0}'/><Value Type='Text'><![CDATA[{1}]]></Value></Eq><Eq><FieldRef Name='{2}'/><Value Type='Lookup'>{3}</Value></Eq></And></Where></Query>", field.LookupField, value, "ModuleNameLookup", module);
                    else
                        whereQuery = string.Format("<Query><Where><Eq><FieldRef Name='{0}'/><Value Type='Text'><![CDATA[{1}]]></Value></Eq></Where></Query>", field.LookupField, value);

                    ClientOM.CamlQuery query = new ClientOM.CamlQuery();
                    query.ViewXml = string.Format("<View><ViewFields><FieldRef Name='Title'/></ViewFields><RowLimit>1</RowLimit>{0}</View>", whereQuery);
                    ClientOM.ListItemCollection collection = list.GetItems(query);
                    cContext.Load(collection);
                    cContext.ExecuteQuery();

                    if (collection.Count > 0)
                    {
                        lookup = new ClientOM.FieldLookupValue();
                        lookup.LookupId = collection[0].Id;
                    }
                }
                catch (Exception ex)
                {
                    ULog.WriteException(ex, "GetLookupValue");
                    return null;
                }
            }
            
            return lookup;
        }

        /// <summary>
        /// Get field from source list based on first contenttype 
        /// </summary>
        /// <param name="listName"></param>
        /// <returns></returns>
        public static List<string> GetListFields(ClientOM.ClientContext context, string listName)
        {
            List<string> fieldList = new List<string>();

            ClientOM.List spList = context.Web.Lists.GetByTitle(listName);
            context.Load(spList);
            context.Load(spList.ContentTypes);
            context.ExecuteQuery();

            ClientOM.FieldCollection fields = spList.Fields;
            context.Load(fields);
            context.ExecuteQuery();

            List<ClientOM.Field> sFields = fields.AsEnumerable().Where(x => !x.ReadOnlyField && !x.Hidden && !x.Sealed && x.InternalName != "ContentType" && x.InternalName != "Attachments").ToList();

            foreach (ClientOM.Field f in sFields)
            {
                fieldList.Add(f.InternalName);
            }

            if (!fieldList.Contains("Title"))
                fieldList.Add("Title");


            //if (!fieldList.Contains("Created"))
            //    fieldList.Add("Created");

            //if (!fieldList.Contains("Author"))
            //    fieldList.Add("Author");

            //if (!fieldList.Contains("Editor"))
            //    fieldList.Add("Editor");

            //if (!fieldList.Contains("Modified"))
            //    fieldList.Add("Modified");

            return fieldList;
        }


        public static string GetListID(ClientOM.ClientContext context, string listName)
        {
            ClientOM.List list = context.Web.Lists.GetByTitle(listName);
            if (list == null)
                return string.Empty;

            context.Load(list, x => x.Id);
            try
            {
                context.ExecuteQuery();
            }
            catch (Exception ex)
            {
                // Throws exception if list doesn't exist!
                ULog.WriteException(ex, "Error getting ID of list " + listName);
                return string.Empty;
            }

            return list.Id.ToString();
        }
    }
}
