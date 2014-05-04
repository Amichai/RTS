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

    $scope.addMessage = function () {
        hub.server.newMessage(QueryString.username, QueryString.id, $scope.inputText).done(function (result) {
            $scope.state =  $sce.trustAsHtml(result.ToHtml);
            $scope.inputText = '';
            $scope.$apply();
        });
    }

    var margin = { top: 20, right: 20, bottom: 30, left: 40 },
        width = 960 - margin.left - margin.right,
        height = 500 - margin.top - margin.bottom;

    var formatPercent = d3.format(".0%");

    var x = d3.scale.ordinal()
        .rangeRoundBands([0, width], .1, 1);

    var y = d3.scale.linear()
        .range([height, 0]);
    function setState(state) {

        var xAxis = d3.svg.axis()
            .scale(x)
            .orient("bottom");

        var yAxis = d3.svg.axis()
            .scale(y)
            .orient("left")
            .tickFormat(formatPercent);
        
        $('#boardState').html('')
        var svg = d3.select("#boardState").insert("svg")
            .attr("width", width + margin.left + margin.right)
            .attr("height", height + margin.top + margin.bottom)
          .insert("g")
            .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

        
        var data = state.State;

            data.forEach(function (d) {
                d.frequency = +d.frequency;
            });

            x.domain(data.map(function (d) { return d; }));
            y.domain([0, d3.max(data, function (d) { return d; })]);

            svg.insert("g")
                .attr("class", "x axis")
                .attr("transform", "translate(0," + height + ")")
                .call(xAxis);

            svg.append("g")
                .attr("class", "y axis")
                .call(yAxis)
              .append("text")
                .attr("transform", "rotate(-90)")
                .attr("y", 6)
                .attr("dy", ".71em")
                .style("text-anchor", "end")
                .text("Frequency");

            svg.selectAll(".bar")
                .data(data)
              .enter().append("rect")
                .attr("class", "bar")
                .attr("x", function (d) { return x(d); })
                .attr("width", x.rangeBand())
                .attr("y", function (d) { return y(d); })
                .attr("height", function (d) { return height - y(d); });

        

        $scope.$apply();
    }

    function type(d) {
        d.frequency = +d.frequency;
        return d;
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
    });

    $("html").keypress(function (e) {
        hub.server.newMessage(QueryString.username, QueryString.id, e.keyCode).done(function (result) {
            setState(result);
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