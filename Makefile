# Makefile for Docker
# include mssql.env

# MSSQL
DUMPS_DIR=./django-seed
container_db=sqlserver1
container_web=web
container_admin=sql

container_app = app
context_prj=src/EfDbContext/EfDbContext.csproj
api_prj=src/PRAGReport.Api/PRAGReport.Api.csproj

#############################################
help:
	@echo ""
	@echo "usage: make COMMAND"
	@echo ""
	@echo "COMMANDs:"
	@echo "  mig mname=<MigrateName>			создание миграции MigrateName" 
	@echo "  rm						удаление миграции" 
	@echo "  script					скрипт миграции" 
	@echo "  updb						обновление БД" 
	@echo "----------------------"
	@echo "Docker:"
	@echo "  start_all					всех докеров" 
	@echo "  start_app_pg					app, posgresql - докера" 
	@echo "  stop_all					остановка всех докеров" 
	@echo "  stop_elk					остановка локального ELK" 
	@echo "  start_rmq					запуск локального RabbitMq" 
	@echo "  stop_rmq					остановка локального RabbitMq" 
	@echo "  stop_rmq+					остановка локального RabbitMq с очисткой кеша" 
	@echo "----------------------"
	@echo "  stats						статистика загрузки докером" 
	@echo "  show						показ контейнеров" 
	@echo "  df						объемы докера" 

################ EF #####################
mig:
	@dotnet ef migrations add $(mname) -p $(context_prj) -s $(api_prj)

rm:
	@dotnet ef migrations remove -p $(context_prj) -s $(api_prj)

updb: 
	@dotnet ef database update -p $(context_prj) -s $(api_prj)

script:
	@dotnet ef migrations script -p $(context_prj) -s $(api_prj)

################ docker #####################
start_app_pg:
	docker-compose -f docker-cmp/docker-compose.yml -f docker-cmp/docker-compose.pg.yml up -d 

start_all:
	docker-compose -f docker-cmp/docker-compose.yml -f docker-cmp/docker-compose.pg.yml -f docker-cmp/docker-compose.plus.yml up -d 
stop_all:
	docker-compose -f docker-cmp/docker-compose.yml -f docker-cmp/docker-compose.yml -f docker-cmp/docker-compose.pg.yml -f docker-cmp/docker-compose.plus.yml down

start_elk:
	docker-compose -f dockers/elk/docker-compose.yml up -d  
stop_elk:
	docker-compose -f dockerss/elk/docker-compose.yml down -v

start_rmq:
	docker-compose -f dockers/rabbitmq/docker-compose_rmq.yml up -d  	
stop_rmq:
	docker-compose -f dockers/rabbitmq/docker-compose_rmq.yml down 
stop_rmq+:
	@docker-compose -f dockers/rabbitmq/docker-compose_rmq.yml down -v
	@make clean_rb

clean_rb:
	rm -Rf dockers/rabbitmq/data/*
	rm -Rf dockers/rabbitmq/logs/*

stats:
	@docker stats
	
show: #show docker's containers
	@docker ps -a -s

df:
	@docker system df

################ разное #####################
# log-app:
# 	docker-compose logs -f $(container_app)  

# cnn_app: #
# 	@docker-compose exec $(container_app) /bin/sh

# instmc: # Install mc to conteiner
# 	@echo "Установка mc"
# 	@docker-compose exec -u root $(container_app) /bin/bash -c "apt update && apt upgrade && apt-get install mc -y htop tree"		

# mc: # mc
# 	@docker-compose exec $(container_app) mc 

# run_2: # Run 
#     @docker run --name apiprt_sample --rm -it -p 8005:5005 apiport

# start:
# 	docker-compose -f dockers/elk/docker-compose.yml up -d  
# 	docker-compose -f dockers/rabbitmq/docker-compose_rmq.yml up -d  

# apidoc:
# 	@docker-compose exec -T php php -d memory_limit=256M -d xdebug.profiler_enable=0 ./app/vendor/bin/apigen generate app/src --destination app/doc
# 	@make resetOwner

# start2:
# 	@docker-compose up -d --build

# @rm -Rf $(DUMPS_DIR)/*

# stop_all4:
# 	docker-compose -f Deploy/elk/docker-compose.yml down 
# 	docker-compose -f Deploy/rabbitmq/docker-compose_rmq.yml down 

# stop_all3:
# 	@docker-compose -f Deploy/elk/docker-compose.yml down -v
# 	@docker-compose -f Deploy/rabbitmq/docker-compose_rmq.yml down -v
# 	@make clean

# show:
# 	@docker ps -a

# cnn_db: #Connect to DB container
# 	@docker-compose exec -u root $(container_db) /bin/bash

# cnn_admin: #Connect to DB container
# 	@docker-compose exec -u root $(container_admin) sh

# utils: #Install utils 
# 	@docker-compose exec -u root $(container_db) /bin/bash -c "apt update && apt upgrade && apt-get install -y mc tree htop curl"	

# utils2: #Install utils 
# 	@docker-compose exec -u root $(container_admin) sh -c "apk add mc tree htop curl"	

# backup: #Create DB backup
# 	@docker-compose exec -u root $(container_db) /bin/bash -c "pg_dump -U postgres -h localhost --port 5432 -c -d postgres > /docker-entrypoint-initdb.d/backup_pg_db.sql"	

# restore:
# 	@docker-compose exec -T $(container_db) sh -c 'psql -U postgres -h localhost --port 5432 -d postgres' < ./db-seed/backup_pg_db.sql	

# backup_db2: #Create DB backup
# 	@docker-compose exec -u root $(container_db) /bin/bash -c "pg_dump -U postgres -h localhost --port 5432 -Fc -d postgres > /docker-entrypoint-initdb.d/backup_pg_db.dump"	

# restore_db2:
# 	@docker-compose exec -T $(container_db) sh -c 'pg_restore -U postgres -h localhost --port 5432 -c -C -d postgres' < ./db-seed/backup_pg_db.dump	

# copy_db:	
# 	@docker cp django_db:/docker-entrypoint-initdb.d/backup_db.sql.gz .

# set_Owner: #Owner for all folder 
# 	@sudo chown -R $(USER):$(GROUP) .

# logs:
# 	@docker-compose logs -f

# logsdb:
# 	@docker-compose logs -f $(container_db)

# # =============================================================================
# resetOwner:
# 	@$(shell chown -Rf $(SUDO_USER):$(shell id -g -n $(SUDO_USER)) $(DUMPS_DIR) "$(shell pwd)/etc/ssl" "$(shell pwd)/web/app" 2> /dev/null)
