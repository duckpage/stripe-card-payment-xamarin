#! /usr/bin/env python3.6
"""
Python 3.6 or newer required.
"""
import json
import os
import stripe
# This is a sample test API key. Sign in to see examples pre-filled with your key.
stripe.api_key = "sk_test_4eC39HqLyjWDarjtT1zdp7dc"


from flask import Flask, render_template, jsonify, request


app = Flask(__name__, static_folder=".",
            static_url_path="", template_folder=".")


def calculate_order_amount(items):
    # Replace this constant with a calculation of the order's amount
    # Calculate the order total on the server to prevent
    # people from directly manipulating the amount on the client
    return 1400


@app.route('/create-payment-intent', methods=['POST'])
def create_payment():
    try:
        data = json.loads(request.data)
        intent = stripe.PaymentIntent.create(
            amount=calculate_order_amount(data['items']),
            currency='usd'
        )

        return jsonify({
          'clientSecret': intent['client_secret']
        })
    except Exception as e:
        return jsonify(error=str(e)), 403

@app.route('/result-payment', methods=['GET'])
def result_payment():
    return "<!doctype html><html lang='en'> <head> <meta charset='utf-8'> <meta name='viewport' content='width=device-width, initial-scale=1, shrink-to-fit=no'> <link rel='stylesheet' href='https://stackpath.bootstrapcdn.com/bootstrap/4.3.1/css/bootstrap.min.css'> <title>Return Url</title> </head> <body> <div class='container'> <div class='row p-5 text-center'> <div class='col-md-12'> <h1>Your Server</h1> <p> This is your return url where you can inform user about payment result. <br><br>You can now close this window. </p></div><a class='btn btn-primary text-white col-md-12 mt-5' href='close://'>Close</a> </div></div></body></html>"

if __name__ == '__main__':
    app.run()