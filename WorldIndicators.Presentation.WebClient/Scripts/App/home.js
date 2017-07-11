window.app["home"] = (function (window, logger) {
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
            tbMainDataTable: $("#tb-main-data-table"),
            btnBuscar: $("#btn-buscar"),
            drpdnCountry: $("#CountryCode"),
            txtFechaInicio: $("#txt-fecha-inicio"),
            txtFechaFinal: $("#txt-fecha-final")
        };

        initInterface();
        initEventHandlers();
        logger.Info(getName() + " Module successfully started");
    }

    function initInterface() {
        pgElements.btnBuscar.prop("disabled", true);

        pgElements.txtFechaInicio.datepicker({
            format: " yyyy",
            viewMode: "years", 
            minViewMode: "years",
            startView: "years",
            clearBtn: true,
            language: "es",
            title: "Fecha Inicio"
        }).on("changeDate", function (ev) {
            var fechaFinal = pgElements.txtFechaFinal.datepicker("getDate");
            var fechaInicio = pgElements.txtFechaInicio.datepicker("getDate");
            if (fechaFinal != null && fechaInicio > fechaFinal) {
                pgElements.txtFechaInicio.datepicker("update", "");
            } else {
                pgElements.txtFechaInicio.datepicker("hide");
            }
        }).data("datepicker");;
        pgElements.txtFechaFinal.datepicker({
            format: " yyyy",
            viewMode: "years",
            minViewMode: "years",
            startView: "years",
            clearBtn: true,
            language: "es",
            title: "Fecha Final",
        }).on("changeDate", function (ev) {
            var fechaFinal = pgElements.txtFechaFinal.datepicker("getDate");
            var fechaInicio = pgElements.txtFechaInicio.datepicker("getDate");
            if (fechaInicio != null && fechaInicio > fechaFinal) {
                pgElements.txtFechaFinal.datepicker("update", "");
            } else {
                pgElements.txtFechaFinal.datepicker("hide");
            }
        }).data("datepicker");

        pgElements.tbMainDataTable.DataTable({
            "bFilter": false,
            "bLengthChange": false,
            "bSort": false,
            "bSearchable": false,
            "bServerSide": true,
            "sAjaxSource": routes["dataTableMain"],
            "dom": "rtip", // the "r" is for the "processing" message
            "language": {
                "processing": '<span class="spinner"><div class="bounce1"></div><div class="bounce2"></div><div class="bounce3"></div></span>'
            },
            "processing": true,
            "aoColumnDefs": [
                { "sClass": "left", "sWidth": "15%", "bSortable": false, "aTargets": [0], "mData": "CountryName" },
                { "sClass": "left", "sWidth": "5%", "bSortable": false, "aTargets": [1], "mData": "Year" },
                { "sClass": "left", "sWidth": "10%", "bSortable": false, "aTargets": [2], "mData": "BirthRateCrude" },
                { "sClass": "left", "sWidth": "10%", "bSortable": false, "aTargets": [3], "mData": "MortalityRateAdultMale" },
                { "sClass": "left", "sWidth": "10%", "bSortable": false, "aTargets": [4], "mData": "MortalityRateAdultFemale" },
                { "sClass": "left", "sWidth": "10%", "bSortable": false, "aTargets": [5], "mData": "MortalityRateInfant" },
                { "sClass": "left", "sWidth": "10%", "bSortable": false, "aTargets": [6], "mData": "MortalityRateUnder5" },
                { "sClass": "left", "sWidth": "10%", "bSortable": false, "aTargets": [7], "mData": "MortalityRate" }
            ],
            "fnServerData": function (sSource, aoData, fnCallback) {
                aoData.push(
                    { "name": "sSearch_a", "value": pgElements.drpdnCountry.val() },
                    { "name": "sSearch_b", "value": pgElements.txtFechaInicio.val() },
                    { "name": "sSearch_c", "value": pgElements.txtFechaFinal.val() }
                );
                $.getJSON(sSource, aoData, function (json) {
                    fnCallback(json);
                });
            }
        });

        //pgElements.drpdnCountry.change(function () {
        //    var $this = $(this);
        //    pgElements.btnBuscar.prop("disabled", $this.val() === "");
        //});
    }

    function initEventHandlers() {
        pgElements.btnBuscar.click(function () {
            pgElements.tbMainDataTable.dataTable().fnDraw(true);
        });
        pgElements.tbMainDataTable.on("processing.dt", function (e, settings, processing) {
            pgElements.btnBuscar.prop("disabled", processing);
        });
    }

    function getName() {
        var thisName = "";
        for (var name in window.app) {
            if (window.app.hasOwnProperty(name) && window.app[name] === app) {
                thisName = name;
                break;
            }
        }
        return thisName;
    }

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