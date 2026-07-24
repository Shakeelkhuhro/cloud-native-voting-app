# Cloud Native Voting Application

A production-style **Cloud Native Voting Application** built with a microservices architecture and deployed on **Kubernetes** using **Helm**.

This project demonstrates modern cloud-native deployment practices, including Kubernetes resource management, Helm packaging, Ingress routing, persistent storage, and automatic pod scaling using the Horizontal Pod Autoscaler (HPA).

---

## Overview

The application consists of three custom microservices and two supporting infrastructure services.

| Component | Technology | Purpose |
|----------|------------|---------|
| Vote Service | Python (Flask) | Collects user votes |
| Worker Service | .NET | Processes votes from Redis and stores them in PostgreSQL |
| Result Service | Node.js | Displays live voting results |
| Redis | Bitnami Redis | Message Queue |
| PostgreSQL | Bitnami PostgreSQL | Persistent Database |

---

# Architecture

```
                    +----------------------+
                    |        Users         |
                    +----------+-----------+
                               |
                         NGINX Ingress
                               |
                +--------------+--------------+
                |                             |
         Vote Service                  Result Service
                |
                |
            Redis Queue
                |
         Worker Service
                |
        PostgreSQL Database
```

---

# Features

## Application

- Python Flask Vote Service
- .NET Worker Service
- Node.js Result Service
- Redis Message Queue
- PostgreSQL Database

---

## Kubernetes

- Deployments
- StatefulSets
- Services
- ConfigMaps
- Secrets
- Persistent Volume Claims (PVC)
- Namespaces
- NGINX Ingress Controller
- Resource Requests & Limits
- Horizontal Pod Autoscaler (HPA)

---

## Helm

- Parent Helm Chart
- Helm Subcharts
- Helm Template Helpers
- Configurable Values
- Dependency Management
- Bitnami Helm Dependencies

---

## Autoscaling

The Vote service is configured with Kubernetes **Horizontal Pod Autoscaler (HPA)** using the **autoscaling/v2 API**.

### Features

- CPU-based Autoscaling
- Configurable CPU Threshold
- Automatic Scale Up
- Automatic Scale Down
- Configurable Replica Limits

Example configuration:

```yaml
autoscaling:
  enabled: true
  minReplicas: 2
  maxReplicas: 5
  targetCPUUtilizationPercentage: 70
```

The CPU threshold can be changed directly from:

```
helm/voting-app/charts/vote/values.yaml
```

---

# Resource Management

Containers are configured with Kubernetes Resource Requests and Limits.

Example:

```yaml
resources:
  requests:
    cpu: 100m
    memory: 128Mi

  limits:
    cpu: 250m
    memory: 256Mi
```

This enables:

- Fair Scheduling
- Resource Isolation
- CPU-based Autoscaling
- Predictable Performance

---

# Helm Project Structure

```
helm/
└── voting-app/
    ├── Chart.yaml
    ├── values.yaml
    ├── charts/
    │   ├── vote/
    │   ├── worker/
    │   ├── result/
    │   ├── redis/
    │   └── postgresql/
    └── templates/
```

---

# Project Structure

```
cloud-native-voting-app/

├── helm/
│   └── voting-app/
│       ├── Chart.yaml
│       ├── values.yaml
│       ├── templates/
│       └── charts/
│           ├── vote/
│           ├── worker/
│           ├── result/
│           ├── redis/
│           └── postgresql/
│
├── vote/
├── worker/
├── result/
├── .github/
└── README.md
```

---

# Deployment

Build Helm dependencies

```bash
helm dependency build helm/voting-app
```

Install

```bash
helm install voting-app helm/voting-app \
-n voting-app \
--create-namespace
```

Upgrade

```bash
helm upgrade voting-app helm/voting-app \
-n voting-app
```

Uninstall

```bash
helm uninstall voting-app -n voting-app
```

---

# Verification

Pods

```bash
kubectl get pods -n voting-app
```

Services

```bash
kubectl get svc -n voting-app
```

Ingress

```bash
kubectl get ingress -n voting-app
```

Deployments

```bash
kubectl get deployments -n voting-app
```

Horizontal Pod Autoscaler

```bash
kubectl get hpa -n voting-app
```

Watch HPA

```bash
kubectl get hpa -n voting-app -w
```

Monitor Resource Usage

