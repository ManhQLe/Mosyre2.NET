echo off
set package=%1
set key=%2

nuget push %package% %key% -Source https://api.nuget.org/v3/index.json