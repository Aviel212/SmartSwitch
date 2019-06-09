// requires jQuery
let server = "https://backend.conveyor.cloud/api";
let usersApi = server +  "/users";
let plugsApi = server + "/plugs";
let samplesApi = server + "/powerusagesamples";
let tasksApi = server + "/tasks";

/** Defines the possible operations we can call on a plug, either turn it on or off.
 * @enum Operations
 * @property {integer} TurnOn   The operation that calls for a plug to turn on.
 * @property {integer} TurnOff  The operation that calls for a plug to turn off.
 * */
let Operations = Object.freeze({ TurnOn: 0, TurnOff: 1 });

/** Defines the possible types a task can, either repeating or activating just once.
 * @enum TaskTypes
 * @property {integer} OneTime  The type of task that executes once.
 * @property {integer} Repeated The type of task that executes repeatedly.
 * */
let TaskTypes = Object.freeze({ OneTime: 0, Repeated: 1 });

/** Defines the possible priorities a plug can have, a plug can be essential, non-essential or neither (what we call irrelevant).
 * @enum Priorities
 * @property {integer} Essential        The priority that states a plug is essential.
 * @property {integer} Nonessential     The priority that states a plug is non-essential.
 * @property {integer} IRRELEVANT       The priority that states a plug's priority is irrelevant.
 * */
let Priorities = Object.freeze({ Essential: 0, Nonessential: 1, Irrelevant: 2 });

/**
 * Gets a user JSON containing its username and password.
 * @param {string}      username            The user's username.
 * @param {function}    successFunction     Function to execute upon success.
 * @param {function=}   errorFunction       Function to execute upon failure.
 * @param {function=}   completeFunction    Function to execute upon completion.
 */
function getUser(username, successFunction, errorFunction, completeFunction) {
    if (username === undefined || successFunction === undefined) return;

    let request = {
        url: usersApi + "/" + username,
        method: "GET",
        success: successFunction
    };

    if (errorFunction !== undefined) request.error = errorFunction;
    if (completeFunction !== undefined) request.complete = completeFunction;

    $.ajax(request);
}

/**
 * Adds a new user.
 * @param {string}      username            The new user's username.
 * @param {string}      password            The new user's password.
 * @param {function=}   successFunction     Function to execute upon success.
 * @param {function=}   errorFunction       Function to execute upon failure.
 * @param {function=}   completeFunction    Function to execute upon completion.
 */
function addUser(username, password, successFunction, errorFunction, completeFunction) {
    if (username === undefined || password === undefined) return;

    let request = {
        url: usersApi,
        contentType: "application/json",
        method: "POST",
        data: JSON.stringify({
            "Username": username,
            "Password": password
        })
    };

    if (successFunction !== undefined) request.success = successFunction;
    if (errorFunction !== undefined) request.error = errorFunction;
    if (completeFunction !== undefined) request.complete = completeFunction;

    $.ajax(request);
}

/**
 * Changes an existing user's password.
 * @param {string}      username            The existing user's username.
 * @param {string}      password            The existing user's password.
 * @param {function=}   successFunction     Function to execute upon success.
 * @param {function=}   errorFunction       Function to execute upon failure.
 * @param {function=}   completeFunction    Function to execute upon completion.
 */
function changePassword(username, password, successFunction, errorFunction, completeFunction) {
    if (username === undefined || password === undefined) return;

    let request = {
        url: usersApi + "/" + username + "/password",
        contentType: "application/json",
        method: "PUT",
        data: JSON.stringify(password)
    };

    if (successFunction !== undefined) request.success = successFunction;
    if (errorFunction !== undefined) request.error = errorFunction;
    if (completeFunction !== undefined) request.complete = completeFunction;

    $.ajax(request);
}

/**
 * Gets a plug JSON containing its mac, nickname, isOn, approved, priority and addedAt properties.
 * @param {string}      mac                 The plug's mac address.
 * @param {function}    successFunction     Function to execute upon success.
 * @param {function=}   errorFunction       Function to execute upon failure.
 * @param {function=}   completeFunction    Function to execute upon completion.
 */
function getPlug(mac, successFunction, errorFunction, completeFunction) {
    if (mac === undefined || successFunction === undefined) return;

    let request = {
        url: plugsApi + "/" + mac,
        method: "GET",
        success: function (data, textStatus, jqXHR) {
            data.addedAt = new Date(data.addedAt);
            successFunction(data, textStatus, jqXHR);
        }
    };

    if (errorFunction !== undefined) request.error = errorFunction;
    if (completeFunction !== undefined) request.complete = completeFunction;

    $.ajax(request);
}

