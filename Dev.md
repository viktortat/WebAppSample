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


```
cd docker-cmp/
docker-compose -f docker-compose.yml -f docker-compose.pg.yml -f docker-compose.plus.yml up -d 
docker-compose -f docker-compose.yml -f docker-compose.pg.yml -f docker-compose.plus.yml down 
```

make -B help
