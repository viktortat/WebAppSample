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
