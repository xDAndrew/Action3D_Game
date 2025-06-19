# === НАСТРОЙКИ ===
$unityPath = "C:\Program Files\Unity\Hub\Editor\6000.1.4f1\Editor\Unity.exe"  # проверь путь
$baseBuildPath = "D:\Builds"

# === 1. Получаем последний тег ===
$lastTag = git describe --tags --abbrev=0
Write-Host "Последний тег: $lastTag"

# === 2. Инкремент патча ===
$versionParts = $lastTag -split '\.'
if ($versionParts.Length -ne 3) {
    Write-Error "Неверный формат тега: $lastTag. Ожидается X.Y.Z"
    exit 1
}

[int]$patch = [int]$versionParts[2]
$patch++
$newTag = "$($versionParts[0]).$($versionParts[1]).$patch"
Write-Host "Новый тег: $newTag"

# === 3. Создаем тег и пушим ===
git tag $newTag
#git push origin $newTag
Write-Host "✅ Новый тег $newTag создан и отправлен"

# === 4. Сборка без BuildScript ===
$buildPath = "$baseBuildPath\$newTag"
if (-not (Test-Path $buildPath)) {
    New-Item -ItemType Directory -Force -Path $buildPath | Out-Null
}

# Путь до финального .exe
$exePath = "$buildPath\Game.exe"

Write-Host "🏗️ Билдим в: $exePath"

Start-Process -FilePath "$unityPath" -ArgumentList @(
    "-batchmode",
    "-nographics",
    "-quit",
    "-projectPath", "$PSScriptRoot",
    "-buildWindowsPlayer", "$exePath",
    "-logFile", "$buildPath\unity_build.log"
) -Wait

Write-Host "✅ Сборка завершена. Лог: $buildPath\unity_build.log"