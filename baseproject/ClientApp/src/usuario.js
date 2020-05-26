export default APP.usuario = function () {

    let init = () => {

        menuOptionClick();
        //llenarTabla();
        document.getElementById("frmUsuario").
            addEventListener("submit", function (e) {

                e.preventDefault();
                ValidarFormulario();
                if ($("#frmUsuario").valid()) {
                    if (!$("#Id").val())
                        agregarUsuario();
                    else
                        modificarUsuario();
                }
            })
    }
    /*=========================================================================
    LLENAR LA TABLA DE USUARIOS
    ==========================================================================*/
    let llenarTabla = () => {
        $('#tblUsuarios').DataTable({
            language: {
                url: '//cdn.datatables.net/plug-ins/1.10.15/i18n/Spanish.json'
            },
            "bDestroy": true,
            "scrollX": true,
            "processing": true,
            ajax: {
                url: $("#GetUsuarios").data('request-url'),
                dataSrc: '',
                "error": function (jqXHR, textStatus, errorThrown) {
                    $('#tblUsuarios').DataTable().clear().draw();
                }
            },
            dom: 'Bfrtip',
            "columnDefs": [
                {
                    "targets": [4],
                    "visible": false,
                    "searchable": false
                },
            ],
            columns: [
                {
                    data: "codigoUsuario",
                },
                {
                    data: "nombre",
                },
                {
                    data: "apellidos",
                },
                {
                    data: "estado",
                },
                {
                    data: "id",
                },
                {
                    data: null,
                    "bSortable": false,
                    "mRender": function (o) {
                        let botones = '<button  name ="editar" class="btn btn-info btn-sm"><i class="fa fa-edit fa-sm"></i></button>';
                        return botones;
                    }
                }
            ],
            buttons: [

                {
                    text: '<i class="fa fa-plus"></i>',
                    titleAttr: 'Nuevo Usuario',
                    action: function (e, dt, node, config) {
                        $("#frmUsuario")[0].reset();
                        $("#btnGuardar").show();
                        $("#usuarioModal").modal()
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
        $('#tblUsuarios tbody').on('click', 'button', function () {
            let table = $('#tblUsuarios').DataTable();
            let action = this.name;
            let data = table.row($(this).parents('tr')).data();
            if (action == 'editar') {
                $("#btnGuardar").show();
                llenarFormulario(data);
                $("#usuarioModal").modal();
            } else if (action == 'eliminar') {
                let id = data['id'];
                eliminarUsuario(id);
            }
        });
    }
    /*=========================================================================
	AGREGAR UN REGISTRO DE USUARIO A LA BASE DE DATOS
    ==========================================================================*/
    let agregarUsuario = () => {
        let form = $("#frmUsuario");
        $.ajax({
            type: "POST",
            dataType: "text",
            url: $("#AgregarUsuario").data('request-url'),
            data: form.serialize(),
            success: function (respuesta) {
                if (respuesta == 1) {
                    llenarTabla();
                    cancelar();
                    myAlerts.myAlerts('Registro almacenado satisfactoriamente', "Guardado", "success");
                }
                else if (respuesta == -2)
                    myAlerts.myAlerts('Ya existe ese usuario en el sistema', "Error", "error");
                else
                    myAlerts.myAlerts('El registro no pudo ser almacenado', "Error", "error");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    }
    /*=========================================================================
	MODIFICAR UN REGISTRO DE USUARIO EN LA BASE DE DATOS
    ==========================================================================*/
    let modificarUsuario = () => {
        let form = $("#frmUsuario");
        $.ajax({
            type: "POST",
            dataType: "text",
            url: $("#ModificarUsuario").data('request-url'),
            data: form.serialize(),
            success: function (respuesta) {
                if (respuesta == 1) {
                    llenarTabla();
                    cancelar();
                    myAlerts.myAlerts('Registro modificado satisfactoriamente', "Modificado", "success");
                }
                else
                    myAlerts.myAlerts('El registro no pudo ser modificado', "Error", "error");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    }
    /*=========================================================================
	ELIMINAR REGISTRO DE USUARIO
    ==========================================================================*/
    let eliminarUsuario = (id) => {
        let parametros = {
            'id': id
        }
        Swal.fire({
            title: 'Está seguro?',
            text: "El proceso no se podrá revertir",
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Sí, eliminar'
        }).then((result) => {
            if (result.value) {
                $.ajax({
                    type: "POST",
                    dataType: "text",
                    url: '/Usuarios/eliminarUsuario',
                    data: parametros,
                    success: function (respuesta) {
                        if (respuesta == 1) {
                            cancelar();
                            llenarTabla();
                            Swal.fire(
                                'Eliminado!',
                                'El registro fue eliminado correctamente.',
                                'success'
                            )
                        }
                        else {
                            Swal.fire(
                                'Error!',
                                'El registro NO fue eliminado.',
                                'error'
                            )
                        }
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        alert(errorThrown);
                    }
                })
            }
        });
    }
    /*=========================================================================
    VALIDAR FORMULARIO
    ==========================================================================*/
    let ValidarFormulario = () => {
        $('#frmUsuario').validate({
            errorElement: 'div',
            errorClass: 'help-block',
            focusInvalid: false,
            ignore: "",
            rules: {
                CodigoUsuario: {
                    required: true,
                    maxlength: 30
                },
                Nombre: {
                    required: true,
                    maxlength: 30
                },
                Apellidos: {
                    required: true,
                    maxlength: 60
                },
                PerfilId: {
                    required: true
                },
                Estado: {
                    required: true
                }
            },
            messages: {
                CodigoUsuario: {
                    required: "El campo codigo de usuario es obligatorio.",
                    maxlength: "El código de usuario no puede exceder los 30 caracteres"
                },
                Nombre: {
                    required: "El campo nombre es obligatorio.",
                    maxlength: "El nombre no puede exceder los 30 caracteres"
                },
                Apellidos: {
                    required: "El campo apellidos es obligatorio.",
                    maxlength: "El apellidos no puede exceder los 60 caracteres"
                },
                PerfilId: "El campo perfil es obligatorio.",
                Estado: "El campo estado es obligatorio."
            },
            highlight: function (e) {
                $(e).closest('.form-group').removeClass('has-info').addClass('has-error');
            },
            success: function (e) {
                $(e).closest('.form-group').removeClass('has-error');//.addClass('has-info');
                $(e).remove();
            },
            errorPlacement: function (error, element) {

                var name = element.attr('name');
                var errorSelector = '.validation_error_message[for="' + name + '"]';
                var $element = $(errorSelector);
                if ($element.length) {
                    $(errorSelector).html(error.html());
                } else {
                    error.insertAfter(element);
                }
            },
        });
    }
    /*=========================================================================
	MÉTODO LLENAR EL FORMULARIO AL HCER CLIC EN UNA FILA DE LA TABLA
    ==========================================================================*/
    let llenarFormulario = (json) => {
        $.each(json, function (key, value) {
            let string = key.charAt(0).toUpperCase() + key.slice(1);//primera letra en mayúscula
            $(`#${string}`).val(value);
        });
    };
    /*=========================================================================
     MÉTODO QUE SE ENCARGA DE LIMPIAR EL FORMULARIO Y CERRAR EL MODAL
    ==========================================================================*/
    let cancelar = () => {
        $("#frmUsuario")[0].reset();
        $('#usuarioModal').modal('hide');
    }
    /*=========================================================================
    ACTIVAR LINK DEL MENÚ AL HACER CLIC
    ==========================================================================*/
    let menuOptionClick = () => {
        $('li a').removeClass("active");
        $('li').removeClass("menu-open");
        $("#liSeguridad").addClass("menu-open");
        $("#linkSeguridad").addClass("active");
        $("#linkUsuarios").addClass("active");
    }
    return {
        init: init
    }
}();
