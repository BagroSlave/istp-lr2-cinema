# ІСтаТП · Лабораторна робота 2 — Кінотека (ASP.NET Core Web API)

**Виконавець:** Костащук Ярослав Васильович, група К26 · **Викладач:** Панченко Т. В.

REST Web API «Кінотека» (фільми та режисери) + клієнт на чистому JavaScript. Домен відрізняється від ЛР1.

## 🛠 Технології
ASP.NET Core Web API · EF Core (Code-First) · PostgreSQL (Npgsql) · Swagger · xUnit · JavaScript (fetch) · Docker.

## ▶️ Запуск
`docker compose up --build` → http://localhost:8080 (клієнт), `/swagger` (API).
Локально: `docker start istp-postgres`, потім `cd app && dotnet run`. Тести: `cd tests && dotnet test`.

## 🗂 Предметна область
Кінотека. Сутності: **Director** (1) → **Movie** (N).

## 📋 Етапи
### Етап 2.0 — Діаграма прецедентів (Use-case)
Формулювання завдання та діаграма use-case предметної області (узгоджено з викладачем).
![Use-case](docs/usecase_cinema.png)

### Етап 2.1 — Web API + модель + контекст (Code-First) + Docker
Проєкт Web API; моделі `Director`/`Movie`; контекст `MovieContext` (EF Core, Code-First, PostgreSQL); перша міграція.
Підтримка **Docker** (Dockerfile + docker-compose) — додатковий бал.
![ER](docs/er_cinema.png)

### Етап 2.2 — Контролери та їх тестування
REST-контролери `MoviesController`/`DirectorsController` (GET/POST/PUT/DELETE); ручне тестування через **Swagger** (`/swagger`).

### Етап 2.3 — Виклик Web API через JavaScript
Клієнт `wwwroot/index.html` + `app.js` (fetch): каталог фільмів, постери, селектор року, режисер вільним вводом.

### Етап 2.4 — Unit-тестування
**xUnit** + EF Core **InMemory**: тести контролерів (GET усі/за id, POST, DELETE). Запуск: `cd tests && dotnet test`.

## 🛡 Захист (коротко)
- **Web API** повертає JSON; **REST** = ресурси + HTTP-методи + статуси (200/201/204/404).
- **Code-First**: модель → міграції → `Database.Migrate()`.
- **`[NotMapped] DirectorName`** — ввід режисера текстом (знайти-або-створити).
- **Unit-тести** — на EF InMemory, без реальної БД.

> PDF діаграм: `docs/Діаграми_ЛР2.pdf`
