#!/bin/bash

dotnet build -c V1_3
dotnet build -c V1_4
dotnet build -c V1_5
dotnet build -c V1_6
dotnet msbuild -target:CopyModFilesToModOutput