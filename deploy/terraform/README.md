# Создание виртуалки

[portal.azure.com-VM](https://portal.azure.com/#blade/HubsExtension/BrowseResource/resourceType/Microsoft.Compute%2FVirtualMachines)  
[azure-CLI](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli-windows?tabs=azure-cli)
[samples-azure](https://docs.microsoft.com/ru-ru/samples/browse/?products=azure0)
[azure-pricing](https://azure.microsoft.com/ru-ru/pricing/)

---

```sh
az vm image list --offer GitLab -o table --all
az vm image list --publisher Canonical -o table --all
az vm list-skus -l westeurope -o table | grep -v "NotAvailableForSubscription"

declare region=westeurope
az vm list-skus -l $region --query '[?resourceType==`virtualMachines` && restrictions==`[]`]' -o table
```
```sh
# format the tf files
terraform fmt

# initialize terraform Azure modules
terraform init

# validate the template
terraform validate

# plan and save the infra changes into tfplan file
terraform plan -out tfplan

# show the tfplan file
terraform show -json tfplan
terraform show -json tfplan >> tfplan.json

# Format tfplan.json file
terraform show -json tfplan | jq '.' > tfplan.json

# apply the infra changes
terraform apply tfplan

# delete the infra
terraform destroy

# cleanup files
rm terraform.tfstate
rm terraform.tfstate.backup
rm tfplan
rm tfplan.json
rm -r .terraform/
```

----
## Terraform and Ansible
```sh
cd ../svrradio/deploy/terraform/azure-vm
terraform init
terraform plan 
terraform apply

ansible-playbook -i ../inventory.yml ../ansible/utils.yml 
ssh ubuntu@52.174.67.185
```

Подключение через ssh

```sh
# ssh-keygen -R 52.148.246.8
ssh ubuntu@52.174.67.185
```
