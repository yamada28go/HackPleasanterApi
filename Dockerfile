#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

# --- net core のビルド用定義

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
COPY ["Generator/", "/Generator/"]
WORKDIR /Generator
RUN dotnet restore "HackPleasanterApi.Generator.CodeGenerator/HackPleasanterApi.Generator.CodeGenerator.csproj"
RUN dotnet restore "HackPleasanterApi.Generator.JsonDefinitionExtractor/HackPleasanterApi.Generator.JsonDefinitionExtractor.csproj"

# ビルド
WORKDIR "/Generator/HackPleasanterApi.Generator.CodeGenerator"
RUN dotnet build "HackPleasanterApi.Generator.CodeGenerator.csproj" -c Release -o /app/build
WORKDIR "/Generator/HackPleasanterApi.Generator.JsonDefinitionExtractor"
RUN dotnet build "HackPleasanterApi.Generator.JsonDefinitionExtractor.csproj" -c Release -o /app/build


FROM build AS publish
WORKDIR "/Generator/HackPleasanterApi.Generator.CodeGenerator"
RUN dotnet publish "HackPleasanterApi.Generator.CodeGenerator.csproj" -c Release -o /app/publish
WORKDIR "/Generator/HackPleasanterApi.Generator.JsonDefinitionExtractor"
RUN dotnet publish "HackPleasanterApi.Generator.JsonDefinitionExtractor.csproj" -c Release -o /app/publish
WORKDIR /app/publish

