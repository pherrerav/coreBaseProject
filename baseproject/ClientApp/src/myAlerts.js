//============================================================
//MENSAJES PARA MOSTRAR AL USUSARIO DEPENDIENTE DE LA ACCIÓN
//QUE ESTÉ EJECUTANDO
//============================================================
export default APP.myAlerts = function () {
    let Mensajes = (miMensaje, miTitulo, tipoMensaje) => {
        Swal.fire({
            type: tipoMensaje,
            title: miTitulo,
            text: miMensaje
        })
    }
    return {
        myAlerts: Mensajes
    }
}();