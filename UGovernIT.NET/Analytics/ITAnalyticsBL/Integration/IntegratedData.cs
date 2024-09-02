using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITAnalyticsBL.Integration
{
    public struct IDInputParam
    {
        public string Listname;
        public string FieldName;
        public string Info;
    }

    public struct IDOutput
    {
        public string Info;
        public string FieldName;
        public string FieldValue;
    }
}
