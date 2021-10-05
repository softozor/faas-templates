FROM python:3.9-slim AS build

ARG FAAS_CLI_VERSION
ARG PERSONAL_ACCESS_TOKEN
ARG CI_API_V4_URL
ARG FAAS_CLIENT_PYPI_PROJECT_ID

COPY ./requirements.txt .

RUN apt update && apt install -y gcc wget git curl \
  && curl -sSL https://github.com/openfaas/faas-cli/releases/download/${FAAS_CLI_VERSION}/faas-cli --output /usr/local/bin/faas-cli \
  && chmod u+x /usr/local/bin/faas-cli \
  && pip install --no-cache-dir -r requirements.txt \
  && pip install faas-client==1.1.0 --extra-index-url http://__token__:${PERSONAL_ACCESS_TOKEN}@${CI_API_V4_URL#http*://}/projects/${FAAS_CLIENT_PYPI_PROJECT_ID}/packages/pypi/simple --trusted-host gitlab.hidora.com

FROM python:3.9-slim

COPY --from=build /usr/local/bin/faas-cli /usr/local/bin/faas-cli
COPY --from=build /usr/local/lib/python3.9/site-packages /usr/local/lib/python3.9/site-packages
COPY --from=build /usr/local/bin /usr/local/bin
# this copy is necessary as long as we clone behave's git repo
COPY --from=build /src /src

WORKDIR /app