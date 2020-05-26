
import 'bootstrap/dist/css/bootstrap.css';
import 'bootstrap';
import myAlerts from './myAlerts';
import './security.css';
import $ from 'jquery';
import validate from 'jquery-validation';
window.jQuery = $;
window.$ = $;
window.jquery = $;

export default APP.security = function () {
    let init = () => {
        $("frmLogin").validate();
        document.getElementById("frmLogin").
            addEventListener("submit", function (e) {

                e.preventDefault();

                if ($("#frmLogin").valid())
                    Login();
            })
    }
    let Login = () => {
        let datos = {
            'usuario': document.getElementById('usuario').value,
            'clave': document.getElementById('clave').value
        }

        $.ajax({
            type: "POST",
            dataType: "text",
            url: $("#login").data('request-url'),
            data: datos,
            success: function (respuesta) {
                if (respuesta == 1) {
                    window.location.href = $("#dashboard").data('request-url');
                }
                else if (respuesta == -2)
                    myAlerts.myAlerts('Usuario o clave incorrectos', "Error", "error");
                else
                    myAlerts.myAlerts('El usuario no existe en el sistema', "Error", "error");
            },
            error: function (jqXHR, textStatus, errorThrown) {
                alert(errorThrown);
            }
        });
    }
    $('#frmLogin').validate({
        errorElement: 'div',
        errorClass: 'help-block',
        focusInvalid: false,
        ignore: "",
        rules: {
            usuario: {
                required: true
            },
            clave: {
                required: true
            }
        },
        messages: {
            usuario: "El campo usuario es obligatorio.",
            clave: "El campo clave es obligatorio."
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

    return {
        init: init
    }
}();
APP.security.init();