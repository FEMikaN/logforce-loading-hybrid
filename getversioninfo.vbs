Function GetFileContents(filename) 
	Const ForReading = 1
	Set objFSO = CreateObject("Scripting.FileSystemObject")
	Set objTextFile = objFSO.OpenTextFile(filename, ForReading)
	GetFileContents = objTextFile.ReadAll
	objTextFile.Close
End Function

Function GetClientVersion(content)
	Set re = New RegExp
	With re
		.Pattern    = ".+""__version__"":.\D([0-9]+\.[0-9]+\.[0-9]+).+"
		.IgnoreCase = False
		.Global     = False
	End With
	Set matches = re.Execute(content)
	if matches.count>0 then
		if matches(0).SubMatches.Count>0 then
			GetClientVersion = matches(0).SubMatches(0)
		end if  
	end if
End Function

Sub SaveClientVersion(version)
	Set objFSO=CreateObject("Scripting.FileSystemObject")
	Set objFile = objFSO.CreateTextFile("version.txt",True)
	objFile.Write version
	objFile.Close
End Sub

' Get commandline arguments
content = GetFileContents("FifthElement.KotkaApp\content\config\app.json")
version = GetClientVersion(content)
SaveClientVersion version