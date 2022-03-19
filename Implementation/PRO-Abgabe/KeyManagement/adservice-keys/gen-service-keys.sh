#!/bin/bash

passwordService=$1
passwordCA=$2
subject=$3
caPath=$4

if [ -z "$1" ]; then
    echo "Service Password is empty! Stopping execution!"
    exit 1
fi

if [ -z "$2" ]; then
    echo "CA Password is empty! Stopping execution!"
    exit 1
fi
if [ -z "$3" ]; then
    echo "Subject is empty! Stopping execution!"
    exit 1
fi
if [ -z "$4" ]; then
    echo "CA path ist empty! Assuming that the CA files are in the current folder!"
fi

# Generate private key
openssl genrsa -aes256 -passout pass:"$passwordService" -out service.key 4096

# Create certificate signing request
openssl req -passin pass:"$passwordService" -new -key service.key -out service.csr -subj "/CN=$subject"

# Sign certificate by CA
openssl x509 -req -passin pass:"$passwordCA" -days 30 -in service.csr -CA "$4ca.crt" -CAkey "$4ca.key" -set_serial -01 -out service.crt

# Create PFX file, which is later used in the C# Program
openssl pkcs12 -export -out service.pfx -passin pass:"$passwordService" -inkey service.key -in service.crt -passout pass:"$passwordService"
