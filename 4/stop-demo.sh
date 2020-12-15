docker ps -a | grep client-cluster | awk '{print $1}' | xargs -I {} docker stop {}
docker ps -a | grep silo-host-cluster | awk '{print $1}' | xargs -I {} docker stop {}