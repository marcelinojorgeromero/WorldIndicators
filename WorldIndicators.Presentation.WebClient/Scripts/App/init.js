
(function ($) {
    "use strict";

    window["app"] = {};
    window.app["ajax"] = (function () {

        function get(sUrl, oParams) {
            var jqXhr = $.ajax({
                type: "GET",
                cache: false,
                url: urlBuilder(sUrl, oParams),
                async: true
            });
            return jqXhr;
        }

        function post(sUrl, data) {
            var jqXhr = $.ajax({
                type: "POST",
                cache: false,
                url: sUrl,
                data: data,
                async: true
            });
            return jqXhr;
        }

        function del(sUrl, data, contentType) {
            var jqXhr = $.ajax({
                type: "DELETE",
                cache: false,
                url: sUrl,
                data: data,
                contentType: contentType,
                async: true
            });
            return jqXhr;
        }

        function put(sUrl, data, contentType) {
            var jqXhr = $.ajax({
                type: "PUT",
                cache: false,
                url: sUrl,
                data: data,
                contentType: contentType,
                async: true
            });
            return jqXhr;
        }

        function urlBuilder(sUrl, oParams) {
            var finalUrl = (oParams != null) ? sUrl + "?" + $.param(oParams) : sUrl;
            return finalUrl;
        }

        return {
            Get: get,
            Post: post,
            Delete: del,
            Put: put,
            UrlBuilder: urlBuilder
        };
    })();

    function Log(isEnabled) {
        this.isEnabled = isEnabled;
    };
    
    Log.prototype = function () {
        function startGroup(message) {
            console.group(message);
        }
        function endGroup() {
            console.groupEnd();
        }
        function info(message) {
            if (!this.isEnabled) return;
            if (arguments.length == 1) {
                console.log("[Info]: " + (typeof message == typeof {} ? "%o" : "%s"), message);
                return;
            }
            var msg = messageBuilder("[Info]: ", arguments);
            //var args = Array.prototype.slice.call(arguments);
            //args.splice(0, 0, msg);
            console.log(msg);
        }
        function error(message) {
            if (!this.isEnabled) return;
            if (arguments.length == 1) {
                console.log("[Error]: " + (typeof message == typeof {} ? "%o" : "%s"), message);
                return;
            }
            var msg = messageBuilder("[Error]: ", arguments);
            console.log(msg);
        }
        function warning(message) {
            if (!this.isEnabled) return;
            if (arguments.length == 1) {
                console.log("[Warning]: " + (typeof message == typeof {} ? "%o" : "%s"), message);
                return;
            }
            var msg = messageBuilder("[Warning]: ", arguments);
            console.log(msg);
        }
        function messageBuilder(sPrefix, arrArgs) {
            var msgBuilder = sPrefix;
            for (var i = 0; i < arrArgs.length; i++) {
                msgBuilder += (typeof arrArgs[i] == typeof {} ? JSON.stringify(arrArgs[i]) : arrArgs[i]);
            }
            return msgBuilder;
        }

        function enableLogger(bEnable) {
            this.isEnabled = bEnable;
        }

        return {
            Info: info,
            Error: error,
            Warning: warning,
            StartGroup: startGroup,
            EndGroup: endGroup,
            EnableLogger: enableLogger
        };
    }();

    ///<summary>
    /// Set globals
    ///</summary>
    window.logger = new Log(false);

    function initAjax() {
        var token = $("input[name=\"__RequestVerificationToken\"]").val();
        var headers = {};
        headers["__RequestVerificationToken"] = token;
        logger.StartGroup("Ajax headers are setted to:");
        logger.Info(headers);
        logger.EndGroup();
        $.ajaxSetup({
            headers: headers,
            data: {
                "__RequestVerificationToken": token
            }
        });
    }

    initAjax();

    /// Agrega al jQuery metodo para agregar elementos a un combo box
    /// Ejemplo:
    ///pgElements.cboEmpresas.addOptionItems([{ Selected: true, Text: "Seleccione...", Value: "" }]);
    ///pgElements.cboEmpresas.addOptionItems("prueba");
    $.fn.addOptionItems = function (data) {
        return this.each(function () {
            var list = this;
            $.each(data, function (index, itemData) {
                //var option = new Option(index + '. ' + itemData.Text, itemData.Value);
                var option = new Option(itemData.Text, itemData.Value);
                list.add(option);
            });
        });
    };

    $.fn.removeAttributes = function () {
        return this.each(function () {
            var attributes = $.map(this.attributes, function (item) {
                return item.name;
            });
            var element = $(this);
            $.each(attributes, function (i, item) {
                element.removeAttr(item);
            });
        });
    };

    /* Set the defaults for DataTables initialisation */
    $.extend(true, $.fn.DataTable.defaults, {
        "sDom": "<'row'<'col-sm-12'<'pull-right'f><'pull-left'l>r<'clearfix'>>>t<'row'<'col-sm-12'<'pull-left'i><'pull-right'p><'clearfix'>>>",
        "oLanguage": {
            "sLengthMenu": 'Mostrar <select>' +
                                    '<option value="10">10</option>' +
                                    '<option value="15">15</option>' +
                                    '<option value="20">20</option>' +
                                    '</select> Registros',
            "sZeroRecords": "No se encontraron registros",
            "sInfo": "Registros del _START_ al _END_ de un total de _TOTAL_",
            "sInfoPostFix": "",
            "sEmptyTable": "No hay registros para mostrar",
            "sInfoFiltered": "(Filtrado a partir de un total de _MAX_ registros)",
            "oPaginate": {
                "sFirst": '<button type="button" class="btn btn-default btn-xs"><strong><|</strong></button> ',
                "sLast": ' <button type="button" class="btn btn-default btn-xs"><strong>|></strong></button> ',
                "sNext": ' <button type="button" class="btn btn-default btn-xs"><strong>></strong></button> ',
                "sPrevious": ' <button type="button" class="btn btn-default btn-xs"><strong><</strong></button> '
            },
            "sSearch": "",
            "sProcessing": "Procesando..",
            "sLoadingRecords": "Cargando...",
            "sInfoEmpty": "Mostrando de 0 a 0 de un total de 0 registros"
        }
    });
})(jQuery);


