# Script to add Turkish (Case 6) translations from DISMTools_TR to main project

$mainPanels = "C:\Users\Mustafa\Desktop\dism\DISMTools-0.7.3_upd1\Panels"
$trPanels = "C:\Users\Mustafa\Desktop\dism\DISMTools-0.7.3_upd1\DISMTools_TR\Panels"

# Get all TR files with Case 6
$trFiles = Get-ChildItem -Path $trPanels -Recurse -Filter "*.vb" | Where-Object {
    (Get-Content $_.FullName -Raw) -match "Case 6"
}

$processedCount = 0
$errorCount = 0

foreach ($trFile in $trFiles) {
    $relativePath = $trFile.FullName.Replace($trPanels, "")
    $mainFile = Join-Path $mainPanels $relativePath
    
    if (-not (Test-Path $mainFile)) {
        Write-Host "Main file not found: $relativePath" -ForegroundColor Yellow
        continue
    }
    
    $mainContent = Get-Content $mainFile -Raw
    
    # Check if Case 6 already exists
    if ($mainContent -match "Case 6") {
        Write-Host "Case 6 already exists in: $relativePath" -ForegroundColor Gray
        continue
    }
    
    # Read TR file content
    $trContent = Get-Content $trFile.FullName -Raw
    
    # Extract all Case 6 blocks from TR file
    $case6Pattern = '(?s)Case 6.*?(?=Case \d+|End Select)'
    $case6Matches = [regex]::Matches($trContent, $case6Pattern)
    
    if ($case6Matches.Count -eq 0) {
        Write-Host "No Case 6 blocks found in TR file: $relativePath" -ForegroundColor Yellow
        continue
    }
    
    # For each Case 6 block, insert it after Case 5 in main file
    $modifiedContent = $mainContent
    $insertCount = 0
    
    foreach ($case6Match in $case6Matches) {
        $case6Block = $case6Match.Value
        
        # Find the corresponding Case 5 block in main file
        # We'll insert Case 6 right after Case 5 ends
        $case5Pattern = '(?s)(Case 5.*?)(\r?\n\s*)(End Select|Case \d+)'
        
        if ($modifiedContent -match $case5Pattern) {
            $replacement = '$1' + "`r`n            " + $case6Block.Trim() + '$2$3'
            $modifiedContent = $modifiedContent -replace $case5Pattern, $replacement
            $insertCount++
        }
    }
    
    if ($insertCount -gt 0) {
        try {
            Set-Content -Path $mainFile -Value $modifiedContent -Encoding UTF8
            Write-Host "Added $insertCount Case 6 block(s) to: $relativePath" -ForegroundColor Green
            $processedCount++
        } catch {
            Write-Host "Error writing to: $relativePath - $_" -ForegroundColor Red
            $errorCount++
        }
    } else {
        Write-Host "Could not find insertion point in: $relativePath" -ForegroundColor Yellow
    }
}

Write-Host "`nSummary:" -ForegroundColor Cyan
Write-Host "Processed: $processedCount files" -ForegroundColor Green
Write-Host "Errors: $errorCount files" -ForegroundColor Red
