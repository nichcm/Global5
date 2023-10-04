using System;

namespace Global5.Domain.Entities
{

    public class LogRegistration : BaseAudit
    {
        public LogRegistration(DateTime changeDate, int idUser, string modifiedField, string oldValue, string newValue)
        {
            ChangeDate = changeDate;
            IdUser = idUser;
            ModifiedField = modifiedField;
            OldValue = oldValue;
            NewValue = newValue;
        }
        public int Id { get; set; }
        public DateTime ChangeDate { get; set; }
        public int IdUser { get; set; }
        public string ModifiedField { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
    }
}
