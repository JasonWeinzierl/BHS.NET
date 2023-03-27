$variables = Select-String -Path '.env' -Pattern '^\s*[^\s=#]+=[^\s]+$' -Raw

foreach ($var in $variables) {
    $keyVal = $var -split '=', 2
    $key = $keyVal[0].Trim()
    $val = $keyVal[1].Trim('"')
    [Environment]::SetEnvironmentVariable($key, $val)
    #"$key=$([Environment]::GetEnvironmentVariable(($key)))"
}
