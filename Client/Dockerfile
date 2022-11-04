#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

# update system
RUN apt-get update -y && apt-get upgrade -y

# dotnet specific env vars, default to development environment
#ENV ASPNETCORE_ENVIRONMENT=Development
#ENV ASPNETCORE_URLS=http://+:80;https://+:443

# dotnet kestrel env vars
#ENV Kestrel:Certificates:Default:Path=/etc/ssl/private/cert.pfx
#ENV Kestrel:Certificates:Default:Password=changeit
#ENV Kestrel:Certificates:Default:AllowInvalid=true
#ENV Kestrel:EndPointDefaults:Protocols=Http1AndHttp2

# copy certificate authority certs from local file system
#ARG CA_KEY=rootCA-key.pem
#ARG CA_CERT=rootCA.pem
#ARG DOMAINS='host.docker.internal localhost 127.0.0.1 ::1'

# default ca cert location (mkcert)
#COPY ${CA_KEY} /root/.local/share/mkcert/rootCA-key.pem
#COPY ${CA_CERT} /root/.local/share/mkcert/rootCA.pem

# install CA and SSL cert
#RUN apt-get install curl -y && \
#	curl -L https://github.com/FiloSottile/mkcert/releases/download/v1.4.3/mkcert-v1.4.3-linux-amd64 > /usr/local/bin/mkcert && \
#	chmod +x /usr/local/bin/mkcert
#RUN mkcert -install
#RUN mkcert -p12-file /etc/ssl/private/cert.pfx -pkcs12 $DOMAINS

WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Client/Client.csproj", "Client/"]
RUN dotnet restore "Client/Client.csproj"
COPY . .
WORKDIR "/src/Client"
RUN dotnet build "Client.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Client.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Client.dll"]