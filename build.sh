#!/bin/bash

dotnet build -c "13"
dotnet build -c "14"
dotnet build -c "15"
dotnet build -c "16"
dotnet msbuild -target:CopyModFilesToModOutput