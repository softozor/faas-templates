FROM openfaas/of-watchdog:0.8.2 AS watchdog

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine-amd64 AS builder

ARG FUNCTION_NAME
ENV FUNCTION_NAME=${FUNCTION_NAME}
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1
WORKDIR /src/function
COPY ./function/$FUNCTION_NAME.csproj  ./Function.csproj
WORKDIR /src
COPY ./root.csproj  .
RUN dotnet nuget add source https://gitlab.hidora.com/api/v4/projects/198/packages/nuget/index.json --name "gitlab-softozor-packages" \
    && dotnet restore
COPY . .
RUN test -f ./function/appsettings.json && mv ./function/appsettings.json .
RUN dotnet publish -c release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine-amd64

COPY --from=watchdog /fwatchdog /usr/bin/fwatchdog

RUN addgroup --system app \
    && adduser --system --ingroup app app

WORKDIR /app
COPY --from=builder /app .
RUN chown app:app -R /app

USER app

ENV ASPNETCORE_URLS "http://+:5000"

ENV fprocess="dotnet ./root.dll"
ENV cgi_headers="true"
ENV mode="http"
ENV upstream_url="http://localhost:5000"
EXPOSE 8080

HEALTHCHECK --interval=3s CMD [ -e /tmp/.lock ] || exit 1

CMD ["fwatchdog"]
