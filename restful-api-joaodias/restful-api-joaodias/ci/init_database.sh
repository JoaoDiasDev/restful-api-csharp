for i in `find /home/database -name "*.sql" | sort --version-sort`; do mysql -udocker -pdocker restful-api-joaodias < $i; done;
