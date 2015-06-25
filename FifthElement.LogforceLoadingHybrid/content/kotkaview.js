//Provides functionality to the sample ui

function debugAddBriefcaseSuccess(dummy) {
    return dummy;
}
function debugAddBriefcaseError() {
    return JSON.stringify(arguments);
}


function KotkaView(kotkaCommand) {

    //some basic utils
    var el = function (id) { return document.getElementById(id); },
        click = function (id, callback) { el(id).addEventListener("click", callback); },
        get = function (id, content) { return el(id).value },
        set = function (id, content) { el(id).innerHTML = content; },
        error = function () { set("error", "ERROR:" + JSON.stringify(arguments)); };


    click("testMemory", function () {
        var params = { memorySize: get("memorySize") };
        kotkaCommand.executeCommand(function (result) {
            set("JsonOutput", JSON.stringify(result));
        }, error, "TestMemory", JSON.stringify(params));
    });

    click("getMultipleResponses", function () {
        var params = {};
        kotkaCommand.executeCommand(function (result) {
            set("multipleResponsesCounter", JSON.stringify(result));
        }, error, "GetMultipleResponses", JSON.stringify(params));
    });


    // Database access

    click("getData", function () {
        var params = { key: get("keyValueGet") };
        kotkaCommand.executeCommand(
            function (result) { set("JsonOutput", JSON.stringify(result)); },
            function (result) { set("JsonOutputError", result); },
            "GetData",
            JSON.stringify(params)
        );
    });

    click("saveData", function () {
        var params = { key: get("keyValueSave"), infoText: get("dataText"), infoCode: get("dataInt") };
        kotkaCommand.executeCommand(
            function (result) { set("JsonOutput", JSON.stringify(result)); },
            function (result) { set("JsonOutputError", result); },
            "SaveData",
            JSON.stringify(params)
        );
    });

    click("getList", function () {
        var params = { };
        kotkaCommand.executeCommand(
            function (result) { set("JsonOutput", JSON.stringify(result)); },
            function (result) { set("JsonOutputError", result); },
            "GetList",
            JSON.stringify(params)
        );
    });


    click("generateError", function () {
        var params = {};
        kotkaCommand.executeCommand(
            function (result) { set("JsonOutput", JSON.stringify(result)); },
            function (result) { set("JsonOutputError", result); },
            "GenerateError",
            JSON.stringify(params)
        );
    });


    // Network commands
        click("startListener", function () {
            var params = { listenerId : "mytest"};
            kotkaCommand.executeNetworkCommand(function (result) {
                set("listenerResponse", JSON.stringify(result));
            }, error, "AddWatch", JSON.stringify(params));
        });
        click("stopListener", function () {
            var params = { listenerId: "mytest"  };
            kotkaCommand.executeNetworkCommand(function (result) {
                set("listenerResponse", JSON.stringify(result));
            }, error, "ClearWatch", JSON.stringify(params));
        });
    
   

};