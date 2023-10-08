[Создание веб-API с помощью контроллеров ASP.NET Core](https://learn.microsoft.com/ru-ru/training/modules/build-web-api-aspnet-core)

dotnet new webapi -o PizzaWebApi -f net6.0
cd PizzaWebApi/
dotnet run

https://localhost:7101/weatherforecast


dotnet dev-certs https --trust
dotnet tool install -g Microsoft.dotnet-httprepl
```sh
httprepl https://localhost:5101
connect https://localhost:5101
ls
get WeatherForecast
exit
```
---

Работа с тестами
```sh
set ASPNETCORE_ENVIRONMENT=Staging
dotnet run --project src/PizzaWeb.Api/PizzaWeb.Api.csproj --no-launch-profile
https://localhost:5101/swagger/index.html

# Запуск тестов
cd playwright_tests/
npx playwright test tests/PizzaWeb_Api.spec.ts
```

[Tetss - help](playwright_tests/Dev.md)

[youtube.com/@letcode](https://www.youtube.com/@letcode/playlists)
[lambdatest.com/learning-hub/how-to-playwright](https://www.lambdatest.com/learning-hub/how-to-install-playwright)
[github.com/ortoniKC/Playwright-Test-Runner](https://github.com/ortoniKC/Playwright-Test-Runner/blob/main/apitest/service-now.test.ts)
## Запуск
```sh
# docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d --build --force-recreate --remove-orphans
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
# docker-compose -f docker-compose.yml -f docker-compose.override.yml down

http://localhost:5000/api/users
```
