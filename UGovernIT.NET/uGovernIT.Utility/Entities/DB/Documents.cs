using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace uGovernIT.Utility.Entities
{
    [Table(DatabaseObjects.Tables.Documents)]
    public class Document:DBBaseEntity
    {
        public long Id { get; set; }
        public string FileID { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public long Size { get; set; }
        public string ContentType { get; set; }
        public byte[] Blob { get; set; }

       
    }

}
