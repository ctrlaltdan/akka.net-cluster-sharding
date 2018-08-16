#! /bin/sh

kubectl apply -f ./kubernetes/namespace.yaml
kubectl create configmap config-postgres --from-file=config-postgres --namespace=cluster-sample

kubectl apply -f ./kubernetes/lighthouse-deployment.yaml
kubectl apply -f ./kubernetes/postgres-deployment.yaml