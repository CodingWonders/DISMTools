$MainForm = New-Object -TypeName System.Windows.Forms.Form
[System.Windows.Forms.Label]$Label1 = $null
[System.Windows.Forms.ListView]$ListView1 = $null
[System.Windows.Forms.ColumnHeader]$ColumnHeader1 = $null
[System.Windows.Forms.ColumnHeader]$ColumnHeader2 = $null
[System.Windows.Forms.Button]$Cancel_Button = $null
[System.Windows.Forms.Button]$OK_Button = $null
function InitializeComponent
{
$Label1 = (New-Object -TypeName System.Windows.Forms.Label)
$ListView1 = (New-Object -TypeName System.Windows.Forms.ListView)
$ColumnHeader1 = (New-Object -TypeName System.Windows.Forms.ColumnHeader)
$ColumnHeader2 = (New-Object -TypeName System.Windows.Forms.ColumnHeader)
$Cancel_Button = (New-Object -TypeName System.Windows.Forms.Button)
$OK_Button = (New-Object -TypeName System.Windows.Forms.Button)
$MainForm.SuspendLayout()
#
#Label1
#
$Label1.Anchor = ([System.Windows.Forms.AnchorStyles][System.Windows.Forms.AnchorStyles]::Top -bor [System.Windows.Forms.AnchorStyles]::Left -bor [System.Windows.Forms.AnchorStyles]::Right)
$Label1.AutoSize = $true
$Label1.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Segoe UI',[System.Single]11.25,[System.Drawing.FontStyle]::Bold))
$Label1.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]12,[System.Int32]9))
$Label1.Name = [System.String]'Label1'
$Label1.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]498,[System.Int32]20))
$Label1.TabIndex = [System.Int32]0
$Label1.Text = [System.String]'Pick the keyboard layout to switch to from the list below and click OK:'
#
#ListView1
#
$ListView1.Anchor = ([System.Windows.Forms.AnchorStyles][System.Windows.Forms.AnchorStyles]::Top -bor [System.Windows.Forms.AnchorStyles]::Bottom -bor [System.Windows.Forms.AnchorStyles]::Left -bor [System.Windows.Forms.AnchorStyles]::Right)
$ListView1.Columns.AddRange([System.Windows.Forms.ColumnHeader[]]@($ColumnHeader1,$ColumnHeader2))
$ListView1.FullRowSelect = $true
$ListView1.GridLines = $true
$ListView1.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]12,[System.Int32]45))
$ListView1.MultiSelect = $false
$ListView1.Name = [System.String]'ListView1'
$ListView1.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]600,[System.Int32]346))
$ListView1.TabIndex = [System.Int32]1
$ListView1.UseCompatibleStateImageBehavior = $false
$ListView1.View = [System.Windows.Forms.View]::Details
$ListView1.add_SelectedIndexChanged($ListView1_SelectedIndexChanged)
#
#ColumnHeader1
#
$ColumnHeader1.Text = [System.String]'Layout Code'
$ColumnHeader1.Width = [System.Int32]128
#
#ColumnHeader2
#
$ColumnHeader2.Text = [System.String]'Layout Name'
$ColumnHeader2.Width = [System.Int32]384
#
#Cancel_Button
#
$Cancel_Button.Anchor = ([System.Windows.Forms.AnchorStyles][System.Windows.Forms.AnchorStyles]::Bottom -bor [System.Windows.Forms.AnchorStyles]::Right)
$Cancel_Button.BackColor = [System.Drawing.SystemColors]::Control
$Cancel_Button.DialogResult = [System.Windows.Forms.DialogResult]::Cancel
$Cancel_Button.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Segoe UI',[System.Single]11.25))
$Cancel_Button.ForeColor = [System.Drawing.Color]::FromArgb(([System.Int32]([System.Byte][System.Byte]0)),([System.Int32]([System.Byte][System.Byte]0)),([System.Int32]([System.Byte][System.Byte]0)))

$Cancel_Button.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]516,[System.Int32]397))
$Cancel_Button.Name = [System.String]'Cancel_Button'
$Cancel_Button.RightToLeft = [System.Windows.Forms.RightToLeft]::No
$Cancel_Button.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]96,[System.Int32]32))
$Cancel_Button.TabIndex = [System.Int32]2
$Cancel_Button.Text = [System.String]'Cancel'
$Cancel_Button.UseVisualStyleBackColor = $true
#
#OK_Button
#
$OK_Button.Anchor = ([System.Windows.Forms.AnchorStyles][System.Windows.Forms.AnchorStyles]::Bottom -bor [System.Windows.Forms.AnchorStyles]::Right)
$OK_Button.BackColor = [System.Drawing.SystemColors]::Control
$OK_Button.Enabled = $false
$OK_Button.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Segoe UI',[System.Single]11.25))
$OK_Button.ForeColor = [System.Drawing.Color]::FromArgb(([System.Int32]([System.Byte][System.Byte]0)),([System.Int32]([System.Byte][System.Byte]0)),([System.Int32]([System.Byte][System.Byte]0)))

$OK_Button.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]414,[System.Int32]397))
$OK_Button.Name = [System.String]'OK_Button'
$OK_Button.RightToLeft = [System.Windows.Forms.RightToLeft]::No
$OK_Button.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]96,[System.Int32]32))
$OK_Button.TabIndex = [System.Int32]2
$OK_Button.Text = [System.String]'OK'
$OK_Button.UseVisualStyleBackColor = $true
$OK_Button.add_Click($OK_Button_Click)
#
#MainForm
#
$MainForm.AcceptButton = $OK_Button
$MainForm.CancelButton = $Cancel_Button
$MainForm.ClientSize = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]624,[System.Int32]441))
$MainForm.Controls.Add($ListView1)
$MainForm.Controls.Add($Label1)
$MainForm.Controls.Add($Cancel_Button)
$MainForm.Controls.Add($OK_Button)
$MainForm.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Segoe UI',[System.Single]11.25,[System.Drawing.FontStyle]::Regular,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$MainForm.FormBorderStyle = [System.Windows.Forms.FormBorderStyle]::FixedSingle
$MainForm.MaximizeBox = $false
$MainForm.MinimizeBox = $false
$MainForm.ShowIcon = $false
$MainForm.StartPosition = [System.Windows.Forms.FormStartPosition]::CenterScreen
$MainForm.Text = [System.String]'Change Keyboard Layout'
$MainForm.add_Load($MainForm_Load)
$MainForm.ResumeLayout($false)
$MainForm.PerformLayout()
Add-Member -InputObject $MainForm -Name Label1 -Value $Label1 -MemberType NoteProperty
Add-Member -InputObject $MainForm -Name ListView1 -Value $ListView1 -MemberType NoteProperty
Add-Member -InputObject $MainForm -Name ColumnHeader1 -Value $ColumnHeader1 -MemberType NoteProperty
Add-Member -InputObject $MainForm -Name ColumnHeader2 -Value $ColumnHeader2 -MemberType NoteProperty
Add-Member -InputObject $MainForm -Name Cancel_Button -Value $Cancel_Button -MemberType NoteProperty
Add-Member -InputObject $MainForm -Name OK_Button -Value $OK_Button -MemberType NoteProperty
}
. InitializeComponent
