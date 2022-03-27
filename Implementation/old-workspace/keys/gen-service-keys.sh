#!/bin/bash

passwordService=$1
passwordCA=$2
subject=$3

# Generate private key
openssl genrsa -aes256 -passout pass:"$passwordService" -out service.key 4096

# Create certificate signing request
openssl req -passin pass:"$passwordService" -new -key service.key -out service.csr -subj "/CN=$subject"

# Sign certificate by CA
openssl x509 -req -passin pass:"$passwordCA" -days 30 -in service.csr -CA ca.crt -CAkey ca.key -set_serial -01 -out service.crt

# Create PFX file, which is later used in the C# Program
openssl pkcs12 -export -out service.pfx -passin pass:"$passwordService" -inkey service.key -in service.crt -passout pass:"$passwordService"
