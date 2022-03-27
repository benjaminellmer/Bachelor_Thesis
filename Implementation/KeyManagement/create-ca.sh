#!/bin/bash

password=$1
subject=$2

# Create private key
openssl genrsa -aes256 -passout pass:"$password" -out ca.key 4096

# Create public key certificate
openssl req -new -passin pass:"$password" -key ca.key -x509 -days 365 -out ca.crt -subj "/CN=$subject"
