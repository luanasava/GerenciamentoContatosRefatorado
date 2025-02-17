#!/bin/sh
# Aguarda até o RabbitMQ estar disponível
until nc -z -v -w30 rabbitmq 5672
do
  echo "Esperando o RabbitMQ iniciar..."
  sleep 5
done
echo "RabbitMQ está disponível!"
exec "$@"