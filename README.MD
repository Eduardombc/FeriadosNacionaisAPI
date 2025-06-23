Feriados Nacionais API

Este é um projeto de treino, afim de praticar um pouco de C# e usando algumas tecnologias e passar por fundamentos básicos.

Tecnologias usadas: .NET, ASP .NET Core Web API, Entity Framework Core, SQLite, xUnit, Moq.

Uma API .NET que consome dados de feriados de uma API pública e os armazena em um cache local com SQLite para otimizar a performance, onde é feita um pequena consulta em cache, antes de fazer uma requisitação a API.
Fazendo com que diminua a latência de resposta e diminua o número de requisições ao banco, evitando sobrecarga desnecessária.

#Eduardombc