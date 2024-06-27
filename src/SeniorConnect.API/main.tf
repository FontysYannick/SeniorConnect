terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "2.5.0"
    }
  }
}

provider "azurerm" {
  subscription_id = "cf6d83c6-2782-4980-89e3-b2ab7d72ce0e"
  use_msi         = true
  features {}
}

terraform {
    backend "azurerm" {
        resource_group_name  = "tf_rg_blobstore"
        storage_account_name = "tfstorageacccemaydemir"
        container_name       = "tfstate"
        key                  = "terraform.tfstate"
    }
}

variable "imagebuild" {
  type        = string
  description = "Latest Image Build"
}

# Reference to existing resource group
data "azurerm_resource_group" "existing_rg" {
  name = "ouderenplatform-RG"
}

resource "azurerm_container_group" "tfcg_test" {
  name                = "seniorconnectapi"
  location            = data.azurerm_resource_group.existing_rg.location
  resource_group_name = data.azurerm_resource_group.existing_rg.name

  ip_address_type = "public"
  dns_name_label  = "seniorconnect"
  os_type         = "Linux"

  container {
    name   = "seniorconnectapi"
    image  = "cemaydemir/seniorconnectapi:${var.imagebuild}"
    cpu    = "1"
    memory = "1"

    ports {
      port     = 8081
      protocol = "TCP"
    }
  }
}
