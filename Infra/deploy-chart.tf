provider "helm" {
  kubernetes {
    host                   = data.aws_eks_cluster.tc_eks_cluster.endpoint
    cluster_ca_certificate = base64decode(data.aws_eks_cluster.tc_eks_cluster.certificate_authority[0].data)
    exec {
      api_version = "client.authentication.k8s.io/v1beta1"
      args        = ["eks", "get-token", "--cluster-name", "eks_vid-proc"]
      command     = "aws"
    }
  }
}

resource "helm_release" "status" {
  name             = "status"
  namespace        = "dev"
  create_namespace = true
  chart            = "../Helm/status_tracking-chart"

  values = [
    file("../Helm/status_tracking-chart/values.yaml"),
    file("../Helm/status_tracking-chart/values-dev.yaml")
  ]

  set {
    name  = "rabbitmq.password"
    value = var.brokerpassword
  }
  set {
    name  = "rds.host"
    value = var.rdsHost
  }
}