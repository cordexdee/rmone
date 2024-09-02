using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace uGovernIT.Utility.Entities.DMSDB
{
    public class DirectoryDetail
    {
        public string Name { get; set; }

        public string DirectorySize { get; set; }

        public int FileCount { get; set; }

        public string Owners { get; set; }

        public string Modified { get; set; }

        public int? DirectoryParentId { get; set; }

        public List<FilesDirectories> Files { get; set; }

        public DirectoryDetail()
        {
            Files = new List<FilesDirectories>();
        }
    }
}
