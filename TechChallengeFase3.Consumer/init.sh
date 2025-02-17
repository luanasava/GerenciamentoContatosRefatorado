#!/bin/bash

# Esperar o SQL Server estar disponível
echo "Aguardando o SQL Server iniciar..."
until /opt/mssql-tools/bin/sqlcmd -S sqlserver -U sa -P "YourStrongPassword!123" -Q "SELECT 1" &>/dev/null
do
  sleep 1
done

# Rodando as migrations
echo "Executando migrations..."
dotnet ef database update

# Iniciar a API
echo "Iniciando a API..."
dotnet Api1.dll