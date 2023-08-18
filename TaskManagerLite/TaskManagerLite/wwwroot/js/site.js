"use strict";
var taskStyleStatusDict = {
    0: "task-new",
    1: "task-close-to-overdue",
    2: "task-overdue",
    3: "task-deleted",
    4: "task-finished", // Finished
};
var taskStatusDict = {
    0: "New",
    1: "< 1 day left",
    2: "Overdue",
    3: "Deleted",
    4: "Finished",
};
function Submit() {
    var textInput = document.getElementById("textInput");
    var deletedInput = document.getElementById('showDeletedInput');
    var expiredInput = document.getElementById('showExpiredInput');
    var finishedInput = document.getElementById('showFinishedInput');
    var request = new XMLHttpRequest();
    request.open("POST", "tasks");
    request.setRequestHeader('Content-Type', 'application/json'); // Set the appropriate content type
    request.onreadystatechange = function () {
        if (request.readyState === XMLHttpRequest.DONE) {
            if (request.status === 200) {
                var data = JSON.parse(request.responseText);
                DisplayTasks(data.tasks);
            }
        }
    };
    var json = JSON.stringify({
        "name": textInput.value,
        "showDeleted": deletedInput.checked,
        "showExpired": expiredInput.checked,
        "showFinished": finishedInput.checked
    });
    request.send(json);
}
function DisplayTasks(data) {
    var container = document.getElementById("tasks-list-container");
    container.innerHTML = "";
    for (var i = 0; i < data.length; i++) {
        var task = data[i];
        var subclass = taskStyleStatusDict[task.status];
        var status = taskStatusDict[task.status];
        var htmlTask = "";
        switch (task.status) {
            case 3:
                htmlTask = `<div class="task-container ${subclass}"><input class="task-input" id="${task.id}" value="${task.name}" onblur="ChangeName(this)"/><span class="status-label">${status}</span><button class="task-button" id="${task.id}" onclick="RestoreTask(this)"> Restore </button> <button class="task-button" onclick="OpenEditDialog(this)" id="${task.id}">Edit</button></div>`;
                break;
            case 4:
                htmlTask = `<div class="task-container ${subclass}"><input class="task-input" id="${task.id}" value="${task.name}" onblur="ChangeName(this)"/><span class="status-label">${status}</span><button class="task-button" id="${task.id}" onclick="ReopenTask(this)">Reopen</button><button class="task-button" id="${task.id}" onclick="OpenDeleteDialog(this)"> Delete </button> <button class="task-button" onclick="OpenEditDialog(this)" id="${task.id}">Edit</button></div>`;
                break;
            default:
                htmlTask = `<div class="task-container ${subclass}"><input class="task-input" id="${task.id}" value="${task.name}" onblur="ChangeName(this)"/><span class="status-label">${status}</span><button class="task-button" id="${task.id}" onclick="FinishTask(this)">Finish </button><button class="task-button" id="${task.id}" onclick="OpenDeleteDialog(this)"> Delete </button> <button class="task-button" onclick="OpenEditDialog(this)" id="${task.id}">Edit</button></div>`;
                break;
        }
        container.innerHTML += htmlTask;
    }
}
function CreateTask() {
    var nameInput = document.getElementById("nameInput");
    var descInput = document.getElementById("descInput");
    var duedateInput = document.getElementById("duedateInput");
    if (nameInput.value === "") {
        alert("\"Name\" is required!");
        return;
    }
    var request = new XMLHttpRequest();
    request.open("POST", "task");
    request.setRequestHeader('Content-Type', 'application/json'); // Set the appropriate content type
    request.onreadystatechange = function () {
        if (request.readyState === XMLHttpRequest.DONE) {
            if (request.status === 200) {
                switch (request.responseText) {
                    case "0":
                        Submit();
                        return;
                    case "1":
                        alert("More than two tasks cannot have the same due date");
                        return;
                    default:
                }
            }
        }
    };
    var json = JSON.stringify({
        "name": nameInput.value,
        "description": descInput.value === "" ? null : descInput.value,
        "dueDate": duedateInput.value === "" ? null : duedateInput.value
    });
    request.send(json);
}
function EditTask(placeholder) {
    var nameInput = document.getElementById("nameInput");
    var descInput = document.getElementById("descInput");
    var duedateInput = document.getElementById("duedateInput");
    if (nameInput.value === "") {
        alert("\"Name\" is required!");
        return;
    }
    var request = new XMLHttpRequest();
    request.open("POST", "edit-task");
    request.setRequestHeader('Content-Type', 'application/json'); // Set the appropriate content type
    request.onreadystatechange = function () {
        if (request.readyState === XMLHttpRequest.DONE) {
            if (request.status === 200) {
                switch (request.responseText) {
                    case "0":
                        Submit();
                        return;
                    case "1":
                        alert("More than two tasks cannot have the same due date");
                        return;
                    case "2":
                        alert("Task does not exist !");
                        return;
                    default:
                }
            }
        }
    };
    var json = JSON.stringify({
        "id": placeholder.id,
        "name": nameInput.value,
        "description": descInput.value === "" ? null : descInput.value,
        "dueDate": duedateInput.value === "" ? null : duedateInput.value
    });
    request.send(json);
}
function ChangeName(placeholder) {
    if (placeholder.value === "")
        return;
    var request = new XMLHttpRequest();
    request.open("POST", "change-name");
    request.setRequestHeader('Content-Type', 'application/json'); // Set the appropriate content type
    request.onreadystatechange = function () { };
    var json = JSON.stringify({
        "id": placeholder.id,
        "name": placeholder.value
    });
    request.send(json);
}
function DeleteTask(placeholder) {
    var request = new XMLHttpRequest();
    request.open("POST", "delete-task");
    request.setRequestHeader('Content-Type', 'application/json'); // Set the appropriate content type
    request.onreadystatechange = function () {
        if (request.readyState === XMLHttpRequest.DONE) {
            if (request.status === 200) {
                switch (request.responseText) {
                    case "0":
                        Submit();
                        return;
                    case "2":
                        alert("Task does not exist !");
                        return;
                    default:
                }
            }
        }
    };
    var json = JSON.stringify({
        "id": placeholder.id,
    });
    request.send(json);
}
function FinishTask(placeholder) {
    var request = new XMLHttpRequest();
    request.open("POST", "finish-task");
    request.setRequestHeader('Content-Type', 'application/json'); // Set the appropriate content type
    request.onreadystatechange = function () {
        if (request.readyState === XMLHttpRequest.DONE) {
            if (request.status === 200) {
                switch (request.responseText) {
                    case "0":
                        Submit();
                        return;
                    case "2":
                        alert("Task does not exist !");
                        return;
                    default:
                }
            }
        }
    };
    var json = JSON.stringify({
        "id": placeholder.id,
    });
    request.send(json);
}
function ReopenTask(placeholder) {
    var request = new XMLHttpRequest();
    request.open("POST", "reopen-task");
    request.setRequestHeader('Content-Type', 'application/json'); // Set the appropriate content type
    request.onreadystatechange = function () {
        if (request.readyState === XMLHttpRequest.DONE) {
            if (request.status === 200) {
                switch (request.responseText) {
                    case "0":
                        Submit();
                        return;
                    case "2":
                        alert("Task does not exist !");
                        return;
                    default:
                }
            }
        }
    };
    var json = JSON.stringify({
        "id": placeholder.id,
    });
    request.send(json);
}
function RestoreTask(placeholder) {
    var request = new XMLHttpRequest();
    request.open("POST", "restore-task");
    request.setRequestHeader('Content-Type', 'application/json'); // Set the appropriate content type
    request.onreadystatechange = function () {
        if (request.readyState === XMLHttpRequest.DONE) {
            if (request.status === 200) {
                switch (request.responseText) {
                    case "0":
                        Submit();
                        return;
                    case "2":
                        alert("Task does not exist !");
                        return;
                    default:
                }
            }
        }
    };
    var json = JSON.stringify({
        "id": placeholder.id,
    });
    request.send(json);
}
class Task {
    id;
    name;
    status;
    constructor(id, name, status) {
        this.id = id;
        this.name = name;
        this.status = status;
    }
}
