cd ./src/Deploy/Postgres
docker-compose -f docker-compose_pg.yml up -d
# stop
docker-compose -f docker-compose_pg.yml down 
docker-compose -f docker-compose_pg.yml down -v
 
http://localhost:8020/#/containers

http://localhost:8015/browser/
admin@admin.net
Password12!
