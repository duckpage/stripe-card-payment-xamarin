using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

using Xamarin.Forms;

namespace XamStripe
{
    public partial class CheckoutPage : ContentPage
    {
        Payment.StripeBackendResponse PaymentIntentResponse;

        public CheckoutPage()
        {
            InitializeComponent();

            PayButton.Clicked += async delegate
            {
                if (string.IsNullOrEmpty(CardNumber.Text) && string.IsNullOrEmpty(MonthYear.Text) && string.IsNullOrEmpty(CVC.Text))
                {
                    await DisplayAlert("Missing Data", "Please add valid credit data.", "OK");
                    return;
                }

                await CardLayout.FadeTo(0);
                CardLayout.IsVisible = false;
                Loader.IsVisible = true;
                await Loader.FadeTo(1);

                await Payment.Methods.Pay(PaymentIntentResponse, new Stripe.PaymentMethodCardCreateOptions
                {
                    Number = CardNumber.Text.Replace("/", string.Empty),
                    ExpMonth = Convert.ToInt64(MonthYear.Text.Split('/')[0]),
                    ExpYear = Convert.ToInt64(MonthYear.Text.Split('/')[1]),
                    Cvc = CVC.Text
                });

                await Loader.FadeTo(0);
                Loader.IsVisible = false;

                CardNumber.Text = string.Empty;
                MonthYear.Text = string.Empty;
                CVC.Text = string.Empty;

                CardLayout.IsVisible = true;
                await CardLayout.FadeTo(1);

            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            _ = InitializeIntent();
        }


        async Task InitializeIntent()
        {
            PaymentIntentResponse = await Payment.Methods.StartCheckout();
        }

    }
}
