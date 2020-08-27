#! /bin/bash

if [ "$#" -ne "1" ] ; then 
  echo "Usage: $0 <nuget-api-key>"
  echo "Connect to https://www.nuget.org/users/account/LogOn and get your API KEY"
  exit 1
fi

API_KEY=$1

# clear current release builds
rm -Rf ./lib/HasuraHandling/bin/Release

# build all libs
dotnet build -c Release ./lib/HasuraHandling/HasuraHandling.csproj

# publish all libs
dotnet nuget push ./lib/HasuraHandling/bin/Release/Shopozor.HasuraHandling.*.nupkg --api-key $API_KEY --source https://api.nuget.org/v3/index.json