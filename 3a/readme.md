# readme

This example builds on top of Solution 3 by adding persistence to grains.

## grain id 0

```json
{
  "GrainReference": "_GrainReference=000000000000000000000000000000000300000002293d31",
  "GrainType": "Grains.HelloWorld,Grains.helloWorldState",
  "ETag": 3,
  "StringState": "{\"$id\":\"1\",\"$type\":\"Grains.HelloWorldState, Grains\",\"GreetingTimeUtc\":\"2021-02-19T00:01:44.128848Z\"}"
}
```

## grain id 2

```json
{
  "GrainReference": "_GrainReference=000000000000000000000000000000020300000002293d31",
  "GrainType": "Grains.HelloWorld,Grains.helloWorldState",
  "ETag": 0,
  "StringState": "{\"$id\":\"1\",\"$type\":\"Grains.HelloWorldState, Grains\",\"GreetingTimeUtc\":\"2021-02-19T00:09:41.28897Z\"}"
}
```