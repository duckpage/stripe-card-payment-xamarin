using System.Text.Json.Serialization;

namespace XamStripe.Payment
{
    public class StripeBackendResponse
    {
        [JsonPropertyName("clientSecret")]
        public string ClientSecret { get; set; }

        public string IntentID
        {
            get
            {
                return string.Format("pi_{0}", ClientSecret.Split('_')[1]);
            }
        }
    }
}
