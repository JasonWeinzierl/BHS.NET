param(
    [Parameter(Mandatory=$false, Position=0)]
    [string]$envType
)

$envFile = Resolve-Path -Path "$PSScriptRoot/.env"

if ($envType) {
    $envTypeFile = Resolve-Path -Path "$PSScriptRoot/.env.$envType" -ErrorAction Stop
}

$variables = Get-Content $envFile | Select-String -Pattern '^\s*[^\s=#]+=[^\s]+$' -Raw
if ($envType) {
    $variables += Get-Content $envTypeFile | Select-String -Pattern '^\s*[^\s=#]+=[^\s]+$' -Raw
}

foreach ($var in $variables) {
    $keyVal = $var -split '=', 2
    $key = $keyVal[0].Trim()
    $val = $keyVal[1].Trim('"')
    [Environment]::SetEnvironmentVariable($key, $val)
    #"$key=$([Environment]::GetEnvironmentVariable(($key)))"
}
