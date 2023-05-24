#!/bin/bash -eux

# Set up environment variables

# Restore project dependencies
dotnet restore --ignore-failed-sources

# Build the project
dotnet build --no-restore --filter "-Category=Microsoft.Maui"

# Run tests
dotnet test --no-build --verbosity normal --framework net6.0-windows

# Publish
dotnet publish --no-build --configuration Release --output ./publish

