FROM openfaas/of-watchdog:0.8.1 AS watchdog

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine-amd64 AS builder

ARG DOTNET_OPTIMIZATION_OPTIONS
ARG FUNCTION_NAME
ENV FUNCTION_NAME=${FUNCTION_NAME}
# Supress collection of data.
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1
# Optimize for Docker builder caching by adding projects first.
WORKDIR /src/function
COPY ./function/$FUNCTION_NAME.csproj  ./Function.csproj
WORKDIR /src
COPY ./root.csproj  .
RUN dotnet restore
COPY . .
COPY ./function/appsettings.json .
RUN dotnet publish ${DOTNET_OPTIMIZATION_OPTIONS} -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine-amd64 

COPY --from=watchdog /fwatchdog /usr/bin/fwatchdog

WORKDIR /app
COPY --from=builder /app .

ENV fprocess="./root"
ENV cgi_headers="true"
ENV mode="http"
ENV upstream_url="http://localhost:80"
EXPOSE 8080

HEALTHCHECK --interval=3s CMD [ -e /tmp/.lock ] || exit 1

CMD ["fwatchdog"]