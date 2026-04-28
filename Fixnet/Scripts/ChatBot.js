let estadoChat = {
    servicio: null,
    provincia: null,
    departamento: null,
    localidad: null,
    idLocalidad: null
};

let chatMinimizado = false;

document.addEventListener("DOMContentLoaded", function () {
    const estadoGuardado = localStorage.getItem("chatMinimizado");

    if (estadoGuardado === "true") {
        toggleChat();
    }
    if (window.innerWidth <= 576) {
        chatMinimizado = true;
        document.getElementById("chatbot").classList.add("minimizado");
    }
    agregarMensaje("bot", "Hola, soy Fixi 👋 ¿Qué servicio necesitás?");
    mostrarServicios();
});

function toggleChat(e) {
    if (e) e.stopPropagation();

    const chat = document.getElementById("chatbot");

    chatMinimizado = !chatMinimizado;

    localStorage.setItem("chatMinimizado", chatMinimizado);

    if (chatMinimizado) {
        chat.classList.add("minimizado");
        document.getElementById("btn-minimizar").innerText = "+";
    } else {
        chat.classList.remove("minimizado");
        document.getElementById("btn-minimizar").innerText = "−";
    }
}

function buscarPrestadores() {
    if (!estadoChat.idLocalidad || !estadoChat.servicio) {
        agregarMensaje("bot", "Falta información para buscar 😕");
        return;
    }

    let datos = {
        servicio: estadoChat.servicio,
        localidad: estadoChat.idLocalidad
    };

    fetch("Default.aspx/ContarPrestadores", {
        method: "POST",
        headers: {
            "Content-Type": "application/json; charset=utf-8"
        },
        body: JSON.stringify({
            datos: {
                servicio: estadoChat.servicio,
                localidad: estadoChat.idLocalidad
            }
        })
    })
        .then(res => res.json())
        .then(data => {
            let cantidad = data.d;

            if (cantidad === 0) {
                agregarMensaje("bot", "No encontré prestadores 😕");
            } else if (cantidad === 1) {
                agregarMensaje(
                    "bot",
                    `Encontré ${cantidad} prestador en tu zona 👷‍♂️ Registrate para contactarlo!.<br><br>
                    <a href="/Registro.aspx" style="
                        display:inline-block;
                        background:#25d366;
                        color:white;
                        padding:8px 12px;
                        border-radius:8px;
                        text-decoration:none;
                    ">
                        Registrarme
                    </a>`
                );
            }

            else {
                agregarMensaje(
                    "bot",
                    `Encontré ${cantidad} prestadores en tu zona 👷‍♂️ Registrate para contactarlos!.<br><br>
                    <a href="/Registro.aspx" style="
                        display:inline-block;
                        background:#25d366;
                        color:white;
                        padding:8px 12px;
                        border-radius:8px;
                        text-decoration:none;
                    ">
                        Registrarme
                    </a>`
                );
            }

            mostrarBotonReiniciar();
        });
}

function seleccionarServicio(servicio) {
    estadoChat.servicio = servicio;

    agregarMensaje("usuario", servicio);
    agregarMensaje("bot", "Perfecto 👍 Ahora elegí tu provincia: ");
    mostrarProvincias();
}

async function mostrarProvincias() {
    let contenedor = document.getElementById("chat-opciones");
    contenedor.innerHTML = "Cargando provincias... ⏳";

    let res = await fetch("https://apis.datos.gob.ar/georef/api/provincias");
    let data = await res.json();

    let provincias = data.provincias.sort((a, b) =>
        a.nombre.localeCompare(b.nombre)
    );

    let select = document.createElement("select");
    select.className = "form-select";

    select.innerHTML = `<option selected disabled>Seleccioná provincia</option>`;

    provincias.forEach(p => {
        select.innerHTML += `<option value="${p.nombre}">${p.nombre}</option>`;
    });

    select.onchange = () => seleccionarProvincia(select.value);

    contenedor.innerHTML = "";
    contenedor.appendChild(select);
}

