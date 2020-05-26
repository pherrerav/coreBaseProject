export default APP.logs = function () {

    let init = () => {
        menuOptionClick();
        llenarTablaLogs();
    }
    /*=========================================================================
   LLENAR LA TABLA DE LOGS
   ==========================================================================*/
    let llenarTablaLogs = () => {
        $('#tblLogs').DataTable({
            language: {
                url: '//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json'
            },
            "bDestroy": true,
            "order": [[0, "desc"]],
            "scrollX": true,
            "processing": true,
            ajax: {
                url: $("#GetLogsFiles").data('request-url'),
                dataSrc: '',
                "error": function (jqXHR, textStatus, errorThrown) {
                    $('#tblLogs').DataTable().clear().draw();
                }
            },
            dom: 'Blfrtip',
            columns: [
                {
                    data: "nombre"
                },
                {
                    data: null,
                    "bSortable": false,
                    "mRender": function (o) {
                        let botones = '<button  name ="ver" class="btn btn-info btn-sm"><i class="fa fa-eye fa-sm"></i></button>';
                        return botones;
                    }
                }
            ],
            buttons: []
        });
        $('#tblLogs tbody').on('click', 'button', function () {
            let table = $('#tblLogs').DataTable();
            let data = table.row($(this).parents('tr')).data();
            $("#modalLog").modal();
            $("#logFileTitle").html(data['nombre']);
            document.getElementById("logsIframe").src = $("#logsFile").data('request-url') + data['nombre'];

        });
    }
    /*=========================================================================
    ACTIVAR LINK DEL MENÚ AL HACER CLIC
    ==========================================================================*/
    let menuOptionClick = () => {
        $('li a').removeClass("active");
        $('li').removeClass("menu-open");
        $("#liSeguridad").addClass("menu-open");
        $("#linkSeguridad").addClass("active");
        $("#linkLogs").addClass("active");
    }
    return {
        init : init
    }
}();