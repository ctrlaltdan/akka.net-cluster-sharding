#! /bin/sh
echo "|-------- Building docker files ---------|"
echo "Working Directory: $PWD"
echo ""

docker build --tag cluster.sharding.apinode --file ./src/ApiNode/Dockerfile .
docker build --tag cluster.sharding.clusternode --file ./src/ClusterNode/Dockerfile .