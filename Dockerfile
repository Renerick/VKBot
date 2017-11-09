FROM microsoft/dotnet:latest

WORKDIR /app
ADD . .
ENTRYPOINT [ "dotnet", "VKBot.dll" ]