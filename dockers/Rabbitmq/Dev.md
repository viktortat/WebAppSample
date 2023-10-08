## RabbitMq

```sh
# start
cd ./src
docker-compose -f docker-compose_rmq.yml up -d
# stop
docker-compose -f docker-compose_rmq.yml down

# команды
rabbitmqctl add_user mytest mytest
rabbitmqctl delete_user username
rabbitmqctl change_password  username newpassword
rabbitmqctl set_user_tags user tag tag tag       # [administrator,monitoring,policymaker,management]
rabbitmqctl  set_permissions  -p /  mytest  '.*'  '.*'  '.*'
rabbitmqctl list_permissions -p /
rabbitmqctl  list_user_permissions <username>
rabbitmqctl clear_permissions [-p vhostpath] username

# ссылки
http://localhost:15672
http://localhost:15672/api/definitions
# пароли
admin:admin123 (admin account)
viktor:viktor (admin account)
ops0:ops0 (msg producer account)
ops1:ops1 (msg consumer account)
```

### Утилиты

```sh
docker run -it ubuntu sh
apt update
apt install mc
# alpine
apk update
apk add mc
```
