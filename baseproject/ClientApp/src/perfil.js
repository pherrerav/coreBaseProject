export default APP.perfiles = function () {

    /*=========================================================================
        INICIALIZAR LA FUNCIÓN
    ==========================================================================*/
    let init = () => {
        //llenarTabla();
        menuOptionClick();
        $('#tblPaginas').DataTable();

        document.getElementById("frmPerfiles").
            addEventListener("submit", function (e) {

                e.preventDefault();
                let idPerfil = $("#id").val();
                if (idPerfil == "")
                    agregarPerfil();
                else
                    modificarPerfil();
            })
    }
    /*=========================================================================
        AGREGAR UN REGISTRO DE PERFIL A LA BASE DE DATOS
    ==========================================================================*/
    let agregarPerfil = () => {
        let myCheckboxes = '';	//GUARDAR LOS CHECKBOX MARCADOS EN UN ARREGLO
        $('#perfilesForm input[type=checkbox]').each(function () {
            if (this.checked)
                myCheckboxes += $(this).attr('id') + ', ';
        });
        let arreglo = crearArreglo(document.getElementById("id").value, myCheckboxes)
        let parametros = {
            "PerfilNombre": document.getElementById("perfil").value,
            "Paginas_Perfiles": arreglo
        };
        $.ajax({
            type: "POST",
            dataType: "text",
            url: $("#AgregarPerfil").data('request-url'),
            data: parametros,
            success: function (respuesta) {
                if (respuesta == 1) {
                    cancelar();
                    llenarTabla();
                    myAlerts.myAlerts('Registro almacenado satisfactoriamente', "Guardado", "success");
                }
                else if (respuesta == -2) {
                    myAlerts.myAlerts('Ya existe ese perfil en el sistema', "Error", "error");
                }
                else {
                    myAlerts.myAlerts('El registro no pudo ser almacenado', "Error", "error");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    }
    /*=========================================================================
       CREAR ARREGLO DE PAGINAS Y PERFILES
    ==========================================================================*/
    let crearArreglo = (perfil, paginas) => {
        let paginasPerfiles = [];
        let paginasArr = paginas.split(',');
        let largo = paginasArr.length - 1;
        let obj = {};
        for (let i = 0; i < largo; i++) {
            obj = { 'perfilId': perfil, 'paginaId': paginasArr[i] }
            paginasPerfiles.push(obj);
        }
        return paginasPerfiles;
    }
    /*=========================================================================
        MODIFICAR UN REGISTRO DE PERFIL EN LA BASE DE DATOS
    ==========================================================================*/
    let modificarPerfil = () => {
        let myCheckboxes = '';	//GUARDAR LOS CHECKBOX MARCADOS EN UN ARREGLO
        $('#frmPerfiles input[type=checkbox]').each(function () {
            if (this.checked)
                myCheckboxes += $(this).attr('id') + ', ';
        });
        let arreglo = crearArreglo(document.getElementById("id").value, myCheckboxes)
        var parametros = {
            "Id": document.getElementById("id").value,
            "PerfilNombre": document.getElementById("perfil").value,
            "Paginas_Perfiles": arreglo
        };
        $.ajax({
            type: "POST",
            dataType: "text",
            url: $("#ModificarPerfil").data('request-url'),
            data: parametros,
            success: function (respuesta) {
                if (respuesta == 1) {
                    cancelar();
                    llenarTabla();
                    myAlerts.myAlerts('Registro modificado satisfactoriamente', "Modificado", "success");
                }
                else {
                    myAlerts.myAlerts('El registro no pudo ser modificado', "Error", "error");
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    }
    /*=========================================================================
        LLENAR LA TABLA DE PERFILES
    ==========================================================================*/
    let llenarTabla = () => {
        $('#tblPerfiles').DataTable({
            language: {
                url: '//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json'
            },
            "bDestroy": true,
            "scrollX": true,
            "processing": true,
            ajax: {
                url: $("#GetPerfiles").data('request-url'),
                dataSrc: '',
                "error": function (jqXHR, textStatus, errorThrown) {
                    $("#tblPerfiles").DataTable().clear().draw();
                }
            },
            dom: 'Bfrtip',
            columns: [
                { data: "id" },
                { data: "perfilNombre" },
                {
                    data: null,
                    "bSortable": false,
                    "mRender": function (o) {
                        let botones = '<button  name ="editar" class="btn btn-info btn-sm"><i class="fa fa-edit fa-sm"></i></button> ';
                        return botones;
                    }
                }
            ],
            buttons: [
                {
                    text: '<i class="fa fa-plus"></i>',
                    titleAttr: 'Nuevo Perfil',
                    action: function (e, dt, node, config) {
                        $("#frmPerfiles")[0].reset();
                        $("#btnGuardar").show();
                        $("#perfilModal").modal()
                    }
                },
                {
                    text: '<i class="fa fa-refresh"></i>',
                    titleAttr: 'Actualizar',
                    action: function (e, dt, node, config) {
                        llenarTabla();
                    }
                }
            ]
        });
        $('#tblPerfiles tbody').on('click', 'button', function () {

            let table = $('#tblPerfiles').DataTable();
            let action = this.name;
            let data = table.row($(this).parents('tr')).data();
            if (action == 'editar') {
                $("#btnGuardar").show();
                llenarFormulario(data);
                $("#perfilModal").modal();
            }
        });
    }
    /*=========================================================================
        MÉTODO LLENAR EL FORMULARIO
    ==========================================================================*/
    let llenarFormulario = (json) => {
        $("#frmPerfiles")[0].reset();
        let IdPerfil = json.id;
        let perfilNombre = json.perfilNombre;
        document.getElementById("id").value = IdPerfil;
        document.getElementById("perfil").value = perfilNombre;
        $.ajax({
            type: "POST",
            dataType: "json",
            url: $("#GetPerfilPaginas").data('request-url'),
            data: {
                'Id': IdPerfil
            },
            success: function (respuesta) {
                respuesta = JSON.stringify(respuesta);
                let paginas = $.parseJSON(respuesta);
                $(function () {
                    paginas = eval(paginas);
                    for (let i in paginas) {
                        document.getElementById(paginas[i]['paginaId']).checked = true;
                    }
                });
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    };
    /*=========================================================================
        MÉTODO QUE SE ENCARGA DE LIMPIAR EL FORMULARIO Y CERRAR EL MODAL
    ==========================================================================*/
    let cancelar = () => {
        $("#frmPerfiles")[0].reset();
        $('#perfilModal').modal('hide');
    }
    /*=========================================================================
    ACTIVAR LINK DEL MENÚ AL HACER CLIC
    ==========================================================================*/
    let menuOptionClick = () => {
        $('li a').removeClass("active");
        $('li').removeClass("menu-open");
        $("#liSeguridad").addClass("menu-open");
        $("#linkSeguridad").addClass("active");
        $("#linkPerfiles").addClass("active");
    }
    return {
        init: init
    }
}();
