namespace Global5.Domain.Entities
{
    public class VehicleBrand : BaseAudit
    {

        #region Entity main

        public int Id { get; set; }
        public bool IsNational { get; set; }
        public bool Status { get; set; }
        public string BrandName { get; set; }

        #endregion
    }
}
