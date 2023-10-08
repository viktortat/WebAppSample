# Основная

[portal.azure.com-VM](https://portal.azure.com/#blade/HubsExtension/BrowseResource/resourceType/Microsoft.Compute%2FVirtualMachines)  
[azure-CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli-windows?tabs=azure-cli)

```sh
az
# terraform plan
terraform apply
# terraform destroy
```

---

Подключение через ssh

```
ssh adminuser@20.86.112.88
```

---

```
az vm image list --offer GitLab -o table --all
az vm image list --publisher Canonical -o table --all
az vm list-skus -l westeurope -o table | grep -v "NotAvailableForSubscription"

declare region=westeurope
az vm list-skus -l $region --query '[?resourceType==`virtualMachines` && restrictions==`[]`]' -o table
```
ssh-keygen -t rsa -m PEM -b 4096 -C "azureuser@viktor"

terraform plan -out vm
terraform apply -auto-approve vm