```bash
kubectl top pods -n voting-app
```

---

# Technology Stack

## Application

- Python Flask
- Node.js
- .NET

## Infrastructure

- Redis
- PostgreSQL

## Containerization

- Docker

## Orchestration

- Kubernetes
- Kind

## Packaging

- Helm

## Networking

- NGINX Ingress Controller

## Scaling

- Horizontal Pod Autoscaler (HPA)
- Metrics Server

## CI/CD & GitOps

- GitHub Actions
- ArgoCD *(Planned)*

## Monitoring

- Prometheus *(Planned)*
- Grafana *(Planned)*

---

# Current Project Status

| Feature | Status |
|----------|:------:|
| Dockerized Microservices | ✅ |
| Kubernetes Deployments | ✅ |
| Services | ✅ |
| ConfigMaps | ✅ |
| Secrets | ✅ |
| Persistent Storage | ✅ |
| StatefulSets | ✅ |
| Helm Parent Chart | ✅ |
| Helm Subcharts | ✅ |
| Ingress | ✅ |
| Resource Requests & Limits | ✅ |
| Horizontal Pod Autoscaler | ✅ |
| Metrics Server | ✅ |
| Prometheus | 🚧 |
| Grafana | 🚧 |
| GitHub Actions CI | 🚧 |
| ArgoCD | 🚧 |
| cert-manager | 🚧 |
| TLS/HTTPS | 🚧 |
| KEDA | 🚧 |
| Vertical Pod Autoscaler | 🚧 |
| Cluster Autoscaler | 🚧 |

---

# Project Roadmap

## Phase 1 — Kubernetes Foundation ✅

- [x] Dockerize Services
- [x] Kubernetes Deployments
- [x] Services
- [x] ConfigMaps
- [x] Secrets
- [x] StatefulSets
- [x] Persistent Volumes
- [x] Helm Charts
- [x] Ingress
- [x] Resource Requests & Limits
- [x] Horizontal Pod Autoscaler (HPA)

---

## Phase 2 — Observability

- [ ] Deploy Prometheus
- [ ] Deploy Grafana
- [ ] Create Dashboards
- [ ] Configure AlertManager
- [ ] Application Metrics

---

## Phase 3 — GitOps

- [ ] Deploy ArgoCD
- [ ] GitOps Workflow
- [ ] Automated Synchronization
- [ ] Self-Healing Deployments
- [ ] Rollback Strategy

---

## Phase 4 — Security

- [ ] cert-manager
- [ ] TLS Certificates
- [ ] HTTPS Ingress
- [ ] RBAC
- [ ] Network Policies
- [ ] Pod Security Standards
- [ ] Secret Management

---

## Phase 5 — Scaling & Reliability

- [ ] Vertical Pod Autoscaler (VPA)
- [ ] KEDA
- [ ] Pod Disruption Budgets
- [ ] Cluster Autoscaler
- [ ] High Availability

---

## Phase 6 — CI/CD

- [ ] GitHub Actions
- [ ] Docker Image Build
- [ ] Image Scanning
- [ ] Helm Lint
- [ ] Helm Packaging
- [ ] Kubernetes Deployment
- [ ] Automated Releases

---

## Phase 7 — Production Readiness

- [ ] ExternalDNS
- [ ] Logging Stack
- [ ] Distributed Tracing
- [ ] Backup & Restore
- [ ] Disaster Recovery
- [ ] Multi-Node Kubernetes Cluster
- [ ] Performance Testing
- [ ] Production Monitoring

---

# Learning Outcomes

This project demonstrates hands-on experience with:

- Docker Containerization
- Kubernetes Workloads
- Stateful Applications
- Helm Chart Development
- Helm Subcharts
- Helm Templates
- ConfigMaps
- Secrets
- Persistent Storage
- Resource Management
- Horizontal Pod Autoscaler
- NGINX Ingress
- Cloud Native Application Deployment

---

# Contributing

Contributions are welcome.

If you have ideas for improvements, feel free to:

- Open an Issue
- Submit a Pull Request
- Suggest Enhancements
- Improve Documentation

---

# License

This project is licensed under the **MIT License**.

---

# Author

**Shakeel Ahmed Khuhro**

Junior Software Engineer | DevOps Engineer

Passionate about Cloud Native Technologies, Kubernetes, DevOps, Automation, Platform Engineering, and Open Source.
