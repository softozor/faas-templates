FROM openfaas/of-watchdog:0.8.2 AS watchdog

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine-amd64 AS builder

ARG FUNCTION_NAME
ENV FUNCTION_NAME=${FUNCTION_NAME}
# Supress collection of data.
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1
# Optimize for Docker builder caching by adding projects first.
WORKDIR /src/function
COPY ./function/$FUNCTION_NAME.csproj  ./Function.csproj
WORKDIR /src
COPY ./root.csproj  .
RUN dotnet nuget add source http://gitlab.hidora.com/api/v4/projects/198/packages/nuget/index.json --name "gitlab-softozor-packages" \
    && dotnet restore
COPY . .
RUN dotnet publish -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine-amd64 

ARG FUNCTION_NAME
ENV FUNCTION_NAME=${FUNCTION_NAME}

COPY --from=watchdog /fwatchdog /usr/bin/fwatchdog

WORKDIR /app
COPY --from=builder /app .

ENV fprocess="dotnet ./${FUNCTION_NAME}.dll"
ENV cgi_headers="true"
ENV mode="http"
ENV upstream_url="http://localhost:80"
EXPOSE 8080

HEALTHCHECK --interval=3s CMD [ -e /tmp/.lock ] || exit 1

CMD ["fwatchdog"]