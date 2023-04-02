environment = "production"
location    = "centralus"
auto_deploy = false

custom_hostnames = [
  {
    name = "root",
    hostname = "beltonhistoricalsociety.org",
  },
  {
    name = "www",
    hostname = "www.beltonhistoricalsociety.org",
  },
]
storage_account_name = "beltonhistoricalstorage"
app_service_name = "beltonhistorical"
