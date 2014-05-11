var app = angular.module('root', [])
.controller('tableController', ['$scope', '$http', '$sce', function ($scope, $http, $sce) {


    var hub = $.connection.TableHub;
    $.connection.hub.start().done(function () {
        hub.server.myConnectionID(QueryString.username).done(function (result) {
            $scope.myConnectionID = result;
            $scope.$apply();
        });
        hub.server.getCurrentState(QueryString.id).done(function (result) {
            setState(result);
        });
    });

    var updated = true;

    function p1(data) {
        for (var i = 0; i < data.length; i++) {
            var col = data[i];
            for (var j = 0; j < col.length; j++) {
                var val = col[j];
                $('#boardState').append('<div class="border"><div class="cell">' + val + '</div></div>');
            }
            $('#boardState').append('<br />');
        }
    }

    function p2(data) {
        for (var i = data.length - 1; i >= 0; i--) {
            var col = data[i];
            for (var j = 0; j < col.length; j++) {
                var val = col[j];
                $('#boardState').append('<div class="border"><div class="cell">' + val + '</div></div>');
            }
            $('#boardState').append('<br />');
        }
    }

    function setState(state) {
        $('#boardState').html('')
        var data = state.State;
        if ($scope.firstPlayer) {
            p1(data);
        } else {
            p2(data);
        }


        $scope.$apply();
    }

    hub.client.State = function (result) {
        setState(result);
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
        if (table.Users[0].Name == $scope.username) {
            $scope.firstPlayer = true;
        } else {
            $scope.firstPlayer = false;
        }
    });

    $("html").keypress(function (e) {
        if (!updated) {
            return;
        }
        updated = false;
        hub.server.newMessage(QueryString.username, QueryString.id, e.keyCode).done(function (result) {
            setState(result);
            updated = true;
        });
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