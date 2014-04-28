var app = angular.module('root', [])
.controller('tableController', ['$scope', '$http', function ($scope, $http) {
    $scope.messages = new Array();


    var hub = $.connection.TableHub;
    $.connection.hub.start().done(function () {
        hub.server.myConnectionID(QueryString.username).done(function (result) {
            $scope.myConnectionID = result;
            $scope.$apply();
        });
    });    

    $scope.addMessage = function () {
        hub.server.newMessage(QueryString.username, QueryString.id, $scope.inputText).done(function (result) {
            $scope.messages = result;
            $scope.inputText = '';
            $scope.$apply();
        });
    }

    hub.client.State = function (state) {
        $scope.messages = state;
        $scope.$apply();
    }

    var QueryString = function () {
        // This function is anonymous, is executed immediately and 
        // the return value is assigned to QueryString!
        var query_string = {};
        var query = window.location.search.substring(1);
        var vars = query.split("&");
        for (var i = 0; i < vars.length; i++) {
            var pair = vars[i].split("=");
            // If first entry with this name
            if (typeof query_string[pair[0]] === "undefined") {
                query_string[pair[0]] = pair[1];
                // If second entry with this name
            } else if (typeof query_string[pair[0]] === "string") {
                var arr = [query_string[pair[0]], pair[1]];
                query_string[pair[0]] = arr;
                // If third or later entry with this name
            } else {
                query_string[pair[0]].push(pair[1]);
            }
        }
        return query_string;
    }();

    $scope.username = QueryString.username;
    $scope.tableID = QueryString.id;

    $http.get(baseUrl + 'api/tableapi/gettable?id=' + QueryString.id).success(function (table) {
        $scope.table = table;
    });
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