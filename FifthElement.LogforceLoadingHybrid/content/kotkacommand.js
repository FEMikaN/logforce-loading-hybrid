/**
* A JavaScript-side wrapper for KotkaCommand command
*/
function KotkaCommand() {

    var exec = function (success, error, method, args) {
        cordova.exec(success, error, "LogforceLoadingCommand", method, args);
        document.getElementById("JsonInput").innerHTML = JSON.stringify(args);
        document.getElementById("KotkaCommand").innerHTML = method;
    };

    this.executeCommand = function (success, error, command, args) {
        exec(success, error, command, JSON.parse(args));
    }

    this.executeNetworkCommand = function (success, error, command, args) {
        cordova.exec(success, error, "NetworkStateCommand", command, JSON.parse(args));
        document.getElementById("JsonInput").innerHTML = JSON.stringify(args);
        document.getElementById("KotkaCommand").innerHTML = method;
    }
    this.executeMapPackagesCommand = function (success, error, command, args) {
        cordova.exec(success, error, "MapPackagesCommand", command, JSON.parse(args));
        document.getElementById("JsonInput").innerHTML = JSON.stringify(args);
        document.getElementById("KotkaCommand").innerHTML = method;
    }


    this.generateKeyValueList = function (success, error, tableName, keyFieldName, valueFieldName) {
        exec(success, error, "GenerateKeyValueList", {
            tableName: tableName,
            keyFieldName: keyFieldName,
            valueFieldName: valueFieldName
        });
    }

    this.generateError = function (success, error) {
        exec(success, error, "GenerateError");
    }



};