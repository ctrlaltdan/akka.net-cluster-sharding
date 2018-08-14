# Akka.net Cluster Sharding Example

The following example demonstrates two services communicating with eachother in a sharded cluster model using the Akka.net modelling system.

By default state is persisted to PostgreSql using the connection string `User ID=example;Password=example;Host=localhost;Port=5433;Database=example;`

The following `docker-compose` will install PostgreSql locally into your docker machine.

```
---
version: '3'
services:
  postgres:
    image: postgres:9.4
    environment:
      POSTGRES_USER: example
      POSTGRES_PASSWORD: example
    ports: ['127.0.0.1:5433:5432']
    volumes:
      - ./init.sql:/docker-entrypoint-initdb.d/init.sql
      
networks: {stack: {}}
```