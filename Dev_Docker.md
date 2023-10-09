# Работа c докером для WebAppApi
```sh
docker build -f src/WebApp/Dockerfile -t webapp src/WebApp/.
docker run --rm -it -d -p 8000:80 webapp
# docker run --rm -it -d -p 443:443/tcp -p 8000:80 webapp

docker build -f src/WebApp/Dockerfile_alpine -t webapp_alpine src/WebApp/.
docker run --rm -it -d -p 8000:80 webapp_alpine


docker build -f src/WebApp.Api/Dockerfile -t webappapi src/WebApp.Api/.
docker run --rm -it -d -p 8001:80 webappapi

docker build -f src/WebApp.Api/Dockerfile_alpine -t webappapi_alpine src/WebApp.Api/.
docker run --rm -it -d -p 8001:80 webappapi_alpine

docker images | grep webapp

http://localhost:8080/
http://localhost:8080/swagger
```
## Отправка его не GitLab
```
docker login registry.gitlab.com
docker build -f src/Dockerfile_WebAppApi -t registry.gitlab.com/viktortat/appcore70/WebAppApi src/.
docker push registry.gitlab.com/viktortat/appcore50/WebAppApi:001
docker run --rm -it -p 443:443/tcp -p 8080:80 registry.gitlab.com/viktortat/appcore70/WebAppApi:001  
http://localhost:8080/  
```
[ow to build smaller and secure Docker Images for .NET](https://www.thorsten-hans.com/how-to-build-smaller-and-secure-docker-images-for-net5/)  
[Using Azure Files in Kubernetes Deployments with ASP.NET Core](https://www.thorsten-hans.com/using-azure-files-in-kubernetes-deployments-with-asp-net-core/)
[6 Steps To Run .NET Core Apps In Azure Kubernetes](https://www.thorsten-hans.com/6-steps-to-run-netcore-apps-in-azure-kubernetes/)  
[Instrumenting .NET Apps with OpenTelemetry](https://www.thorsten-hans.com/instrumenting-dotnet-apps-with-opentelemetry/)  
[]()  
[]()  
