# Akka.net Cluster Sharding Example

The following example demonstrates two services communicating with eachother in a sharded cluster model using the Akka.net modelling system.

State is persisted to Postgres using the connection string `User ID=example;Password=example;Host=localhost;Port=5433;Database=example;`

Run `docker-compose up -d` to run the Postgres service and launch the `ApiNode`, `ClusterNode` and `Lighthouse` applications in Visual Studio.

## The following API endpoints exist

### Fetch the user's basket contents.

This endpoint will return a basket whether or not the user has been seen before.

```
GET localhost:4010/{user}

RESPONSE
{
    "shardId": "9",
    "entityId": "Dan",
    "items": {
        "Coco Pops": 3
    }
}
```

### Add items to the user's basket

This endpoint will add items to the user. Items are persisted in Postgres.

```
POST localhost:4010/{user}
BODY
{
	"product": "Coco Pops",
	"quantity": 1
}
```


### Clear the basket

This endpoint clears all the persisted items in Postgres for a user.

```
DELETE localhost:4010/{user}
```

