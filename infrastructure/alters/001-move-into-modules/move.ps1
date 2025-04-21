$csvFilePath = "move.txt"

if (-not (Test-Path -Path $csvFilePath)) {
    Write-Host "The file $csvFilePath does not exist. Please check the path and try again." -ForegroundColor Red
    exit
}

# Import the CSV file
$data = Import-Csv -Path $csvFilePath -Delimiter " "

# Loop through each row and construct the terraform state mv commands
foreach ($row in $data) {
    $originalResource = $row.OldResource
    $moduleName = $row.NewModule

    # Construct the target resource name
    $targetResource = "$moduleName.$originalResource"

    # Construct and output the terraform state mv command
    $command = "terraform state mv $originalResource $targetResource"
    Invoke-Expression $command
}
