FROM peterevans/vegeta:6.8.1
WORKDIR /app
COPY . .
COPY ./vegeta.sh .
RUN apk update &&      apk add jq && apk add curl
RUN chmod +x ./vegeta.sh
CMD [ "./vegeta.sh" ]