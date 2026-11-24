$WDSHCImagePicker = New-Object -TypeName System.Windows.Forms.Form
[System.Windows.Forms.Label]$Label1 = $null
[System.Windows.Forms.Label]$Label2 = $null
[System.Windows.Forms.TableLayoutPanel]$TableLayoutPanel1 = $null
[System.Windows.Forms.Label]$Label3 = $null
[System.Windows.Forms.ComboBox]$ImgNameCombo = $null
[System.Windows.Forms.ComboBox]$ImgGroupCombo = $null
[System.Windows.Forms.Button]$Select_Button = $null
[System.Windows.Forms.Button]$Refresh_Button = $null
[System.Windows.Forms.GroupBox]$SelectedImageDetailsGroup = $null
[System.Windows.Forms.TextBox]$ImageDetailsTB = $null
function InitializeComponent
{
$Label1 = (New-Object -TypeName System.Windows.Forms.Label)
$Label2 = (New-Object -TypeName System.Windows.Forms.Label)
$TableLayoutPanel1 = (New-Object -TypeName System.Windows.Forms.TableLayoutPanel)
$Label3 = (New-Object -TypeName System.Windows.Forms.Label)
$ImgNameCombo = (New-Object -TypeName System.Windows.Forms.ComboBox)
$ImgGroupCombo = (New-Object -TypeName System.Windows.Forms.ComboBox)
$Select_Button = (New-Object -TypeName System.Windows.Forms.Button)
$Refresh_Button = (New-Object -TypeName System.Windows.Forms.Button)
$SelectedImageDetailsGroup = (New-Object -TypeName System.Windows.Forms.GroupBox)
$ImageDetailsTB = (New-Object -TypeName System.Windows.Forms.TextBox)
$TableLayoutPanel1.SuspendLayout()
$SelectedImageDetailsGroup.SuspendLayout()
$WDSHCImagePicker.SuspendLayout()
#
#Label1
#
$Label1.AutoSize = $true
$Label1.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Segoe UI',[System.Single]11.25,[System.Drawing.FontStyle]::Bold,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$Label1.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]12,[System.Int32]9))
$Label1.Name = [System.String]'Label1'
$Label1.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]527,[System.Int32]20))
$Label1.TabIndex = [System.Int32]0
$Label1.Text = [System.String]'Select the image group and image name from the lists below and click OK.'
#
#Label2
#
$Label2.AutoSize = $true
$Label2.BackColor = [System.Drawing.SystemColors]::Control
$Label2.Dock = [System.Windows.Forms.DockStyle]::Fill
$Label2.ForeColor = [System.Drawing.Color]::FromArgb(([System.Int32]([System.Byte][System.Byte]0)),([System.Int32]([System.Byte][System.Byte]0)),([System.Int32]([System.Byte][System.Byte]0)))

$Label2.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]3,[System.Int32]0))
$Label2.Name = [System.String]'Label2'
$Label2.RightToLeft = [System.Windows.Forms.RightToLeft]::No
$Label2.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]132,[System.Int32]34))
$Label2.TabIndex = [System.Int32]0
$Label2.Text = [System.String]'Image Group:'
$Label2.TextAlign = [System.Drawing.ContentAlignment]::MiddleLeft
#
#TableLayoutPanel1
#
$TableLayoutPanel1.AllowDrop = $true
$TableLayoutPanel1.Anchor = ([System.Windows.Forms.AnchorStyles][System.Windows.Forms.AnchorStyles]::Top -bor [System.Windows.Forms.AnchorStyles]::Left -bor [System.Windows.Forms.AnchorStyles]::Right)
$TableLayoutPanel1.ColumnCount = [System.Int32]2
$TableLayoutPanel1.ColumnStyles.Add((New-Object -TypeName System.Windows.Forms.ColumnStyle -ArgumentList @([System.Windows.Forms.SizeType]::Percent,[System.Single]23)))
$TableLayoutPanel1.ColumnStyles.Add((New-Object -TypeName System.Windows.Forms.ColumnStyle -ArgumentList @([System.Windows.Forms.SizeType]::Percent,[System.Single]77)))
$TableLayoutPanel1.Controls.Add($Label2,[System.Int32]0,[System.Int32]0)
$TableLayoutPanel1.Controls.Add($Label3,[System.Int32]0,[System.Int32]1)
$TableLayoutPanel1.Controls.Add($ImgNameCombo,[System.Int32]1,[System.Int32]1)
$TableLayoutPanel1.Controls.Add($ImgGroupCombo,[System.Int32]1,[System.Int32]0)
$TableLayoutPanel1.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]12,[System.Int32]43))
$TableLayoutPanel1.Name = [System.String]'TableLayoutPanel1'
$TableLayoutPanel1.RowCount = [System.Int32]2
$TableLayoutPanel1.RowStyles.Add((New-Object -TypeName System.Windows.Forms.RowStyle -ArgumentList @([System.Windows.Forms.SizeType]::Percent,[System.Single]50)))
$TableLayoutPanel1.RowStyles.Add((New-Object -TypeName System.Windows.Forms.RowStyle -ArgumentList @([System.Windows.Forms.SizeType]::Percent,[System.Single]50)))
$TableLayoutPanel1.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]600,[System.Int32]68))
$TableLayoutPanel1.TabIndex = [System.Int32]1
#
#Label3
#
$Label3.AutoSize = $true
$Label3.BackColor = [System.Drawing.SystemColors]::Control
$Label3.Dock = [System.Windows.Forms.DockStyle]::Fill
$Label3.ForeColor = [System.Drawing.Color]::FromArgb(([System.Int32]([System.Byte][System.Byte]0)),([System.Int32]([System.Byte][System.Byte]0)),([System.Int32]([System.Byte][System.Byte]0)))

