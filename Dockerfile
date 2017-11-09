FROM microsoft/dotnet:runtime

WORKDIR /app
ADD . .
ENTRYPOINT [ "dotnet", "VKBot.dll" ]