async function seleccionarProvincia(provincia) {
    estadoChat.provincia = provincia;

    agregarMensaje("usuario", provincia);
    agregarMensaje("bot", "Bien 👍 Ahora elegí tu departamento");

    let contenedor = document.getElementById("chat-opciones");
    contenedor.innerHTML = "Cargando departamentos... ⏳";

    let res = await fetch(
        `https://apis.datos.gob.ar/georef/api/departamentos?provincia=${provincia}&max=500`
    );
    let data = await res.json();

    let departamentos = data.departamentos.sort((a, b) =>
        a.nombre.localeCompare(b.nombre)
    );

    let select = document.createElement("select");
    select.className = "form-select";

    select.innerHTML = `<option selected disabled>Seleccioná departamento</option>`;

    departamentos.forEach(d => {
        select.innerHTML += `<option value="${d.nombre}">${d.nombre}</option>`;
    });

    select.onchange = () => seleccionarDepartamento(select.value);

    contenedor.innerHTML = "";
    contenedor.appendChild(select);
}

async function seleccionarDepartamento(departamento) {
    estadoChat.departamento = departamento;

    agregarMensaje("usuario", departamento);
    agregarMensaje("bot", "Perfecto 👌 Ahora elegí tu localidad");

    let contenedor = document.getElementById("chat-opciones");
    contenedor.innerHTML = "Cargando localidades... ⏳";

    let res = await fetch(
        `https://apis.datos.gob.ar/georef/api/localidades?provincia=${estadoChat.provincia}&departamento=${departamento}&max=200`
    );
    let data = await res.json();

    let localidades = data.localidades;

    // Quito los duplicados
    let unicas = [...new Map(localidades.map(l => [l.nombre, l])).values()];

    let select = document.createElement("select");
    select.className = "form-select";

    select.innerHTML = `<option selected disabled>Seleccioná localidad</option>`;

    unicas.forEach(l => {
        select.innerHTML += `<option value="${l.id}|${l.nombre}">${l.nombre}</option>`;
    });

    select.onchange = () => {
        let [id, nombre] = select.value.split("|");
        seleccionarLocalidad(id, nombre);
    };

    contenedor.innerHTML = "";
    contenedor.appendChild(select);
}

function seleccionarLocalidad(id, nombre) {
    estadoChat.localidad = nombre;
    estadoChat.idLocalidad = id;


    agregarMensaje("usuario", nombre);
    agregarMensaje("bot", "Buscando prestadores... 🔍");

    buscarPrestadores();
}

function agregarMensaje(tipo, texto) {
    let chat = document.getElementById("chat-body");

    let div = document.createElement("div");
    div.className = tipo === "usuario" ? "msg-user" : "msg-bot";

    div.innerHTML = texto;
    chat.appendChild(div);

    chat.scrollTop = chat.scrollHeight;
}

function mostrarBotonReiniciar() {
    let contenedor = document.getElementById("chat-opciones");
    contenedor.innerHTML = "";

    let btn = document.createElement("button");
    btn.innerText = "Buscar otro servicio";
    btn.onclick = reiniciarChat;

    contenedor.appendChild(btn);
}

function reiniciarChat() {
    estadoChat.servicio = null;
    estadoChat.provincia = null;

    agregarMensaje("bot", "¿Qué servicio necesitás?");
    mostrarServicios();
}

async function mostrarServicios() {
    let contenedor = document.getElementById("chat-opciones");
    contenedor.innerHTML = "Cargando servicios... ⏳";

    try {
        let res = await fetch("Default.aspx/TraerServicios", {
            method: "POST",
            headers: { "Content-Type": "application/json; charset=utf-8" },
            body: JSON.stringify({})
        });
        let data = await res.json();
        let servicios = data.d;

        let select = document.createElement("select");
        select.className = "form-select";
        select.innerHTML = `<option selected disabled>Seleccioná un servicio</option>`;

        servicios.forEach(s => {
            select.innerHTML += `<option value="${s.nombre}">${s.nombre}</option>`;
        });

        select.onchange = () => seleccionarServicio(select.value);

        contenedor.innerHTML = "";
        contenedor.appendChild(select);
    } catch {
        contenedor.innerHTML = "Error al cargar servicios 😕";
    }
}