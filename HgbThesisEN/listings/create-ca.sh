#!/bin/bash

password=$1
subject=$2

# Create private key
openssl genrsa -aes256 -passout pass:"$password" -out ca_private_key.pem 4096

# Create public key
openssl rsa -in ca_private_key.pem -passin pass:"$password" -pubout > ca_public_key.pem

# Create public key certificate
openssl req -new -passin pass:"$password" -key ca_private_key.pem -x509 -days 365 -out ca_cert.pem -subj "/CN=$subject"
