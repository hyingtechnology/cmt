using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace cmt.DTOs
{
    public class FileDTO
    {
        public string Id { get; set; }
        public HttpPostedFileBase File { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Type { get; set; }
        public string ContentType { get; set; }
    }
}