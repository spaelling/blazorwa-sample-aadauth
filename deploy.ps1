$AccountName = '<storage account name>'
Remove-Item -Path .\out -Force -Recurse -ErrorAction SilentlyContinue
dotnet publish -c Release -o out
$null = az storage blob upload-batch --account-name $AccountName -s ".\out\<project name>\dist" -d `$web
$null = az storage blob update --account-name $AccountName -c `$web -n _framework/wasm/mono.wasm --content-type application/wasm