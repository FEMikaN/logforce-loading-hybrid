rem Update the file references in KotkaApp.csproj
rem All files under 'content' directory are added
pushd FifthElement.KotkaApp
cscript updatecontent.vbs KotkaApp.csproj content
cscript ModifyConfig.vbs %1
popd
rem done vbscript
pause