#!/bin/bash-eux

# Set up environment variables

# Install MAUI workloads
dotnet workload install maui
dotnet workload install maui-android
dotnet workload install maui-ios
dotnet workload install maccatalyst

# Restore project dependencies
dotnet restore

# Build the project
dotnet build --no-restore

# Run tests
dotnet test --no-build --verbosity normal --framework net6.0-windows

# Publish
dotnet publish --no-build --configuration Release --output ./publish

