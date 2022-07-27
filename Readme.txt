docker-compose -f docker-compose.yml up -d
docker-compose -f docker-compose.yml stop
docker-compose -f docker-compose.yml down
 
docker-compose -f docker-compose.yml ps    
         Name                      Command            State                        Ports
--------------------------------------------------------------------------------------------------------------
kafka-druid_kafka_1       /etc/confluent/docker/run   Up      0.0.0.0:29092->29092/tcp, 0.0.0.0:9092->9092/tcp
kafka-druid_zookeeper_1   /etc/confluent/docker/run   Up      0.0.0.0:2181->2181/tcp, 2888/tcp, 3888/tcp      


docker exec -it kafka-druid_kafka_1  /bin/bash

# inside the bash shell
kafka-topics --bootstrap-server localhost:9092 --topic first_topic --create --partitions 3 --replication-factor 1

# list topics
kafka-topics --bootstrap-server localhost:9092 --list
kafka-topics --bootstrap-server localhost:9092 --describe --topic first_topic

# to delete a topic
kafka-topics --bootstrap-server localhost:9092 --delete --topic first_topic

# console producer
kafka-console-producer --bootstrap-server localhost:9092 --topic first_topic


# console consumer, get messages - starting now
kafka-console-consumer --bootstrap-server localhost:9092 --topic first_topic

# console consumer, get all possible messages
kafka-console-consumer --bootstrap-server localhost:9092 --topic first_topic --from-beginning

# druid console
http://localhost:8888/unified-console.html#load-data

# zookeeper error that port not available
# Cannot start service zookeeper: Ports are not available: listen tcp 0.0.0.0:2181: bind: An attempt was made to access a socket in a way forbidden by its access permissions
# open up a admin command prompt and run the following command
net stop winnat

# trouble with design: https://www.druidforum.org/t/rollup-compact-all-record-to-maintain-last-value-only/7530/4