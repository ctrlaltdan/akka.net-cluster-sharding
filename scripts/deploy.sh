#! /bin/sh

kubectl apply -f ./kubernetes/namespace.yaml
kubectl create configmap config-postgres --from-file=config-postgres --namespace=cluster-sample --dry-run -o yaml | kubectl apply -f -

kubectl apply -f ./kubernetes/lighthouse-deployment.yaml
kubectl apply -f ./kubernetes/postgres-deployment.yaml
kubectl apply -f ./kubernetes/apinode-deployment.yaml
kubectl apply -f ./kubernetes/clusternode-deployment.yaml