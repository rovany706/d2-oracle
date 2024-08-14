param(
    # Path to 'dota 2 beta\game\dota\pak01_dir.vpk'
    [string]$VpkPath='D:\SteamLibrary\steamapps\common\dota 2 beta\game\dota\pak01_dir.vpk'
)

[System.IO.Directory]::CreateDirectory('temp')
& .\vpkeditcli.exe $VpkPath --extract "scripts/npc/items.txt" --output "temp\items.txt"
yarn run convert
[System.IO.Directory]::Delete('temp', $true)
