Set args = Wscript.Arguments
appConfig = "App.config"

if args.count>0 then environment = args.Item(0)
if environment="" then environment = "test"

Set settingsDictionary = CreateObject("Scripting.Dictionary")
if environment="test" then
  settingsDictionary.Add "RestServiceUrlGet","http://146.119.33.89/MG0472_KotkaRestJson_gateway/http/json"
  settingsDictionary.Add "RestServiceUrlSave","http://146.119.33.89/MG0472_KotkaRestJsonSave_gateway/http/json"
  settingsDictionary.Add "RestServiceUrlGis","http://146.119.33.89/MG0472_KotkaRestJsonGis_gateway/http/json"
  settingsDictionary.Add "RestServiceUrlKtj","http://146.119.33.89/MG0450_EstateOwner_gateway/http/gw"
  settingsDictionary.Add "RestServiceUrlGisMapSymbols","http://146.119.33.89/MG0582_mapsymbol_management_gateway/http/MG0582_mapsymbol_management_HttpGateway/MG0582_IncomingRestMessage"
  settingsDictionary.Add "RestServiceIlves","http://146.119.235.26/ilves-api/servlet/stands?"
  settingsDictionary.Add "WfsServiceKotka","http://146.119.74.232/services/geometry/WFS/Kotka?"
  settingsDictionary.Add "MapserverAuthority","146.119.74.232"
  settingsDictionary.Add "MapPackageIndex","http://146.119.33.32/maps/available_packages.geojson"
  settingsDictionary.Add "environmentText","TESTIVERSIO"
elseif environment="prod" then
  settingsDictionary.Add "RestServiceUrlGet","http://146.119.33.95/MG0472_KotkaRestJson_gateway/http/json"
  settingsDictionary.Add "RestServiceUrlSave","http://146.119.33.95/MG0472_KotkaRestJsonSave_gateway/http/json"
  settingsDictionary.Add "RestServiceUrlGis","http://146.119.33.95/MG0472_KotkaRestJsonGis_gateway/http/json"
  settingsDictionary.Add "RestServiceUrlKtj","http://146.119.33.95/MG0450_EstateOwner_gateway/http/gw"
  settingsDictionary.Add "RestServiceUrlGisMapSymbols","http://146.119.33.95/MG0582_mapsymbol_management_gateway/http/MG0582_mapsymbol_management_HttpGateway/MG0582_IncomingRestMessage"
  settingsDictionary.Add "RestServiceIlves","http://146.119.235.51/ilves-api/servlet/stands?"
  settingsDictionary.Add "WfsServiceKotka","http://146.119.74.230/services/geometry/WFS/Kotka?"
  settingsDictionary.Add "MapserverAuthority","146.119.74.230"
  settingsDictionary.Add "MapPackageIndex","http://146.119.33.32/maps/available_packages.geojson"
  settingsDictionary.Add "environmentText","TUOTANTOVERSIO"
end if

' Read App.config file
Set objXMLDoc = CreateObject("Microsoft.XMLDOM") 
objXMLDoc.async = False 
objXMLDoc.load(appConfig)
Call SetParameters(settingsDictionary)
objXMLDoc.save(appConfig)


Sub SetParameters(settings)
  Set contentNodes = objXMLDoc.selectNodes ("//configuration/appSettings/add")
  For Each contentNode in contentNodes
    key = contentNode.Attributes.getNamedItem("key").Text
	if settings.Exists(key) then
		contentNode.Attributes.getNamedItem("value").Text = settings.item(key)
	end if
  Next
End Sub
