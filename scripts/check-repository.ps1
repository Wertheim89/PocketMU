# Phase 0 only: no game code/assets have been admitted yet.
$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
$allowed = @(
    '.gitignore',
    'README.md',
    'PROJECT_GOAL.md',
    'docs/PHASE_0_FEASIBILITY.md',
    'docs/ARCHITECTURE.md',
    'docs/MILESTONES.md',
    'docs/SOURCE_AUDIT.md',
    'scripts/check-repository.ps1',
    '.github/workflows/repository-safety.yml'
)
$tracked = @(git -C $repoRoot ls-files)
if ($LASTEXITCODE -ne 0) { throw 'Could not inspect tracked files.' }
if ($tracked.Count -eq 0) { throw 'No tracked files were found.' }
$failures = [System.Collections.Generic.List[string]]::new()
foreach ($path in $tracked) {
    if ($allowed -cnotcontains $path) {
        $failures.Add("Not allowed in Phase 0: $path")
        continue
    }
    # Read the index, not a potentially different working-tree version.
    $lines = @(git -C $repoRoot show ":$path")
    if ($LASTEXITCODE -ne 0) { throw "Could not read staged file: $path" }
    $content = $lines -join "\n"
    if ($content.IndexOf([char]0) -ge 0) {
        $failures.Add("Binary content found: $path")
    }
    # Deliberately bounded screening; not a complete secret-detection product.
    $tokenPatterns = @(
        'gh[pousr]_[A-Za-z0-9]{20,}',
        'github_pat_[A-Za-z0-9_]{20,}',
        'AKIA[A-Z0-9]{16}',
        '-----BEGIN (RSA |EC |OPENSSH )?PRIVATE KEY-----'
    )
    foreach ($pattern in $tokenPatterns) {
        if ($content -cmatch $pattern) {
            $failures.Add("Possible credential in $path; value is not printed.")
        }
    }
}
foreach ($required in $allowed) {
    if ($tracked -cnotcontains $required) { $failures.Add("Required file missing: $required") }
}
if ($failures.Count -gt 0) {
    $failures | ForEach-Object { Write-Error $_ -ErrorAction Continue }
    exit 1
}
Write-Output "PASS: $($tracked.Count) reviewed Phase 0 paths; no screened credentials found."
