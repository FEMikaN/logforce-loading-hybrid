Function GetFileContents(filename) 
	Const ForReading = 1
	Set objFSO = CreateObject("Scripting.FileSystemObject")
	Set objTextFile = objFSO.OpenTextFile(filename, ForReading)
	GetFileContents = objTextFile.ReadAll
	objTextFile.Close
End Function

' Get commandline arguments
Set args = Wscript.Arguments
installerdir = WScript.Arguments.Item(0)
version = WScript.Arguments.Item(1)


kotkaversion = GetFileContents("version.txt")

Dim Fso
Set Fso = WScript.CreateObject("Scripting.FileSystemObject")
Fso.MoveFile installerdir & "SetupKotkaHybrid.msi", installerdir & "SetupKotkaHybrid_" & version & "_" & kotkaversion & ".msi"