#!/bin/bash

containers=($(docker ps --format "{{.Names}}"))

# Logs im Hintergrund abrufen und in Dateien schreiben
echo "--------Running Docker containers--------"
for container in "${containers[@]}"; do
    echo "  - $container"
    docker logs "$container" > "/tmp/${container}_logs.txt" 2>&1 &
done

echo "--------Testing Docker compose up--------"
echo "docker ps -a"
docker ps -a
sleep 3
echo "---------------Docker logs---------------"
# Logs nacheinander ausgeben
for container in "${containers[@]}"; do
    echo "docker logs $container"
    cat "/tmp/${container}_logs.txt"
    echo "-----"
done
echo "docker network ls"
docker network ls
echo "-----------------------------------------"