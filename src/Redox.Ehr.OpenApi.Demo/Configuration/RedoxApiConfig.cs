namespace Redox.DataModel.Builder.Configuration
{
    public class RedoxApiConfig
    {
        public string Key { get; set; }

        public string Secret { get; set; }

        public string BaseUrl { get; set; }

        public string AuthenticateSegment { get; set; }

        public string RefreshTokenSegment { get; set; }

        public string EndpointSegment { get; set; }

        public string QuerySegment { get; set; }

        // Subscriptions
        public string PatientSearch_Destination_ID { get; set; }

        public string PatientSearch_Destination_Name { get; set; }

        public string PatientAdmin_Destination_ID { get; set; }

        public string PatientAdmin_Destination_Name { get; set; }
        
        public string RedoxDestinationControllerPath { get; set; }
        
        public string RedoxDestinationVerificationSegment { get; set; }

        public string RedoxDestinationVerificationToken { get; set; }
    }
}