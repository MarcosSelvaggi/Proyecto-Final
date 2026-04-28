var ultimoHashConvs = '';
var hub = null;

function renderConversacion(c) {
    var activa = c.idConversacion === convActual ? ' activa' : '';
    var badge = c.noLeidos > 0 ? "<span class='badge-noLeidos'>" + c.noLeidos + "</span>" : "";
    var ultimo = c.ultimoMensaje.length > 45 ? c.ultimoMensaje.substring(0, 45) + '…' : (c.ultimoMensaje || 'Sin mensajes');
    var avatarInner = c.otroFoto
        ? "<img src='" + c.otroFoto + "' class='avatar-foto' alt='Foto' />"
        : "<div class='avatar-iniciales'>" + (c.otroNombre[0] || '') + (c.otroApellido[0] || '') + "</div>";
    var avatarClase = c.noLeidos > 0 ? "avatar-conv has-badge" : "avatar-conv";
    return "<div class='conv-item" + activa + "' data-conv='" + c.idConversacion + "' onclick='seleccionarConv(" + c.idConversacion + ")'>" +
        "<div class='" + avatarClase + "'>" + avatarInner + "</div>" +
        "<div class='conv-info'>" +
        "<div class='conv-nombre'>" + c.otroNombre + " " + c.otroApellido + " " + badge + "</div>" +
        "<div class='conv-servicio'>" + c.servicio + "</div>" +
        "<div class='conv-ultimo'>" + ultimo + "</div>" +
        "</div>" +
        "<div class='conv-fecha'>" + c.fecha + "</div>" +
        "<button class='btn-eliminar-conv' onclick='eliminarConv(event," + c.idConversacion + ")' title='Eliminar'>🗑</button>" +
        "</div>";
}

function eliminarConv(event, idConv) {
    event.stopPropagation();
    if (!confirm('¿Eliminar esta conversación?')) return;
    var item = document.querySelector('.conv-item[data-conv="' + idConv + '"]');
    if (item) item.remove();
    if (convActual > 0 && idConv === convActual) {
        fetch('/MensajesHandler.ashx?accion=eliminar&conv=' + idConv)
            .then(function () { window.location.href = 'Mensajes.aspx'; });
        return;
    }
    fetch('/MensajesHandler.ashx?accion=eliminar&conv=' + idConv)
        .catch(function () { window.location.reload(); });
}

function pollConversaciones() {
    fetch('/MensajesHandler.ashx?accion=conversaciones')
        .then(function (r) { return r.json(); })
        .then(function (convs) {
            var hashNuevo = convs.map(function (c) {
                return c.idConversacion + '|' + c.noLeidos + '|' + c.ultimoMensaje;
            }).join(',');
            if (hashNuevo === ultimoHashConvs) return;
            ultimoHashConvs = hashNuevo;
            var lista = document.querySelector('.conv-lista');
            if (!lista) return;
            lista.innerHTML = convs.length > 0
                ? convs.map(renderConversacion).join('')
                : "<p class='sin-mensajes'>No tenés conversaciones aún.</p>";
        })
        .catch(function () { });
}

function seleccionarConv(id) {
    window.location.href = 'Mensajes.aspx?conv=' + id;
}

function volverLista() {
    document.getElementById('convPanel').classList.remove('oculto-mobile');
    document.getElementById('chatPanel').classList.remove('visible-mobile');
    history.pushState({}, '', 'Mensajes.aspx');
}

function scrollFondo() {
    setTimeout(function () {
        var body = document.getElementById('chatBody');
        if (body) body.scrollTop = body.scrollHeight;
    }, 50);
}

function renderMensaje(m) {
    var esMio = m.idEmisor === miIdUsuario;
    var visto = esMio
        ? "<span class='msg-visto " + (m.leido ? "leido" : "enviado") + "'>✓✓</span>"
        : "";
    var avatar = "";
    if (!esMio) {
        if (m.foto) {
            avatar = "<img src='" + m.foto + "' class='avatar-msg' alt='Foto' />";
        } else {
            var iniciales = (m.nombre[0] || '') + (m.apellido[0] || '');
            avatar = "<div class='avatar-msg avatar-iniciales-sm'>" + iniciales.toUpperCase() + "</div>";
        }
    }
    return "<div class='msg-wrapper " + (esMio ? "mio" : "suyo") + "'>" +
        avatar +
        "<div class='msg-burbuja'>" +
        "<span class='msg-texto'>" + m.texto + "</span>" +
        "<div class='msg-meta'>" +
        "<span class='msg-hora'>" + m.hora + "</span>" +
        visto +
        "</div></div></div>";
}

