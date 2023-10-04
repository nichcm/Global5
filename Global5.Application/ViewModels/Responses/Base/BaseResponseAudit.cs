using System;

namespace Global5.Application.ViewModels.Responses.Base
{
    public class BaseResponseAudit
    {
        public string Active { get; set; }
        public int DataSource { get; set; }
        public DateTimeOffset Created { get; set; }
        public string CreateBy { get; set; }
        public DateTimeOffset? Updated { get; set; }
        public string UpdateBy { get; set; }
        public DateTimeOffset? Deleted { get; set; }
        public string DeleteBy { get; set; }
    }
}