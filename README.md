# projectTaskApi
API для управления проектами и задачами, разработанное в рамках технического задания компании "Цифронит".

## Технологический стек
- ASP.NET Core 8
- PostgreSQL + Entity Framework Core
- Docker
- FluentValidation
- xUnit, Moq, InMemoryDatabase
- Serilog
- Redis

## Основные возможности проекта
- CRUD: для проектов и задач
- Пагинация и фильтрация списков 
- Кэширование get-запросов на получение проектов и задач
- Покрытие основных тестовых сценариев при помощи Unit-тестов
- Документация Swagger

<img width="1188" height="837" alt="image" src="https://github.com/user-attachments/assets/b086f987-19e5-4c45-97fd-71f4ffdf11b9" />


## Запуск проекта

1. Склонируйте репозиторий в удобное место:  
```
https://github.com/ObitoUtiha/project-task-api.git
```

2. Запустите контейнеры из корневой папки проекта:  
```docker-compose up --build```

### Документация Api
После запуска контейнера, вам будет доступен UI Swagger, в котором можно проводить все последующие операции с проектами и задачами.  
```
Swagger UI: http://localhost:5000/swagger
```

### Запуск тестов
Запуск тестов доступен из корневой папки по пути:  
```
\project-task-api\ProjectTaskApi.Tests  
```
Путем следующей комманды:  
```
dotnet test
```
