<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Gestion.aspx.cs" Inherits="Fixnet.Gestion" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/Gestion.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:HiddenField ID="hfTurnosJson"    runat="server" />
    <asp:HiddenField ID="hfDisponibilidad" runat="server" />
    <asp:HiddenField ID="hfMesAnio"       runat="server" />

    <div class="container py-4" style="max-width:960px;">

        <div class="page-header">
            <span style="font-size:2rem;">🗓</span>
            <div>
                <h4>Gestión</h4>
                <p>Tus turnos programados. Clickeá un día para ver el detalle.</p>
            </div>
        </div>

        <div style="background:rgba(255,255,255,0.03); border:1px solid rgba(255,255,255,0.08); border-radius:16px; padding:20px;">
            <div class="cal-nav">
                <button class="btn-nav" onclick="cambiarMes(-1)">&#8592;</button>
                <h5 id="calTitulo"></h5>
                <button class="btn-nav" onclick="cambiarMes(1)">&#8594;</button>
            </div>
            <div class="cal-grid mb-1">
                <div class="cal-header-dia">Dom</div>
                <div class="cal-header-dia">Lun</div>
                <div class="cal-header-dia">Mar</div>
                <div class="cal-header-dia">Mié</div>
                <div class="cal-header-dia">Jue</div>
                <div class="cal-header-dia">Vie</div>
                <div class="cal-header-dia">Sáb</div>
            </div>
            <div class="cal-grid" id="calGrid"></div>
        </div>

    </div>

    <!-- Modal detalle día -->
    <div class="modal fade" id="modalDia" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content glass-modal">
                <div class="modal-header modal-header-glass">
                    <h5 class="modal-title modal-title-glass" id="modalDiaTitulo">Turnos del día</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body p-4" id="modalDiaBody"></div>
            </div>
        </div>
    </div>

   <script>
       //IMPORTANTE!!! NO LOS MUEVAN DE ACA DEBIDO A QUE SI PONEN EL CODIGO EN UN ARCHIVO APARTE ES DIFICIL EL MANEJO DE VARIABLES 
       // 1. Inicialización de variables con datos del Servidor
       // Esto evita que al recargar la página el calendario "vuelva" al mes actual si estabas en Mayo
       var hfMesAnioVal = document.getElementById('<%= hfMesAnio.ClientID %>').value;
       var partes = hfMesAnioVal ? hfMesAnioVal.split('|') : [];

       var anioActual = partes.length === 2 ? parseInt(partes[0]) : new Date().getFullYear();
       var mesActual = partes.length === 2 ? parseInt(partes[1]) - 1 : new Date().getMonth();

       var meses = ["Enero", "Febrero", "Marzo", "Abril", "Mayo", "Junio",
           "Julio", "Agosto", "Septiembre", "Octubre", "Noviembre", "Diciembre"];

       // Datos de turnos y disponibilidad
       var turnosJson = document.getElementById('<%= hfTurnosJson.ClientID %>').value;
       var turnos = (turnosJson && turnosJson.trim() !== "") ? JSON.parse(turnosJson) : [];
       var dispRaw = document.getElementById('<%= hfDisponibilidad.ClientID %>').value;
    var diasLaborales = parsearDisponibilidad(dispRaw);

    // 2. DISPARADOR DE DIBUJO (Apenas carga la página)
    document.addEventListener("DOMContentLoaded", function() {
        renderCalendario(turnos);
    });

    // 3. NAVEGACIÓN (Flechas)
    function cambiarMes(delta) {
        mesActual += delta;
        if (mesActual > 11) { mesActual = 0;  anioActual++; }
        if (mesActual < 0)  { mesActual = 11; anioActual--; }
        
        // Guardamos el nuevo valor en el HiddenField (Mes va de 1 a 12 para C#)
        var hf = document.getElementById('<%= hfMesAnio.ClientID %>');
        hf.value = anioActual + '|' + (mesActual + 1);
        
        // Ejecutamos el PostBack para que el servidor procese el cambio
        __doPostBack('<%= hfMesAnio.ClientID %>', '');
       }

       // 4. RENDERIZADO DEL CALENDARIO
       function renderCalendario(ts) {
           var grid = document.getElementById('calGrid');
           if (!grid) return;
           grid.innerHTML = '';
           document.getElementById('calTitulo').textContent = meses[mesActual] + ' ' + anioActual;

           var hoy = new Date();
           var primerDia = new Date(anioActual, mesActual, 1).getDay();
           var diasMes = new Date(anioActual, mesActual + 1, 0).getDate();

           // Celdas vacías
           for (var i = 0; i < primerDia; i++) {
               var v = document.createElement('div');
               v.className = 'cal-dia vacio';
               grid.appendChild(v);
           }

           // Días del mes
           for (var d = 1; d <= diasMes; d++) {
               var diaSem = new Date(anioActual, mesActual, d).getDay();
               var esHoy = d === hoy.getDate() && mesActual === hoy.getMonth() && anioActual === hoy.getFullYear();
               var esLaboral = !!diasLaborales[diaSem];
               var fechaStr = anioActual + '-' + pad(mesActual + 1) + '-' + pad(d);

               var tDia = ts.filter(function (t) { return t.fecha === fechaStr; });

               var cell = document.createElement('div');
               cell.className = 'cal-dia' +
                   (!esLaboral ? ' no-laboral' : '') +
                   (esHoy ? ' hoy' : '') +
                   (tDia.length > 0 ? ' con-turnos' : '');

               cell.innerHTML = '<div class="cal-num">' + d + '</div>';

               // Dibujar chips (Servicio - Cliente)
               tDia.slice(0, 2).forEach(function (t) {
                   cell.innerHTML += '<span class="turno-chip text-truncate">' +
                       t.servicio + ' - ' + t.cliente +
                       '</span>';
               });

               if (tDia.length > 2) {
                   cell.innerHTML += '<div class="mas-turnos">+' + (tDia.length - 2) + ' más</div>';
               }

               // Click para detalle
               (function (dia, td) {
                   if (esLaboral) {
                       cell.onclick = function () { abrirModal(dia, td); };
                   }
               })(d, tDia);

               grid.appendChild(cell);
           }
       }

       // 5. MODAL DE DETALLE
       function abrirModal(dia, tDia) {
           document.getElementById('modalDiaTitulo').textContent =
               '📅 ' + pad(dia) + '/' + pad(mesActual + 1) + '/' + anioActual;

           var body = document.getElementById('modalDiaBody');
           if (!tDia || !tDia.length) {
               body.innerHTML = '<p class="text-center text-muted">Sin turnos para hoy.</p>';
           } else {
               body.innerHTML = tDia.map(function (t) {
                   return '<div class="turno-detalle p-2 mb-2" style="background:rgba(255,255,255,0.05); border-radius:8px;">' +
                       '<div class="fw-bold text-white">👤 ' + t.cliente + '</div>' +
                       '<div class="text-info small">🔧 ' + t.servicio + '</div>' +
                       '</div>';
               }).join('');
           }
           var modal = new bootstrap.Modal(document.getElementById('modalDia'));
           modal.show();
       }

       // 6. FUNCIONES AUXILIARES
       function parsearDisponibilidad(raw) {
           var dias = {};
           if (!raw) return dias;
           var map = { "Domingos": 0, "Lunes": 1, "Martes": 2, "Miércoles": 3, "Jueves": 4, "Viernes": 5, "Sábados": 6 };
           var partes = raw.split(",");
           for (var i = 0; i + 3 < partes.length; i += 4) {
               if (partes[i + 1].trim() === "1" && map[partes[i].trim()] !== undefined)
                   dias[map[partes[i].trim()]] = true;
           }
           return dias;
       }

       function pad(n) { return n < 10 ? '0' + n : '' + n; }
</script>

</asp:Content>
