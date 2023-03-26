"use strict";

var connection = new signalR.HubConnectionBuilder()
    .withUrl("/signalRServer")
    .build();
    
connection.on("LoadSetting", function () {
    location.href = '/Settings/SettingList'
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

connection.on("LoadCourse", function () {
    location.href = '/Courses/CourseList'
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});

connection.on("LoadUser", function () {
    location.href = '/Users/UserControl'
});

connection.start().catch(function (err) {
    return console.error(err.toString());
});