/**
 * Gets an array plug JSONs owned by a given user containing their mac, nickname, isOn, approved, priority and addedAt properties.
 * @param {string}      username            The username of the owner of the plugs.
 * @param {function}    successFunction     Function to execute upon success.
 * @param {function=}   errorFunction       Function to execute upon failure.
 * @param {function=}   completeFunction    Function to execute upon completion.
 */
function getUserPlugs(username, successFunction, errorFunction, completeFunction) {
    if (username === undefined || successFunction === undefined) return;

    let request = {
        url: plugsApi + "/user/" + username,
        method: "GET",
        success: function (data, textStatus, jqXHR) {
            for (let i in data) {
                data[i].addedAt = new Date(data[i].addedAt);
            }
            successFunction(data, textStatus, jqXHR);
        }
    };

    if (errorFunction !== undefined) request.error = errorFunction;
    if (completeFunction !== undefined) request.complete = completeFunction;

    $.ajax(request);
}

/**
 * Updates a given plug's nickname and priority properties.
 * 
 * All properties of the plug will be updated (nickname and priority).
 * @param {object}      plugProperties      A JSON containing the plug's properties to be updated.
 * @param {function=}   successFunction     Function to execute upon success.
 * @param {function=}   errorFunction       Function to execute upon failure.
 * @param {function=}   completeFunction    Function to execute upon completion.
 * @example
 * let plugProperties = {
 *     "mac": "BC:DD:B2:23:D6:60",
 *     "nickname": "Fan",
 *     "priority": Priorities.Nonessential
 * }
 * 
 * function print(data) {
 *     console.log(JSON.stringify(data));
 * }
 * 
 * updatePlug(plugProperties, print);
 */
function updatePlug(plugProperties, successFunction, errorFunction, completeFunction) {
    if (plugProperties === undefined) return;

    let request = {
        url: plugsApi,
        method: "PUT",
        contentType: "application/json",
        data: JSON.stringify(plugProperties)
    };

    if (successFunction !== undefined) request.success = successFunction;
    if (errorFunction !== undefined) request.error = errorFunction;
    if (completeFunction !== undefined) request.complete = completeFunction;

    $.ajax(request);
}

/**
 * Turns a given plug on or off.
 * @param {string}      mac                 The plug's mac address.
 * @param {integer}     _operation          The operation to perform, either Operations.TURNON or Operations.TURNOFF.
 * @param {function=}   successFunction     Function to execute upon success.
 * @param {function=}   errorFunction       Function to execute upon failure.
 * @param {function=}   completeFunction    Function to execute upon completion.
 * @example turnPlug("BC:DD:BD:23:D6:60", Operations.TurnOn);
 */
function turnPlug(mac, _operation, successFunction, errorFunction, completeFunction) {
    if (mac === undefined || _operation === undefined) return;

    let request = {
        url: plugsApi + "/" + mac + "?op=" + _operation,
        method: "PUT"
    };

    if (successFunction !== undefined) request.success = successFunction;
    if (errorFunction !== undefined) request.error = errorFunction;
    if (completeFunction !== undefined) request.complete = completeFunction;

    $.ajax(request);
}

/**
 * Approves a given plug.
 * @param {string}      mac                 The plug's mac address.
 * @param {function=}   successFunction     Function to execute upon success.
 * @param {function=}   errorFunction       Function to execute upon failure.
 * @param {function=}   completeFunction    Function to execute upon completion.
 */
function approvePlug(mac, successFunction, errorFunction, completeFunction) {
    if (mac === undefined) return;

    let request = {
        url: plugsApi + "/" + mac + "?approved=true",
        method: "PUT"
    };

    if (successFunction !== undefined) request.success = successFunction;
    if (errorFunction !== undefined) request.error = errorFunction;
    if (completeFunction !== undefined) request.complete = completeFunction;

    $.ajax(request);
}

/**
 * Denies a given plug.
 * @param {string}      mac                 The plug's mac address.
 * @param {function=}   successFunction     Function to execute upon success.
 * @param {function=}   errorFunction       Function to execute upon failure.
 * @param {function=}   completeFunction    Function to execute upon completion.
 */
function denyPlug(mac, successFunction, errorFunction, completeFunction) {
    if (mac === undefined) return;

    let request = {
        url: plugsApi + "/" + mac + "?approved=false",
        method: "PUT"
    };

    if (successFunction !== undefined) request.success = successFunction;
    if (errorFunction !== undefined) request.error = errorFunction;
    if (completeFunction !== undefined) request.complete = completeFunction;

    $.ajax(request);
}

