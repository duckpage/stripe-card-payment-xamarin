# Stripe PaymentIntent with 3D Secure in Xamarin Forms

Build a simple Xamarin Froms checkout app to collect card details. Included are some basic build and run scripts you can use to start up the application.

## Running the sample

1. Run the server

    **For Python**
    ```
    pip install stripe flask

    export FLASK_APP=server/python/server.py
    python3 -m flask run --port=4242

    ```
    **For GO**
    ```
    go get github.com/stripe/stripe-go

    go run server/go/server.go
    ```

2. Open project _client/XamStripe.sln_ in Visual Studio and run it.

