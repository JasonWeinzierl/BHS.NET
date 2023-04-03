$envType = $args[0]
$verbose = $args.Contains("--verbose")

$envFiles = @(".env")

if ($envType) {
    $envFile = ".env.$envType"
    if (Test-Path $envFile) {
        $envFiles += $envFile
    } else {
        throw "File '$envFile' not found."
    }
}

foreach ($envFile in $envFiles) {

    $variables = Select-String -Path $envFile -Pattern '^\s*[^\s=#]+=[^\s]+$' -Raw

    foreach ($var in $variables) {
        $keyVal = $var -split '=', 2
        $key = $keyVal[0].Trim()
        $val = $keyVal[1].Trim('"')
        [Environment]::SetEnvironmentVariable($key, $val)
        if ($verbose) {
            "$key=$([Environment]::GetEnvironmentVariable(($key)))"
        }
    }
    
}
