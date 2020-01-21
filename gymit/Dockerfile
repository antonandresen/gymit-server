FROM mcr.microsoft.com/dotnet/core/sdk:3.1 as build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0

COPY gymit.csproj /build/

RUN dotnet restore ./build/gymit.csproj

COPY . ./build
WORKDIR /build/
RUN dotnet publish ./gymit.csproj -c $BUILDCONFIG -o out /p:Version=$VERSION

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
EXPOSE 80
COPY --from=build /build/out .

ENTRYPOINT ["dotnet", "gymit.dll"]