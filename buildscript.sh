#!/bin/bash -eux

# Set up environment variables

# Restore project dependencies
dotnet restore --ignore-failed-sources

# Build the project
# Exclude the iOS target if the current system is not macOS
if [[ "$OSTYPE" == "darwin"* ]]; then
  dotnet build --no-restore
else
  dotnet build --no-restore --framework net6.0-android
fi

# Run tests
dotnet test --no-build --verbosity normal --framework net6.0-android

# Publish
dotnet publish --no-build --configuration Release --output ./publish


