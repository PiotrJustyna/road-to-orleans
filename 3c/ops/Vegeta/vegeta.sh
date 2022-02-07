#!/bin/sh
clear
echo "GET http://host.docker.internal:5000/helloworld?n=3" | vegeta attack -redirects=0 -duration=10s -rate=100 | tee results.bin | vegeta report
vegeta report -type=json results.bin > metrics.json
cat results.bin | vegeta plot >plot.html
cat results.bin | vegeta report -type="hist[0,2ms,5ms,10ms,20ms,50ms]"