FROM python:3.9-slim AS build

ARG FAAS_CLI_VERSION

COPY ./requirements.txt .

RUN apt update && apt install -y gcc wget git curl \
  && curl -sSL https://github.com/openfaas/faas-cli/releases/download/${FAAS_CLI_VERSION}/faas-cli --output /usr/local/bin/faas-cli \
  && chmod u+x /usr/local/bin/faas-cli \
  && pip install --no-cache-dir -r requirements.txt

WORKDIR /app