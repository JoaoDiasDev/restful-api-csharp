FROM mysql:5.7.22
EXPOSE 3306
COPY ./restful-api-joaodias/db/migrations /home/database/
COPY ./restful-api-joaodias/db/dataset /home/database/
COPY ./restful-api-joaodias/ci/init_database.sh /docker-entrypoint-initdb.d/init_database.sh
