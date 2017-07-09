window.$app["home"] = (function (window, logger) {
    "use strict";

    var app;
    var model;
    var pgElements;
    var routes = {
        "mainDataTable": ""
    };

    //------------Start Init Functions------------
    function init(externalModel) {
        app = this;
        model = externalModel || {};
        pgElements = {
            tbMainDataTable: $("#tb-main-data-table")
        };

        initInterface();
        initEventHandlers();
        //logger.Info(getName() + " Module successfully started");
    }

    function initInterface() {
        
    }

    function initEventHandlers() {
        
    }

    //function getName() {
    //    var thisName = "";
    //    for (var name in window) {
    //        if (window.hasOwnProperty(name) && window[name] === app) {
    //            thisName = name;
    //            break;
    //        }
    //    }
    //    return thisName;
    //}

    function addRoute(key, value) {
        routes[key] = value;
        return routes;
    }

    //Return the API
    return {
        Init: init,
        AddRoute: addRoute
    };
})(window, window.logger);