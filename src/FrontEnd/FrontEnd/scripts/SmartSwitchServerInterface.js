/// <reference path="jquery.js" />
//let server = "https://backend.conveyor.cloud/api";
let server = "http://localhost:50750/api";
let usersApi = "/users";
let plugsApi = "/plugs";
let tasksApi = "/tasks";

$.ajaxSetup({ async: false});

// adds a user to the system.
// returns "ok" if added successfully or "username exists" if the username is taken.
function addUser(username, password) {
    let returnString;
    $.post(server + usersApi + "/add/" + username + "/" + password, function (data) {
        if (data.startsWith("added")) returnString = "ok";
        else if (data === "username exists") returnString = data;
    });
    return returnString;
}

// returns a string array containing all usernames
function getAllUsernames() {
    let returnStringArray;
    $.get(server + usersApi, function (data) {
        returnStringArray = data;
    });
    return returnStringArray;
}

// removes a user
// returns "no such user" if the user does not exist, "wrong password" if the given password was
// incorrect and "ok" if removed successfully
function removeUser(username, password) {
    let returnString;
    $.post(server + usersApi + "/remove/" + username + "/" + password, function (data) {
        if (data.startsWith("removed")) returnString = "ok";
        else returnString = data;
    });
    return returnString;
}

// changes a user's password
// returns "no such user" if the user does not exist and "ok" if changed password successfully
function changePassword(username, newPassword) {
    let returnString;
    $.post(server + usersApi + "/change-password/" + username + "/" + newPassword, function (data) {
        if (data === "password changed") returnString = "ok";
        else returnString = data;
    });
    return returnString;
}

// returns "no such user" if there is no user with the given username, "incorrect password" if the password doesn't match
// the user's password and a JSON of the user otherwise
function getUser(username, password) {
    let returnData;
    $.get(server + usersApi + "/" + username + "/" + password, function (data) {
        if (data === "no such user" || data === "incorrect password") returnData = data;
        else returnData = JSON.parse(data);
    });
    return returnData;
}

// returns a JSON of the plug
function getPlug(mac) {
    let plug;
    $.get(server + plugsApi + "/" + mac, function (data) {
        plug = JSON.parse(data);
    });
    return plug;

}
///*+ "/" + username*/
//function getPlugs() {
//    let plugs;
//    let username = "/tal";
//    $.get(server + usersApi + "/plugs", function (data) {
//        plugs = data;//JSON.parse(data);
//    });
//    return plugs;
//}

// turns a plug on
// returns "no such plug" if the plug doesn't exist, "plug not connected" if the plug is not
// connected to the server and "ok" if turned on successfully
function turnPlugOn(mac) {
    let returnString;
    $.post(server + plugsApi + "/" + mac + "/ison/true", function (data) {
        if (data === "turned on") returnString = "ok";
        else returnString = data;
    });
    return returnString;
}

// turns a plug off
// returns "no such plug" if the plug doesn't exist, "plug not connected" if the plug is not
// connected to the server and "ok" if turned ff successfully
function turnPlugOff(mac) {
    let returnString;
    $.post(server + plugsApi + "/" + mac + "/ison/false", function (data) {
        if (data === "turned off") returnString = "ok";
        else returnString = data;
    });
    return returnString;
}

// approves a given plug
// returns "no such plug" if the plug doesn't exist and "ok" if the plug was approved successfully
function approvePlug(mac) {
    let returnString;
    $.post(server + plugsApi + "/" + mac + "/approved/true", function (data) {
        returnString = data;
    });
    return returnString;
}

// removes/deletes a plug
// returns "no such plug" if the plug doesn't exist and "ok" if the plug was removed successfully
function removePlug(mac) {
    let returnString;
    $.post(server + plugsApi + "/" + mac + "/approved/false", function (data) {
        returnString = data;
    });
    return returnString;
}

// sets a plug's Approved property as false (effectively delets the plug)
// returns "no such plug" if the plug doesn't exist and "ok" if the plug was denied successfully
function denyPlug(mac) {
    return removePlug(mac);
}

// sets a plug's Priority property
// returns "no such plug" if the plug doesn't exist, "no such priority" if priority
// is not "essential", "nonessential" nor "irrelevant" and "ok" if Priority set successfully
function setPlugPriority(mac, priority) {
    let returnString;
    $.post(server + plugsApi + "/" + mac + "/priority/" + priority, function (data) {
        if (data === "value not recognized") returnString = "no such priority";
        else returnString = data;
    });
    return returnString;
}

// sets a plug's Nickname property
// returns "no such plug" if the plug doesn't exist and "ok" if Nickname set successfully
function setPlugNickname(mac, nickname) {
    let returnString;
    $.post(server + plugsApi + "/" + mac + "/nickname/" + nickname, function (data) {
        returnString = data;
    });
    return returnString;
}

let Operations = { TURNON: "TURNON", TURNOFF: "TURNOFF" };

// adds a OneTimeTask to the plug mac, executing operation op (either Operations.TURNON or Operations.TURNOFF) at date dateToBeExecuted (a JavaScript Date object)
// example: addOneTimeTask("61:0d:83:b1:c0:8e", Operations.TURNOFF, new Date(2019, 4, 31, 17, 20, 30));
function addOneTimeTask(mac, op, dateToBeExecuted) {
    $.ajax({
        url: server + tasksApi + "/" + JSON.stringify({ "Mac": mac, "Operation": op, "DateToBeExecuted": correctToJSON(dateToBeExecuted) }),
        type: "POST",
        async: true
    });
    console.log(server + tasksApi + "/" + JSON.stringify({ "Mac": mac, "Operation": op, "DateToBeExecuted": correctToJSON(dateToBeExecuted) }));
}

// adds a RepeatedTask to the plug mac, executing operation op (either Operations.TURNON or Operations.TURNOFF) starting at date startDate (a JavaScript Date object)
// and repeating every repeatEvery minutes
// example: addRepeatedTask("61:0d:83:b1:c0:8e", Operations.TURNON, new Date(2019, 4, 31, 17, 20, 30), 240);
function addRepeatedTask(mac, op, startDate, repeatEvery) {
    $.ajax({
        url: server + tasksApi + "/" + JSON.stringify({ "Mac": mac, "Operation": op, "StartDate": correctToJSON(startDate), "RepeatEvery": repeatEvery }),
        type: "POST",
        async: true
    });
}

// private function for date calculation, taken from: https://stackoverflow.com/a/36643588/7414734
function correctToJSON(date) {
    var timezoneOffsetInHours = -(date.getTimezoneOffset() / 60); //UTC minus local time
    var sign = timezoneOffsetInHours >= 0 ? 'x' : '-';
    var leadingZero = Math.abs(timezoneOffsetInHours) < 10 ? '0' : '';

    //It's a bit unfortunate that we need to construct a new Date instance 
    //(we don't want _this_ Date instance to be modified)
    var correctedDate = new Date(date.getFullYear(), date.getMonth(),
        date.getDate(), date.getHours(), date.getMinutes(), date.getSeconds(),
        date.getMilliseconds());
    correctedDate.setHours(date.getHours() + timezoneOffsetInHours);
    var iso = correctedDate.toISOString().replace('Z', '');

    return iso + sign + leadingZero + Math.abs(timezoneOffsetInHours).toString() + ':00';
}