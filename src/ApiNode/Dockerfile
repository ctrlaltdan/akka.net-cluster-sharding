FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app

# should be a comma-delimited list
ENV CLUSTER_SEEDS "[]"
ENV CLUSTER_IP ""
ENV CLUSTER_PORT "2552"

#Akka.Remote inbound listening endpoint
EXPOSE 2552 

FROM microsoft/dotnet:2.1-sdk AS src
WORKDIR /src
COPY . .
WORKDIR /src/src
RUN dotnet restore

FROM src AS build
WORKDIR /src/src/ApiNode
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /src/get-dockerip.sh ./get-dockerip.sh
COPY --from=publish /app .

ENTRYPOINT ["/bin/bash","get-dockerip.sh"]

CMD ["dotnet", "ApiNode.dll"]
