namespace Global5.Infra.Data.Queries
{
    public class VehicleBrandQuery
    {
        public const string SelectIntegrationRequestById = @"exec [SelectIntegrationRequestById]
                                                                                                @IntegrationRequestId";

        public const string UpdateIntegrationRequest = @"exec [UpdateIntegrationRequest]
                                                                                                 @RequestJson
                                                                                                ,@StatusId
                                                                                                ,@StatusDescription
                                                                                                ,@TypeIntegrationRequestId
                                                                                                ,@DataSourceId
                                                                                                ,@CPRId
                                                                                                ,@Id_Status
                                                                                                ,@Created
                                                                                                ,@CreateBy";



        public const string SelectIntegrationRequestByTypeIntegrationRequestId = @"exec [SelectIntegrationRequestByTypeIntegrationRequestId]
                                                                                                                                            @TypeIntegrationRequestId";


        public const string SelectServiceHistoryByCPRId = @"exec [SelectServiceHistoryByCPRId]
                                                                                                @CPRId";

    }
}