$Label3.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]3,[System.Int32]34))
$Label3.Name = [System.String]'Label3'
$Label3.RightToLeft = [System.Windows.Forms.RightToLeft]::No
$Label3.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]132,[System.Int32]34))
$Label3.TabIndex = [System.Int32]0
$Label3.Text = [System.String]'Image Name:'
$Label3.TextAlign = [System.Drawing.ContentAlignment]::MiddleLeft
#
#ImgNameCombo
#
$ImgNameCombo.BackColor = [System.Drawing.SystemColors]::Window
$ImgNameCombo.Dock = [System.Windows.Forms.DockStyle]::Fill
$ImgNameCombo.DropDownWidth = [System.Int32]121
$ImgNameCombo.FlatStyle = [System.Windows.Forms.FlatStyle]::System
$ImgNameCombo.ForeColor = [System.Drawing.SystemColors]::WindowText
$ImgNameCombo.FormattingEnabled = $true
$ImgNameCombo.ImeMode = [System.Windows.Forms.ImeMode]::NoControl
$ImgNameCombo.ItemHeight = [System.Int32]20
$ImgNameCombo.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]141,[System.Int32]37))
$ImgNameCombo.Name = [System.String]'ImgNameCombo'
$ImgNameCombo.RightToLeft = [System.Windows.Forms.RightToLeft]::No
$ImgNameCombo.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]456,[System.Int32]28))
$ImgNameCombo.TabIndex = [System.Int32]1
$ImgNameCombo.add_SelectedIndexChanged($ImgNameCombo_SelectedIndexChanged)
#
#ImgGroupCombo
#
$ImgGroupCombo.Dock = [System.Windows.Forms.DockStyle]::Fill
$ImgGroupCombo.FlatStyle = [System.Windows.Forms.FlatStyle]::System
$ImgGroupCombo.FormattingEnabled = $true
$ImgGroupCombo.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]141,[System.Int32]3))
$ImgGroupCombo.Name = [System.String]'ImgGroupCombo'
$ImgGroupCombo.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]456,[System.Int32]28))
$ImgGroupCombo.TabIndex = [System.Int32]1
$ImgGroupCombo.add_SelectedIndexChanged($ImgGroupCombo_SelectedIndexChanged)
#
#Select_Button
#
$Select_Button.Anchor = ([System.Windows.Forms.AnchorStyles][System.Windows.Forms.AnchorStyles]::Bottom -bor [System.Windows.Forms.AnchorStyles]::Right)
$Select_Button.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]516,[System.Int32]301))
$Select_Button.Name = [System.String]'Select_Button'
$Select_Button.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]96,[System.Int32]32))
$Select_Button.TabIndex = [System.Int32]2
$Select_Button.Text = [System.String]'OK'
$Select_Button.UseVisualStyleBackColor = $true
$Select_Button.add_Click($Select_Button_Click)
#
#Refresh_Button
#
$Refresh_Button.Anchor = ([System.Windows.Forms.AnchorStyles][System.Windows.Forms.AnchorStyles]::Bottom -bor [System.Windows.Forms.AnchorStyles]::Left)
$Refresh_Button.BackColor = [System.Drawing.SystemColors]::Control
$Refresh_Button.ForeColor = [System.Drawing.Color]::FromArgb(([System.Int32]([System.Byte][System.Byte]0)),([System.Int32]([System.Byte][System.Byte]0)),([System.Int32]([System.Byte][System.Byte]0)))

