<h1 align="center">Welcome to RecruitmentAPI 👋</h1>
<p>
</p>

> API de recrutement
<p># RecruitmentAPI

Une API REST développée en ASP.NET Core pour la gestion de candidatures et d'offres d'emploi.

## 🔧 Fonctionnalités

- Authentification avec JWT
- Gestion des rôles : `Candidat` et `Recruteur`
- Inscription et connexion sécurisées avec mot de passe hashé
- Architecture MVC + Entity Framework Core
- Tests unitaires avec xUnit & TDD (Test Driven Development)

## 🚀 Endpoints principaux

| Méthode | URL                   | Rôle requis | Description                       |
|---------|-----------------------|-------------|-----------------------------------|
| POST    | /api/auth/register    | Public      | Inscription utilisateur           |
| POST    | /api/auth/login       | Public      | Connexion et génération de token  |

## ⚙️ Stack technique

- ASP.NET Core 7
- Entity Framework Core + InMemoryDb (test)
- JWT Bearer Auth
- xUnit + Moq
- PostgreSQL (prévu)
- Swagger

## ✅ Lancer le projet

```bash
dotnet run
</p>
### ✨ [Demo](http://localhost:5114/swagger/index.html)

## Install

```sh
dotnet run
```

## Usage

```sh
# RecruitmentAPI
```

## Run tests

```sh
dotnet test
```

## Author

👤 **@Mix208**

* Github: [@Mix208](https://github.com/Mix208)

## Show your support

Give a ⭐️ if this project helped you!

***
_This README was generated with ❤️ by [readme-md-generator](https://github.com/kefranabg/readme-md-generator)_