/**
 * Gets an array of powerUsageSample JSONs of the recent given number of samples of a given plug, containing their sampleDate, current and voltage properties.
 * 
 * The array will be ordered by date, most recent first. Each sample is for one minute following its date.
 * @param {string}      mac                 The plug's (whose samples are requested) mac address.
 * @param {integer}     amount              The amount of recent samples. A strictly positive number.
 * @param {function}    successFunction     Function to execute upon success.
 * @param {function=}   errorFunction       Function to execute upon failure.
 * @param {function=}   completeFunction    Function to execute upon completion.
 * @example
 * getPlugSamples("BB:DD:C2:23:D6:60", 7, function (samples) {
 *     for (let i in samples) {
 *         console.log(samples[i]);
 *     }
 * })
 */
function getPlugSamples(mac, amount, successFunction, errorFunction, completeFunction) {
    if (mac === undefined || amount === undefined || successFunction === undefined || amount <= 0) return;

    let request = {
        url: samplesApi + "/plug/" + mac + "?amount=" + amount,
        method: "GET",
        success: function (data, textStatus, jqXHR) {
            for (let i in data) {
                data[i].sampleDate = new Date(data[i].sampleDate);
            }
            successFunction(data, textStatus, jqXHR);
        }
    };

    if (errorFunction !== undefined) request.error = errorFunction;
    if (completeFunction !== undefined) request.complete = completeFunction;

    $.ajax(request);
}

/**
 * Gets an array of task JSONs of all tasks of a given plug, containing their operation, deviceMac, taskType, repeatEvery and startDate.
 * @param {string}      mac                 The plug's (whose tasks are requested) mac address.
 * @param {function}    successFunction     Function to execute upon success.
 * @param {function=}   errorFunction       Function to execute upon failure.
 * @param {function=}   completeFunction    Function to execute upon completion.
 */
function getPlugTasks(mac, successFunction, errorFunction, completeFunction) {
    if (mac === undefined || amount === undefined || successFunction === undefined || amount <= 0) return;

    let request = {
        url: tasksApi + "/plug/" + mac,
        method: "GET",
        success: function (data, textStatus, jqXHR) {
            for (let i in data) {
                data[i].startDate = new Date(data[i].startDate);
            }
            successFunction(data, textStatus, jqXHR);
        }
    };

    if (errorFunction !== undefined) request.error = errorFunction;
    if (completeFunction !== undefined) request.complete = completeFunction;

    $.ajax(request);
}


/**
 * Adds a new task. Task is an object with the following properties: operation, deviceMac, taskType, repeatEvery and startDate.
 * @param {object}      task                The new task object.
 * @param {function=}   successFunction     Function to execute upon success.
 * @param {function=}   errorFunction       Function to execute upon failure.
 * @param {function=}   completeFunction    Function to execute upon completion.
 * @example
 * // a task that turns a plug on every day
 * let newTask = {
 *     operation: Operations.TurnOn,
 *     deviceMac: "BC:DD:C2:23:D6:69",
 *     taskType: TaskTypes.Repeated,
 *     repeatEvery: 60 * 24,
 *     startDate: new Date()
 * }
 * 
 * // a task that will turn a plug off in 5 minutes
 * let d = new Date();
 * d.setMinutes(d.getMinutes() + 5);
 * let newTask = {
 *     operation: Operations.TurnOff,
 *     deviceMac: "BC:DD:C2:28:D6:60",
 *     taskType: TaskTypes.OneTime,
 *     startDate: d
 * }
 * 
 * addTask(newTask);
 */
function addTask(task, successFunction, errorFunction, completeFunction) {
    if (task === undefined) return;

    let taskToSend = {};
    Object.assign(taskToSend, task);
    taskToSend.startDate = correctToJSON(taskToSend.startDate);

    let request = {
        url: tasksApi,
        method: "POST",
        contentType: "application/json",
        data: JSON.stringify(taskToSend)
    };

    if (successFunction !== undefined) request.success = successFunction;
    if (errorFunction !== undefined) request.error = errorFunction;
    if (completeFunction !== undefined) request.complete = completeFunction;

    $.ajax(request);
}

/** Calculates a correct dateString including time zone.
 * 
 * Private function for date calculation, taken from: https://stackoverflow.com/a/36643588/7414734.
 * @access private
 * @param       {Date}      date    A date object to convert to a dateString with time zone.
 * @returns     {string}            A dateString containg all information about the given date including time zone.
 * */ 
function correctToJSON(date) {
    var timezoneOffsetInHours = -(date.getTimezoneOffset() / 60); //UTC minus local time
    var sign = timezoneOffsetInHours >= 0 ? '+' : '-';
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