#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base

#RUN chmod 644 /usr/local/share/ca-certificates/cert.pfx && update-ca-certificates
#RUN update-ca-certificates

RUN apt-get update -y && apt-get upgrade -y
RUN apt-get install curl -y && \
	curl -L https://github.com/FiloSottile/mkcert/releases/download/v1.4.4/mkcert-v1.4.4-linux-amd64 > /usr/local/bin/mkcert && \
	chmod +x /usr/local/bin/mkcert
RUN mkcert -install >> /tmp/caroot.log
RUN mkcert -CAROOT >> /tmp/caroot.log
RUN cp /root/.local/share/mkcert/rootCA.pem /usr/local/share/ca-certificates/rootCA.crt
RUN chmod 644 /usr/local/share/ca-certificates/rootCA.crt && update-ca-certificates >> /tmp/caroot.log
WORKDIR /https/
RUN mkcert -p12-file cert.pfx -pkcs12 host.docker.internal localhost 127.0.0.1 ::1
RUN cp /https/cert.pfx /etc/ssl/private/cert.pfx

WORKDIR /app
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Identity/Identity.csproj", "Identity/"]
RUN dotnet restore "Identity/Identity.csproj"
COPY . .
WORKDIR "/src/Identity"
RUN dotnet build "Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Identity.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity.dll"]