ARG BASE_IMAGE=mcr.microsoft.com/dotnet/aspnet:8.0
ARG BUILD_IMAGE=mcr.microsoft.com/dotnet/sdk:8.0


FROM ${BASE_IMAGE} AS base
WORKDIR /app

FROM  ${BUILD_IMAGE} AS build
WORKDIR /src
COPY . .

RUN dotnet restore "XResize/XResize.Bot.csproj"
WORKDIR "/src/XResize"
RUN dotnet build "XResize.Bot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "XResize.Bot.csproj" -c Release -o /app/publish

FROM base AS final

ENV TelegramBotToken=6867996261:AAETOAZCUBV2cBP8C-s26TDz91_Lzqz8cIA
ENV ResizerThreadCount=1
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "XResize.Bot.dll"]