using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace uGovernIT.Utility
{
    public static class CAMLQueryHelper
    {
        /// <summary>
        /// Field Ref to Create Xelement
        /// </summary>
        /// <param name="name"></param>
        /// <param name="lookupId"></param>
        /// <returns></returns>
        public static XElement FieldRef(string name, string isLookupId)
        {
            return new XElement(CAMLQuery.FieldRef, new XAttribute(CAMLQuery.Name, name), new XAttribute(CAMLQuery.LookupId, isLookupId));
        }

        public static XElement FieldRef(string name)
        {
            return new XElement(CAMLQuery.FieldRef, new XAttribute(CAMLQuery.Name, name));
        }

        public static XElement Value(string type, object value)
        {
            return new XElement(CAMLQuery.Value, new XAttribute(CAMLQuery.Type, type), value);
        }

        public static XElement Eq(XElement fieldRef, XElement value)
        {
            return new XElement(CAMLQuery.Eq, fieldRef, value);
        }

        public static XElement Neq(XElement fieldRef, XElement value)
        {
            return new XElement(CAMLQuery.Neq, fieldRef, value);
        }

        public static XElement Gt(XElement fieldRef, XElement value)
        {
            return new XElement(CAMLQuery.Gt, fieldRef, value);
        }

        public static XElement Lt(XElement fieldRef, XElement value)
        {
            return new XElement(CAMLQuery.Lt, fieldRef, value);
        }

        public static XElement Leq(XElement fieldRef, XElement value)
        {
            return new XElement(CAMLQuery.Leq, fieldRef, value);
        }

        public static XElement Geq(XElement fieldRef, XElement value)
        {
            return new XElement(CAMLQuery.Geq, fieldRef, value);
        }

        public static XElement And(XElement eq1, XElement eq2)
        {
            return new XElement(CAMLQuery.And, eq1, eq2);
        }

        public static XElement Or(XElement eq1, XElement eq2)
        {
            return new XElement(CAMLQuery.Or, eq1, eq2);
        }

        public static XElement IsNull(XElement fieldRef)
        {
            return new XElement(CAMLQuery.IsNull, fieldRef);
        }

        public static XElement Membership(string type, XElement fieldRef)
        {
            return new XElement(CAMLQuery.Membership, new XAttribute(CAMLQuery.Type, type), fieldRef);
        }

        public static string Where(params XElement[] clause)
        {
            XElement xquery = new XElement(CAMLQuery.Where, clause);

            return xquery.ToString();
        }

        public static string OrderBy(XElement fieldRef)
        {
            XElement xorderby = new XElement(CAMLQuery.OrderBy, fieldRef);
            return xorderby.ToString();
        }

        public static string QueryWithOrderBy(XElement fieldRef, params XElement[] clause)
        {
            XElement xquery = new XElement(CAMLQuery.Where, clause);
            XElement xorderby = new XElement(CAMLQuery.OrderBy, fieldRef);
            return xquery.ToString() + xorderby.ToString();
        }


    }
}