function enviarMensaje() {
    var txt = document.querySelector('.chat-input');
    if (!txt) return;
    var texto = txt.value.trim();
    if (!texto || !convActual) return;

    if (hub && $.connection.hub.state === $.signalR.connectionState.connected) {
        hub.server.enviarMensaje(convActual, miIdUsuario, texto, miNombre, miApellido)
            .done(function () {
                txt.value = '';
                var body = document.getElementById('chatBody');
                if (body) {
                    var mensajeLocal = {
                        idEmisor: miIdUsuario,
                        texto: texto,
                        hora: new Date().toTimeString().substring(0, 5),
                        leido: false,
                        nombre: miNombre,
                        apellido: miApellido,
                        foto: miFoto
                    };
                    body.innerHTML += renderMensaje(mensajeLocal);
                    scrollFondo();
                }
                ultimoHashConvs = '';
                pollConversaciones();
            })
            .fail(function (err) {
                // Reintentar enviar el mensaje cuando el hub se reconecte
                $(document).one('hubConectado', function () {
                    hub.server.enviarMensaje(convActual, miIdUsuario, texto, miNombre, miApellido, miFoto)
                        .done(function () { txt.value = ''; });
                });
            });
    } else {
        // Esperar reconexión
        $(document).one('hubConectado', function () {
            enviarMensaje();
        });
    }
}

// ── Arranco todo con jQuery ready (Con esto garantizamos que SignalR está cargado correctamente)
$(document).ready(function () {
    scrollFondo();
    setInterval(pollConversaciones, 6000);

    //Mostrar el chat en celulares
    if (window.innerWidth <= 640) {
        var params = new URLSearchParams(window.location.search);
        var conv = params.get('conv');

        if (conv) {
            document.getElementById('convPanel')?.classList.add('oculto-mobile');
            document.getElementById('chatPanel')?.classList.add('visible-mobile');
        }
    }
    var btnEnviar = document.getElementById('BtnEnviar');
    if (btnEnviar) btnEnviar.addEventListener('click', enviarMensaje);

    var chatInput = document.querySelector('.chat-input');
    if (chatInput) chatInput.addEventListener('keydown', function (e) {
        if (e.key === 'Enter' && !e.shiftKey) {
            e.preventDefault();
            enviarMensaje();
        }
    });

    if (typeof $.connection !== 'undefined' && $.connection.mensajesHub) {
        hub = $.connection.mensajesHub;

        onNuevoMensaje = function (m) {
            var body = document.getElementById('chatBody');
            if (!body) return;
            var lastMsg = body.lastElementChild;
            if (lastMsg && lastMsg.dataset.msgId === String(m.idEmisor + m.hora + m.texto)) return;
            var wrapper = document.createElement('div');
            wrapper.innerHTML = renderMensaje(m);
            var el = wrapper.firstElementChild;
            el.dataset.msgId = m.idEmisor + m.hora + m.texto;
            body.appendChild(el);
            scrollFondo();
            if (m.idEmisor !== miIdUsuario) {
                hub.server.marcarLeidos(convActual, miIdUsuario);
            }
            ultimoHashConvs = '';
            pollConversaciones();
        };

        onMensajesLeidos = function () {
            document.querySelectorAll('.msg-visto.enviado').forEach(function (el) {
                el.classList.remove('enviado');
                el.classList.add('leido');
            });
        };

        // Unirse al grupo — solo una vez cuando el hub esté listo
        $(document).on('hubConectado', function () {
            if (convActual && hub) {
                hub.server.unirseAConversacion(convActual)
                    .done(function () { })
                    .fail(function () { });
            }
        });

        // Si ya está conectado, disparar el evento manualmente
        if ($.connection.hub.state === $.signalR.connectionState.connected) {
            $(document).trigger('hubConectado');
        }
    }
});