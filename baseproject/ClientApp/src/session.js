import Swal from 'sweetalert2'

export default APP.session = function () {
    let t_pre_detonador = 20000 * 1000;
    let t_sal;
    let t_msj;
    // var t_tit;


    let init = function () {
        window.onload = resetTimer;
        window.onmousemove = resetTimer;
        window.onmousedown = resetTimer;
        window.onclick = resetTimer;
        window.onscroll = resetTimer;
        window.onkeypress = resetTimer;
    }

    let resetTimer = function () {
        clearTimeout(t_msj);
        clearTimeout(t_sal);
        clearTimeout(t_sal);
        t_msj = setTimeout(mostrar_mensaje, t_pre_detonador);
        //t_tit = clearInterval(t_tit);
    }

    let mostrar_mensaje = function () {
        let timerInterval
        let resultado = 0;
       
        Swal.fire({
            allowOutsideClick: false,
            allowEscapeKey: false,
            allowEnterKey: false,
            title: 'SU SESIÓN FINALIZARÁ EN',
            html: '<strong></strong> Segundos.<br/><br/>' +
                '<button id="resume" class="btn btn-danger">' +
                'Continuar Navegando' +
                '</button><br/><br/>' +
                '<a href="/Security/Logout" id="stop">Cerrar sesión</a>',
            timer: 20000,
            onBeforeOpen: () => {
                const content = Swal.getContent()
                const $ = content.querySelector.bind(content)
                const stop = $('#stop')
                const resume = $('#resume')

                Swal.showLoading()

                resume.addEventListener('click', () => {
                    resetTimer();
                    resultado = 1;
                    Swal.closeModal();
                })

                stop.addEventListener('click', () => { logout(); })

                timerInterval = setInterval(() => {
                    Swal.getContent().querySelector('strong')
                        .textContent = (Swal.getTimerLeft() / 1000)
                            .toFixed(0)
                }, 100)
            },
            onClose: () => {
                clearInterval(timerInterval)
            }
        }).then((result) => {
            if (result.dismiss === Swal.DismissReason.timer) {
                if (resultado == 0)
                    logout();
            }
        })
    }

    let logout = () => window.location.href = "/Security/Logout";

    return {
        resetTimer: resetTimer,
        init: init
    }
}();
