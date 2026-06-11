param([string]$FilePath)
$content = Get-Content $FilePath
$content = $content -replace '^pick (e61b2f4.*)', 'edit $1'
$content = $content -replace '^pick (d51c454.*)', 'edit $1'
$content = $content -replace '^pick (3600707.*)', 'edit $1'
$content = $content -replace '^pick (834d013.*)', 'edit $1'
Set-Content -Path $FilePath -Value $content
