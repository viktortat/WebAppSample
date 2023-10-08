[Создание веб-API с помощью контроллеров ASP.NET Core](https://learn.microsoft.com/ru-ru/training/modules/build-web-api-aspnet-core)

dotnet new webapi -o WebApp.Api -f net6.0
cd WebApp.Api/
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

Запуск playwright тестів
```sh
# set ASPNETCORE_ENVIRONMENT=Staging
echo $ASPNETCORE_ENVIRONMENT
export ASPNETCORE_ENVIRONMENT=Development && dotnet run --project src/PizzaWeb.Api/PizzaWeb.Api.csproj --no-launch-profile
https://localhost:5001/swagger/index.html

# Запуск тестів
# echo $MY_BASE_URL
# Якщо потрибно встановлемо Змінні оточення
# unset ASPNETCORE_ENVIRONMENT
# export MY_BASE_URL=https://localhost:5001 && export ASPNETCORE_ENVIRONMENT=Development
cd src/playwright_tests && npx playwright test tests/PizzaWeb_Api.spec.ts
# Звіт
# npx playwright show-report
```
```sh
dotnet test -e ASPNETCORE_ENVIRONMENT=Development
# dotnet test -e ASPNETCORE_ENVIRONMENT=Development --logger trx
# dotnet test -t && dotnet test -e ASPNETCORE_ENVIRONMENT=Development --no-restore --no-build --nologo
# dotnet test -e ASPNETCORE_ENVIRONMENT=Development --no-restore --no-build --nologo --logger trx
```

```sh
# docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d --build --force-recreate --remove-orphans
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
```



[Tetss - help](playwright_tests/Dev.md)

[youtube.com/@letcode](https://www.youtube.com/@letcode/playlists)
[lambdatest.com/learning-hub/how-to-playwright](https://www.lambdatest.com/learning-hub/how-to-install-playwright)
[github.com/ortoniKC/Playwright-Test-Runner](https://github.com/ortoniKC/Playwright-Test-Runner/blob/main/apitest/service-now.test.ts)

```sh
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d

```
