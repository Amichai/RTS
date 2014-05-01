var app = angular.module('root', [])
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

    $scope.updateName = function () {
        hub.server.updateName($scope.usernameInput).done(function (success) {
            if (success) {
                $scope.username = $scope.usernameInput;
                $scope.$apply();
            }
        });
    }

    hub.client.JoinTable = function (tableID) {
        console.log("join table: " + tableID);
        window.location.href = '/table/index?id=' + tableID + '&username=' + $scope.username ;
    }

    $scope.joinTable = function (table) {
        hub.server.joinTable(table.Users[0].ConnectionID, $scope.myConnectionID).done(function (result) {

        });
    }

    hub.client.SetUsername = function (name) {
        $scope.username = name;
    }

    $http.get(baseUrl + 'api/homeapi/getConnectedClients').success(function (clients) {
        console.log('got connected clients');
        $scope.clients = clients;
    });

    $http.get(baseUrl + 'api/homeapi/getWaitingTables').success(function (tables) {
        console.log('got waiting tables');
        $scope.tables = tables;
    });

    $scope.createTable = function () {
        hub.server.createTable().done(function () {
        });
    }
}]);

app.directive('ngEnter', function () {
    return function (scope, element, attrs) {
        element.bind("keydown keypress", function (event) {
            if (event.which === 13) {
                scope.$apply(function () {
                    scope.$eval(attrs.ngEnter);
                });
                event.preventDefault();
            }
        });
    };
});