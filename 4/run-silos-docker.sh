ADVERTISEDIP=`ifconfig | grep -Eo 'inet (addr:)?([0-9]*\.){3}[0-9]*' | grep -Eo '([0-9]*\.){3}[0-9]*' | grep -v '127.0.0.1'`
GATEWAYPORT=3001

docker build -t silo-host -f ./ops/SiloHost/Dockerfile ./ &&
  docker run -it -e ADVERTISEDIP=$ADVERTISEDIP -e GATEWAYPORT=$GATEWAYPORT -p $GATEWAYPORT:3001 -p 8080:8080 --rm silo-host