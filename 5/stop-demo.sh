docker ps | grep client-cluster | awk '{print $1}' | xargs -I {} docker stop {}
docker ps | grep silo-host-cluster | awk '{print $1}' | xargs -I {} docker stop {}