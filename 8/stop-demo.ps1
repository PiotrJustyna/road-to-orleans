docker stop (docker ps | select-string client-cluster | foreach-object { $_.tostring().split('')[0] })  ;
docker stop (docker ps | select-string silo-host-cluster | foreach-object { $_.tostring().split('')[0] } )
