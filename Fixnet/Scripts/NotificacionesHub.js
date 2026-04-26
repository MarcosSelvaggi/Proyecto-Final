function mostrarToast(nombre, apellido, texto, foto, idConversacion) {
    var existing = document.querySelector('.toast-mensaje');
    if (existing) existing.remove();

    var iniciales = ((nombre || '')[0] || '') + ((apellido || '')[0] || '');
    var avatarHtml = foto
        ? "<img src='" + foto + "' style='width:38px;height:38px;border-radius:50%;object-fit:cover;' />"
        : "<div class='toast-avatar'>" + iniciales.toUpperCase() + "</div>";

    var textoCorto = (texto || '').length > 50 ? texto.substring(0, 50) + '…' : (texto || '');

    var toast = document.createElement('div');
    toast.className = 'toast-mensaje';
    toast.innerHTML = avatarHtml +
        "<div class='toast-info'>" +
        "<strong>" + (nombre || '') + " " + (apellido || '') + "</strong>" +
        "<span>" + textoCorto + "</span>" +
        "</div>";

    toast.onclick = function () {
        toast.remove();
        if (idConversacion) {
            window.location.href = '/Mensajes.aspx?conv=' + idConversacion;
        }
    };

    document.body.appendChild(toast);

    setTimeout(function () {
        if (toast.parentNode) toast.remove();
    }, 5000);
}

function actualizarBadgeNoLeidos(num) {
    var badge = document.querySelector('.badge-noLeidos');
    if (num > 0) {
        if (!badge) {
            var link = document.querySelector('a[href*="Mensajes.aspx"]');
            if (link) {
                var s = document.createElement('span');
                s.className = 'badge-noLeidos';
                s.textContent = num;
                link.appendChild(s);
            }
        } else {
            badge.textContent = num;
        }
    } else if (badge) {
        badge.remove();
    }
}