/// <reference path="jquery.js" />

let usersApi = "/users", plugsApi = "/plugs", tasksApi = "/tasks", pusApi = "/powerUsageSamples";
let server = "https://backend.conveyor.cloud/api";

// get a user -
// returns a user object if success, for example: {"yarden":"12345678"}
// returns status code and error message if failed, for example: "404 - Page Couldn't Be Found"
function getUser(userName, pass) {
    $.ajax({
        dataType: "json",
        url: server + usersApi + "/" + userName,
        contentType: "application/json; charset=utf-8",
        type: "GET",
        success: function (data) {
            console.log("Got User Successfully");
            return data;
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log("ERROR - Couldn't Get User");
            return textStatus + " - " + errorThrown;
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
    $.ajax({
        dataType: "json",
        url: server + usersApi + "/" + username,
        contentType: "application/json; charset=utf-8",
        type: "DELETE",
        success: function (data) {
            console.log("Removed User Successfully");

        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.log("ERROR - Couldn't Remove User");

        }
    });
}


