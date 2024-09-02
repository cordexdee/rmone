using System;


namespace uGovernIT.Utility.Entities.DMSDB
{
    public class FilesDirectories
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ? ParentID { get; set; }
        public string FullPath { get; set; }
        public int Size { get; set; }
        public bool IsCheckedOut { get; set; }
        public string AuthorId { get; set; }
        public bool IsFolder { get; set; }
        public bool IsFile { get; set; }
        public string Version { get; set; }
        public bool readAccess { get; set; }
        public bool writeAccess { get; set; }
        public DateTime ? CreatedOn { get; set; }
        public DateTime ? UpdatdedOn { get; set; }
        public string AutherName { get; set; }
        public string DocumentControlID { get; set; }
    }
}