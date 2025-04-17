terraform {
  backend "s3" {
    bucket = "tc-tf-backend"
    key    = "backend/terraform_status_tracking.tfstate"
    region = "us-east-1"
  }
}