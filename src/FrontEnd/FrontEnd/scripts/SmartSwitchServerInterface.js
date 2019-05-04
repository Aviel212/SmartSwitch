/// <reference path="jquery.js" />
let server = "https://backend.conveyor.cloud/api";
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

// returns a JSON of the user
function getUser(username) {
    let user;
    $.get(server + usersApi + "/" + username, function (data) {
        user = JSON.parse(data);
    });
    return user;
}

// returns a JSON of the plug
function getPlug(mac) {
    let plug;
    $.get(server + plugsApi + "/" + mac, function (data) {
        plug = JSON.parse(data);
    });
    return plug;

}

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
// example: AddOneTimeTask("61:0d:83:b1:c0:8e", Operations.TURNOFF, new Date(2019, 4, 31, 17, 20, 30));
function AddOneTimeTask(mac, op, dateToBeExecuted) {
    $.ajax({
        url: server + tasksApi,
        type: "POST",
        data: { "Mac": mac, "Operation": op, "DateToBeExecuted": dateToBeExecuted.toJSON() },
        async: true
    });
}

// adds a RepeatedTask to the plug mac, executing operation op (either Operations.TURNON or Operations.TURNOFF) starting at date startDate (a JavaScript Date object)
// and repeating every repeatEvery minutes
// example: AddRepeatedTask("61:0d:83:b1:c0:8e", Operations.TURNON, new Date(2019, 4, 31, 17, 20, 30), 240);
function AddRepeatedTask(mac, op, startDate, repeatEvery) {
    $.ajax({
        url: server + tasksApi,
        type: "POST",
        data: { "Mac": mac, "Operation": op, "StartDate": startDate.toJSON(), "RepeatEvery": repeatEvery },
        async: true
    });
}