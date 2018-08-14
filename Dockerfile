FROM microsoft/dotnet:2.1-sdk as build-env
WORKDIR /app

COPY . ./
RUN dotnet restore Cineworld19.sln -s https://api.nuget.org/v3/index.json
RUN dotnet publish Cineworld.ConsoleApp/Cineworld.ConsoleApp.csproj -c Release -o /app/publish -r linux-x64


FROM microsoft/dotnet:2.1-runtime
WORKDIR /app
COPY --from=build-env app/publish .
ENTRYPOINT ["dotnet", "Cineworld.ConsoleApp.dll"]
