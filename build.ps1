# === CONFIGURATION ===
$unityPath = "C:\Program Files\Unity\Hub\Editor\6000.1.4f1\Editor\Unity.exe"  # adjust if needed
$baseBuildPath = "D:\Builds"

# === 1. Get the latest tag ===
$lastTag = (git describe --tags --abbrev=0).Trim()
Write-Host "Latest tag: $lastTag"

# === 2. Increment patch version ===
$versionParts = $lastTag -split '\.'
if ($versionParts.Length -ne 3) {
    Write-Error "Invalid tag format: $lastTag. Expected format: X.Y.Z"
    exit 1
}

[int]$patch = [int]$versionParts[2]
$patch++
$newTag = "$($versionParts[0]).$($versionParts[1]).$patch"
Write-Host "New tag: $newTag"

# === 3. Create and push the new tag ===
git tag $newTag
git push origin $newTag
Write-Host "Tag $newTag created"

# === 4. Build project using Unity CLI ===
$buildPath = "$baseBuildPath\$newTag"
if (-not (Test-Path $buildPath)) {
    New-Item -ItemType Directory -Force -Path $buildPath | Out-Null
}

$exePath = "$buildPath\Game.exe"
$logFile = "$buildPath\unity_build.log"

Write-Host "Building to: $exePath"

# Ensure Unity is not already running
if (Get-Process Unity -ErrorAction SilentlyContinue) {
    Write-Host "Unity is currently running. Please close it before running the build."
    exit 1
}

Start-Process -FilePath "$unityPath" -ArgumentList @(
    "-batchmode",
    "-nographics",
    "-quit",
    "-projectPath", "`"$PSScriptRoot`"",
    "-executeMethod", "BuildScript.BuildDebug",
    "-logFile", "`"$logFile`""
) -Wait

Write-Host "Build complete. Log saved to: $logFile"
