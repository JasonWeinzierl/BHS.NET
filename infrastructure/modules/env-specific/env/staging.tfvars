environment = "staging"
location    = "centralus"
auto_deploy = true

custom_hostnames = [
  {
    name = "staging",
    hostname = "staging.beltonhistoricalsociety.org",
  },
]
storage_account_name = "stagebhsstorage"
app_service_name = "staging-beltonhistorical"
enable_free_cosmos = true
