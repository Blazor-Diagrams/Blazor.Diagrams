pushd $PSScriptRoot

$feedSource = "http://proget.wtg.zone/nuget/WTG-Internal/"
$apiKey = ""
$packagePath = "*.nupkg"

function Write-Log {
    Write-Host "$(get-date -f "yyyy-MM-dd HH:mm:ss.fff")`t$args"
}

try {
    $scriptSw = [System.Diagnostics.Stopwatch]::StartNew()
    & nuget.exe push $packagePath -ApiKey $apiKey -Source $feedSource -Verbosity detailed
    if (-not $?)
    {
        Write-Log "FAILED to deploy: $packagePath)"
    }
    else 
    {
        Write-Log "Deployed $packagePath )"
    }
}
finally {
    popd
    Write-Log "Finished (took: $($scriptSw.Elapsed))"
} 