# MongoDB .NET Core Demo
Este é um projeto de API para Controle de Finanças Pessoais, desenvolvido em .NET Core 6. A API oferece funcionalidades intuitivas e fáceis de usar para o gerenciamento abrangente de finanças pessoais, incluindo controle de despesas, gestão de categorias, controle de contas bancárias e estatísticas simplificadas com gráficos.

## Azure Devops
https://dev.azure.com/henriquemachado1993

## Recursos
MongoDB Driver: Utiliza o driver oficial do MongoDB para .NET (MongoDB.Driver) para se conectar ao banco de dados MongoDB.

## Funcionalidades

- **Controle de Finanças Pessoais:**
- **Controle de Transações:**
- **Gestão de Categorias de Transações:**
- **Controle de Contas Bancárias:**
- **Estatísticas Simplificadas com Gráficos:**

## Pré-requisitos
.NET Core 6 SDK
Docker e Docker-Compose

## Como Usar
* Clone o repositório.
* Execute o MongoDB e o Adminer usando o Docker-Compose:
* Crie um arquivo docker-compose.yml em alguma pasta e preencha com as informações abaixo:
```
version: '3'

services:

  mongodb:
       image: mongo:5
       environment:
         - MONGO_INITDB_ROOT_USERNAME=user
         - MONGO_INITDB_ROOT_PASSWORD=password
         - MONGO_INITDB_DATABASE=financedb
       container_name: mongodb
       volumes:
         - dbdata:/data/db
       ports:
         - "27017:27017"

volumes:
  dbdata:
```
* bash
```
docker-compose up -d
```

Execute o projeto.
