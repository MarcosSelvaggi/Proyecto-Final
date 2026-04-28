<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="TurnosSolicitados.aspx.cs" Inherits="Fixnet.TurnosSolicitados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/TurnosSolicitados.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:HiddenField ID="hfIdTurnoAceptar" runat="server" />
    <asp:HiddenField ID="hfIdPrestador" runat="server" />
    <asp:HiddenField ID="hfDisponibilidad" runat="server" />

    <div class="container py-4 container-turnos" style="max-width:780px;">
        <div class="page-header">
            <span style="font-size:2rem;">📋</span>
            <div>
                <h4>Turnos solicitados</h4>
                <p>Gestioná las solicitudes que recibiste de tus clientes.</p>
            </div>
        </div>

        <asp:Repeater ID="rptTurnos" runat="server">
            <ItemTemplate>
                <!-- <div class='turno-card <%# Eval("Estado").ToString() == "Aceptado" ? "turno-aceptado" : Eval("Estado").ToString() == "Rechazado" ? "turno-rechazado" : "" %>'> -->
                <div class='<%# ObtenerEstadoDelTurno(Eval("Estado").ToString()) %>' runat="server">
                    <div class="turno-card-header">
                        <span class="turno-servicio"><%# Eval("Servicio") %></span>
                        <span class='badge-estado <%# ObtenerClaseEstado(Eval("Estado").ToString()) %>'>
                            <%# Eval("Estado") %>
                        </span>
                    </div>
                    <div class="turno-card-body">
                        <div class="turno-info-grid">
                            <div class="turno-info-item">
                                <strong>Cliente</strong>
                                <%# Eval("Nombre") %> <%# Eval("Apellido") %>
                            </div>
                            <div class="turno-info-item">
                                <strong>Teléfono</strong>
                                <%# Eval("Telefono") %>
                            </div>
                            <div class="turno-info-item">
                                <strong>Email</strong>
                                <%# Eval("Email") %>
                            </div>
                            <div class="turno-info-item">
                                <strong>Dirección</strong>
                                <%# Eval("Direccion") %> — <%# Eval("Localidad") %>
                            </div>
                        </div>
                        <%# string.IsNullOrWhiteSpace(Eval("Mensaje").ToString()) ? "" : "<div class='turno-mensaje'>💬 " + Eval("Mensaje") + "</div>" %>
                        <div class="turno-fecha">
                            🕐 Solicitud recibida el <%# Eval("FechaSolicitud", "{0:dd/MM/yyyy HH:mm}") %>
                        </div>
                    </div>
                    <div class="turno-acciones" runat="server" visible='<%# Eval("Estado").ToString() != "Calificado" %>'>
                        <div class="turno-acciones" runat="server" visible='<%# Eval("Estado").ToString() != "Rechazado" %>'>
                            <button type="button" class="btn-aceptar" onclick="abrirModalFecha('<%# Eval("IdTurno") %>')">
                                <%# Eval("Estado").ToString() == "Aceptado" ? "🔁 Reagendar" : "✔ Aceptar" %>
                            </button>
                            <asp:Button ID="btnRechazar" runat="server" Text="✖ Rechazar" CssClass="btn-rechazar"
                                CommandArgument='<%# Eval("IdTurno") %>' OnCommand="RechazarTurno"
                                Visible='<%# Eval("Estado").ToString() == "Pendiente" %>' />
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <!-- MODAL ELEGIR FECHA Y HORA -->
    <div class="modal fade" id="modalFecha" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content glass-modal">
                <div class="modal-header modal-header-glass">
                    <h5 class="modal-title-glass">📅 Programar Fecha de Turno</h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body p-4">
                    <div class="mb-4">
                        <label class="form-label text-white-50">Seleccionar Día</label>
                        <input type="date" id="inputFecha" name="inputFecha" 
                               class="form-control bg-dark text-white border-secondary shadow-none" 
                               onchange="validarFecha()"  />
                    </div>
                    <div id="msgDiaNoLaboral" class="alert alert-warning py-2 mb-0" style="display:none; font-size:0.85rem;">
                        <i class="bi bi-exclamation-triangle-fill me-2"></i>No tenés este día de la semana habilitado como laboral.
                    </div>
                </div>
                <div class="modal-footer border-0 pb-4 px-4">
                    <asp:Button ID="btnConfirmarAceptar" runat="server" Text="Confirmar Turno" 
                        CssClass="btn-aceptar w-100 py-2 shadow-sm" 
                        OnClick="btnConfirmarAceptar_Click" 
                        OnClientClick="return validarFecha();" 
                        Enabled="false" />
                </div>
            </div>
        </div>
    </div>

    <script>
        //NO MOVER A UN .JS PORQUE LAS VARIABLES NO FUNCIONAN (HAY QUE CAMBIAR LOGICA)
        // Días laborales parseados desde el servidor
        var diasLaborales = {};
        var mapDia = {
            "Domingos": 0, "Lunes": 1, "Martes": 2, "Miércoles": 3,
            "Jueves": 4, "Viernes": 5, "Sábados": 6
        };

        function parsearDisponibilidad(raw) {
            diasLaborales = {};
            if (!raw) return;
            var partes = raw.split(",");
            for (var i = 0; i + 3 < partes.length; i += 4) {
                var nombre = partes[i].trim();
                var trabaja = partes[i + 1].trim() === "1";
                var inicio = parseInt(partes[i + 2]);
                var fin = parseInt(partes[i + 3]);
                if (trabaja && mapDia[nombre] !== undefined) {
                    diasLaborales[mapDia[nombre]] = { inicio: inicio, fin: fin };
                }
            }
        }

        function obtenerBtnConfirmar() {
            return document.getElementById('<%= btnConfirmarAceptar.ClientID %>');
        }

        function validarFecha() {
            var inputFecha = document.getElementById('inputFecha');
            var msgDia = document.getElementById('msgDiaNoLaboral');
            var btnConfirmar = obtenerBtnConfirmar();
            
            if (!inputFecha.value) {
                msgDia.style.display = 'none';
                if (btnConfirmar) btnConfirmar.disabled = true;
                return false;
            }

            var fecha = new Date(inputFecha.value + 'T00:00:00');
            var diaSemana = fecha.getDay();
            var esValido = (diasLaborales[diaSemana] !== undefined);
            
            if (!esValido) {
                msgDia.style.display = 'block';
                inputFecha.classList.add('is-invalid');
                if (btnConfirmar) btnConfirmar.disabled = true;
            } else {
                msgDia.style.display = 'none';
                inputFecha.classList.remove('is-invalid');
                if (btnConfirmar) btnConfirmar.disabled = false;
            }
            return esValido;
        }

        function abrirModalFecha(idTurno) {
            document.getElementById('<%= hfIdTurnoAceptar.ClientID %>').value = idTurno;
            var disp = document.getElementById('<%= hfDisponibilidad.ClientID %>').value;
            parsearDisponibilidad(disp);

            var inputFecha = document.getElementById('inputFecha');
            inputFecha.value = '';
            document.getElementById('msgDiaNoLaboral').style.display = 'none';
            if (document.getElementById('msgConflicto')) {
                document.getElementById('msgConflicto').style.display = 'none';
            }

            var hoy = new Date().toISOString().split('T')[0];
            inputFecha.min = hoy;

            var btnConfirmar = obtenerBtnConfirmar();
            if (btnConfirmar) btnConfirmar.disabled = true;
            if (btnConfirmar && modoModal === "reagendar") {
                btnConfirmar.value = "Confirmar Reagendado";
            } else if (btnConfirmar) {
                btnConfirmar.value = "Confirmar Turno";
            }

            var modal = new bootstrap.Modal(document.getElementById('modalFecha'));
            modal.show();
        }

        var modoModal = "";
        function abrirModalReagendar(idTurno) {
            modoModal = "reagendar";
            abrirModalFecha(idTurno);
        }

        // Función global para eliminar backdrops de modales 
        window.cerrarModalBackdrop = function () {
            var modales = document.querySelectorAll('.modal.show');
            modales.forEach(function (m) {
                var modal = bootstrap.Modal.getInstance(m);
                if (modal) modal.hide();
            });
            var backdrops = document.querySelectorAll('.modal-backdrop');
            backdrops.forEach(function (b) { b.remove(); });
            document.body.classList.remove('modal-open');
        };

        // Al cerrar el modal con la X o con el botón cerrar
        var modalElement = document.getElementById('modalFecha');
        if (modalElement) {
            modalElement.addEventListener('hidden.bs.modal', function () {
                var backdrops = document.querySelectorAll('.modal-backdrop');
                backdrops.forEach(function (b) { b.remove(); });
                document.body.classList.remove('modal-open');
            });
        }
    </script>
</asp:Content>