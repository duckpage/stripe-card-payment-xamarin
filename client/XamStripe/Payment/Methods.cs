using System;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;


using Xamarin.Forms;

using Stripe;


namespace XamStripe.Payment
{

    public class Methods
    {
        static string BackendUrl = "http://127.0.0.1:4242";

        public static async Task<StripeBackendResponse> StartCheckout()
        {
            // Create a PaymentIntent as soon as the view loads

            var jsonContent = new StringContent(JsonSerializer.Serialize(new Models.ItemsRequest { Items = new List<Models.Item> { new Models.Item { ID = "shoose" } } }), Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync(string.Format("{0}/create-payment-intent", BackendUrl), jsonContent);
                return JsonSerializer.Deserialize<StripeBackendResponse>(await response.Content.ReadAsStringAsync());
            }
        }

        public static async Task Pay(StripeBackendResponse intent, PaymentMethodCardCreateOptions card)
        {
            try
            {
                var Client = new StripeClient("pk_test_TYooMQauvdEDq54NiTphI7jx");

                var payIntent = new PaymentIntentService(Client);
                var payResult = await payIntent.ConfirmAsync(intent.IntentID, new PaymentIntentConfirmOptions
                {
                    ClientSecret = intent.ClientSecret,
                    PaymentMethodData = new PaymentIntentPaymentMethodDataOptions
                    {
                        Type = "card",
                        Card = card,
                        BillingDetails = new BillingDetailsOptions
                        {
                            // Add Extra Info
                            Name = "User Full Name",
                        }
                    },
                    ReturnUrl = string.Format("{0}/result-payment", BackendUrl) // Change this with your Return URL
                });

                if (payResult.NextAction != null)
                {
                    if (payResult.NextAction.Type == "redirect_to_url")
                    {
                        var webView = new WebView
                        {
                            Source = new UrlWebViewSource { Url = payResult.NextAction.RedirectToUrl.Url }
                        };

                        webView.Navigating += (s, e) =>
                        {
                            if (e.Url.StartsWith("close://"))
                            {
                                e.Cancel = true;

                                Xamarin.Forms.Application.Current.MainPage.Navigation.PopModalAsync();
                            }   
                        };

                        await Xamarin.Forms.Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ContentPage
                        {
                            Title = "3D Secure",
                            Content = webView
                        }));

                    }
                    else if (payResult.NextAction.Type == "use_stripe_sdk")
                    {
                        await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Stripe", "Use Stripe SDK", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
