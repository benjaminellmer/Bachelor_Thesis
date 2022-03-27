#!/bin/bash

passwordService=$1
passwordCA=$2
subject=$3

# Generate private key
openssl genrsa -aes256 -passout pass:"$passwordService" -out service_private_key.pem

# Create certificate signing request
openssl req -passin pass:"$passwordService" -new -key service_private_key.pem -out service.csr -subj "/CN=$subject"

# Sign certificate by CA
openssl x509 -req -passin pass:"$passwordCA" -days 0 -in service.csr -CA ../ca-keys/ca.crt -CAkey ../ca-keys/ca.key -set_serial -01 -out service_cert.pem
