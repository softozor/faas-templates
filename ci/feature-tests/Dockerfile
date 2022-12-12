ARG PYTHON_VERSION=3.9
ARG DOCKER_REGISTRY=""

FROM ${DOCKER_REGISTRY}python:${PYTHON_VERSION}-alpine AS build

ARG INDEX_URL
ARG FAAS_CLI_VERSION

ENV PATH=/venv/bin:$PATH

COPY ./requirements.txt .

# git is necessary as long as we clone behave's git repo
RUN apk add --update curl git \
  && curl -sSL https://github.com/openfaas/faas-cli/releases/download/${FAAS_CLI_VERSION}/faas-cli --output /usr/local/bin/faas-cli \
  && chmod u+x /usr/local/bin/faas-cli \
  && python -m venv /venv \
  && pip install -i $INDEX_URL --no-cache-dir -r requirements.txt

FROM ${DOCKER_REGISTRY}docker:latest

ENV PATH=/venv/bin:$PATH

RUN apk add --update git

COPY --from=build /usr/local/bin/faas-cli /usr/local/bin/faas-cli
COPY --from=build /venv /venv
COPY --from=build /usr/local/bin/faas-cli /usr/local/bin/faas-cli

WORKDIR /app
