```
docker-compose -f docker-compose.yml -f docker-compose.pg.yml -f docker-compose.plus.yml up -d 
docker-compose -f docker-compose.yml -f docker-compose.pg.yml -f docker-compose.plus.yml down 
docker-compose -f docker-compose.yml -f docker-compose.pg.yml -f docker-compose.plus.yml down -v
```
```sh
# pgadmin
http://localhost:8015/browser/
# grafana
http://localhost:3000/login
# WebApp
http://localhost:8090/
# WebappApi
http://localhost:8080/
# zipkin
http://localhost:9412/zipkin/
# portainer
http://localhost:8020/#/init/admin
# prometheus
http://localhost:8081/graph
```