$Refresh_Button.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]12,[System.Int32]301))
$Refresh_Button.Name = [System.String]'Refresh_Button'
$Refresh_Button.RightToLeft = [System.Windows.Forms.RightToLeft]::No
$Refresh_Button.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]96,[System.Int32]32))
$Refresh_Button.TabIndex = [System.Int32]2
$Refresh_Button.Text = [System.String]'Refresh'
$Refresh_Button.UseVisualStyleBackColor = $true
$Refresh_Button.add_Click($Refresh_Button_Click)
#
#SelectedImageDetailsGroup
#
$SelectedImageDetailsGroup.Anchor = ([System.Windows.Forms.AnchorStyles][System.Windows.Forms.AnchorStyles]::Top -bor [System.Windows.Forms.AnchorStyles]::Bottom -bor [System.Windows.Forms.AnchorStyles]::Left -bor [System.Windows.Forms.AnchorStyles]::Right)
$SelectedImageDetailsGroup.Controls.Add($ImageDetailsTB)
$SelectedImageDetailsGroup.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]12,[System.Int32]117))
$SelectedImageDetailsGroup.Name = [System.String]'SelectedImageDetailsGroup'
$SelectedImageDetailsGroup.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]600,[System.Int32]178))
$SelectedImageDetailsGroup.TabIndex = [System.Int32]3
$SelectedImageDetailsGroup.TabStop = $false
$SelectedImageDetailsGroup.Text = [System.String]'Selected Image Details'
#
#ImageDetailsTB
#
$ImageDetailsTB.Dock = [System.Windows.Forms.DockStyle]::Fill
$ImageDetailsTB.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Segoe UI',[System.Single]9,[System.Drawing.FontStyle]::Regular,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$ImageDetailsTB.Location = (New-Object -TypeName System.Drawing.Point -ArgumentList @([System.Int32]3,[System.Int32]23))
$ImageDetailsTB.Multiline = $true
$ImageDetailsTB.Name = [System.String]'ImageDetailsTB'
$ImageDetailsTB.ReadOnly = $true
$ImageDetailsTB.ScrollBars = [System.Windows.Forms.ScrollBars]::Vertical
$ImageDetailsTB.Size = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]594,[System.Int32]152))
$ImageDetailsTB.TabIndex = [System.Int32]0
#
#WDSHCImagePicker
#
$WDSHCImagePicker.ClientSize = (New-Object -TypeName System.Drawing.Size -ArgumentList @([System.Int32]624,[System.Int32]345))
$WDSHCImagePicker.ControlBox = $false
$WDSHCImagePicker.Controls.Add($SelectedImageDetailsGroup)
$WDSHCImagePicker.Controls.Add($Select_Button)
$WDSHCImagePicker.Controls.Add($TableLayoutPanel1)
$WDSHCImagePicker.Controls.Add($Label1)
$WDSHCImagePicker.Controls.Add($Refresh_Button)
$WDSHCImagePicker.Font = (New-Object -TypeName System.Drawing.Font -ArgumentList @([System.String]'Segoe UI',[System.Single]11.25,[System.Drawing.FontStyle]::Regular,[System.Drawing.GraphicsUnit]::Point,([System.Byte][System.Byte]0)))
$WDSHCImagePicker.FormBorderStyle = [System.Windows.Forms.FormBorderStyle]::FixedSingle
$WDSHCImagePicker.ShowIcon = $false
$WDSHCImagePicker.SizeGripStyle = [System.Windows.Forms.SizeGripStyle]::Hide
$WDSHCImagePicker.StartPosition = [System.Windows.Forms.FormStartPosition]::CenterScreen
$WDSHCImagePicker.Text = [System.String]'Choose the installation image to deploy'
$WDSHCImagePicker.add_Load($WDSHCImagePicker_Load)
$TableLayoutPanel1.ResumeLayout($false)
$TableLayoutPanel1.PerformLayout()
$SelectedImageDetailsGroup.ResumeLayout($false)
$SelectedImageDetailsGroup.PerformLayout()
$WDSHCImagePicker.ResumeLayout($false)
$WDSHCImagePicker.PerformLayout()
Add-Member -InputObject $WDSHCImagePicker -Name Label1 -Value $Label1 -MemberType NoteProperty
Add-Member -InputObject $WDSHCImagePicker -Name Label2 -Value $Label2 -MemberType NoteProperty
Add-Member -InputObject $WDSHCImagePicker -Name TableLayoutPanel1 -Value $TableLayoutPanel1 -MemberType NoteProperty
Add-Member -InputObject $WDSHCImagePicker -Name Label3 -Value $Label3 -MemberType NoteProperty
Add-Member -InputObject $WDSHCImagePicker -Name ImgNameCombo -Value $ImgNameCombo -MemberType NoteProperty
Add-Member -InputObject $WDSHCImagePicker -Name ImgGroupCombo -Value $ImgGroupCombo -MemberType NoteProperty
Add-Member -InputObject $WDSHCImagePicker -Name Select_Button -Value $Select_Button -MemberType NoteProperty
Add-Member -InputObject $WDSHCImagePicker -Name Refresh_Button -Value $Refresh_Button -MemberType NoteProperty
Add-Member -InputObject $WDSHCImagePicker -Name SelectedImageDetailsGroup -Value $SelectedImageDetailsGroup -MemberType NoteProperty
Add-Member -InputObject $WDSHCImagePicker -Name ImageDetailsTB -Value $ImageDetailsTB -MemberType NoteProperty
}
. InitializeComponent
