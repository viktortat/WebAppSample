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
----
# Работа c докером для WebAppApi
```sh
docker build -f src/Dockerfile_WebAppApi -t devtest src/.
docker run --rm -it -p 443:443/tcp -p 8080:80 devtest
http://localhost:8080/
http://localhost:8080/WeatherForecast
```
## Отправка его не GitLab
```
docker login registry.gitlab.com
docker build -f src/Dockerfile_WebAppApi -t registry.gitlab.com/viktortat/appcore50/WebAppApi src/.
docker push registry.gitlab.com/viktortat/appcore50/WebAppApi:001
docker run --rm -it -p 443:443/tcp -p 8080:80 registry.gitlab.com/viktortat/appcore50/WebAppApi:001  
http://localhost:8080/  
```

```
cd docker-cmp/
docker-compose -f docker-compose.yml -f docker-compose.pg.yml -f docker-compose.plus.yml up -d 
docker-compose -f docker-compose.yml -f docker-compose.pg.yml -f docker-compose.plus.yml down 
```

make -B help