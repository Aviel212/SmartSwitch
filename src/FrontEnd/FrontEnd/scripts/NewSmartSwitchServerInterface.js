/// <reference path="jquery.js" />

let usersApi = "/users", plugsApi = "/plugs", tasksApi = "/tasks", pusApi = "/powerUsageSamples";
let server = "https://backend.conveyor.cloud/api";

// get a user
function getUser(userName, pass) {
    $.ajax({
        dataType: "json",
        url: server + user + "/" + userName,
        contentType: "application/json; charset=utf-8",
        type: "GET",
        success: function (data) {
            console.log("Got User Successfully");

        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log("ERROR - Couldn't Get User");

        }
    });

}
// adds a user
function addUser(userName, pass) {
    $.ajax({
        dataType: "json",
        url: server + user + "/" + userName + "/" + pass,
        contentType: "application/json; charset=utf-8",
        type: "PUT",
        success: function (data) {
            console.log("User Added Successfully");

        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log("ERROR - User Wasn't Added Properly");

        }
    });
}

// remove a user
function removeUser(userName) {

}


