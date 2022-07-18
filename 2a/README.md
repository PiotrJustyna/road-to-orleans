Silo: dotnet run

Client: dotnet run

//// IGNORE BELOW //// just work in progress play

---- Grafana

brew install Grafana

brew services start grafana

localhost:3000 admin / admin

https://grafana.com/docs/grafana/latest/setup-grafana/installation/mac/

https://grafana.com/docs/grafana/latest/setup-grafana/configure-grafana/

/usr/local/etc/grafana/grafana.ini

---- Java

https://www.java.com/en/download/ >= 8

https://zipkin.io/pages/quickstart.html

curl -sSL https://zipkin.io/quickstart.sh | bash -s

java -jar zipkin.jar

http://127.0.0.1:9411/

java -version

---- DotNet

dotnet tool install --global dotnet-counters

dotnet counters monitor --process-id 123 --counters MyApplication

https://github.com/dotnet/orleans/blob/d4cead8950f73e3de15afddf32e69926422f0a31/src/Orleans.Core/Diagnostics/ActivityPropagationGrainCallFilter.cs