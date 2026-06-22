#!/bin/bash
mysql -u root -p1234 -e "CREATE DATABASE IF NOT EXISTS daejeon_construction CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;"
cd DaejeonConstruction.Web
sed -i 's/YOUR_PASSWORD/1234/g' appsettings.json
dotnet restore
echo "세팅 완료!"