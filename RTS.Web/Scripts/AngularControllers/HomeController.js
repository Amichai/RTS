﻿angular.module('root', [])
.controller('homeController', ['$scope', '$http', function ($scope, $http) {

    var hub = $.connection.HomeHub;
    $.connection.hub.start().done(function () {
        hub.server.myConnectionID().done(function (result) {
            $scope.myConnectionID = result;
            $scope.$apply();
        });
    });

    hub.client.ConnectedClientsChanged = function (clients) {
        $scope.clients = clients;
        $scope.$apply();
    }

    hub.client.WaitingTablesChanged = function (tables) {
        $scope.tables = tables;
        $scope.$apply();
    }

    hub.client.JoinTable = function (tableID) {
        console.log("join table: " + tableID);
        window.location.href = '/table/index?id=' + tableID + '&username=' + $scope.username ;
    }

    hub.client.Test = function () {
        console.log('testing');
    }

    $scope.joinTable = function(id) {
        hub.server.joinTable(id, $scope.myConnectionID).done(function (result) {

        });
    }

    $http.get(baseUrl + 'api/homeapi/getConnectedClients').success(function (clients) {
        console.log('got connected clients');
        $scope.clients = clients;
    });

    $http.get(baseUrl + 'api/homeapi/getWaitingTables').success(function (tables) {
        console.log('got waiting tables');
        $scope.tables = tables;
    });

    $scope.buttonPress = function () {
        hub.server.buttonPress().done(function(result){
            console.log(result);
        });
    }

    $scope.createTable = function () {
        hub.server.createTable().done(function () {
        });
    }
}]);