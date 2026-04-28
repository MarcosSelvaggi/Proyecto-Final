<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="MisTurnos.aspx.cs" Inherits="Fixnet.MisTurnos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/StyleMisTurnos.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container py-4 container-turnos" style="max-width: 780px;">
        <asp:ScriptManager runat="server" />
        <div class="page-header">
            <span style="font-size: 2rem;">📅</span>
            <div>
                <h4>Mis turnos</h4>
                <p>Seguí el estado de tus solicitudes a prestadores.</p>
            </div>
        </div>
        <asp:UpdatePanel runat="server">
            <ContentTemplate>

           
        <asp:Repeater ID="rptTurnos" runat="server">
            <ItemTemplate>
                <div class='turno-card <%# Eval("Estado").ToString() == "Aceptado" ? "turno-aceptado" : Eval("Estado").ToString() == "Rechazado" ? "turno-rechazado" : "turno-pendiente" %>'>

                    <!-- Header -->
                    <div class="turno-card-header">
                        <span class="turno-servicio"><%# Eval("Servicio") %></span>
                        <span class='badge-estado <%# ObtenerClaseEstado(Eval("Estado").ToString()) %>'>
                            <%# Eval("Estado") %>
                        </span>
                    </div>

                    <!-- Body -->
                    <div class="turno-card-body">
                        <div class="turno-info-grid">
                            <div class="turno-info-item">
                                <strong>Prestador</strong>
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
                                <strong>Descripción</strong>
                                <%# Eval("Descripcion") %>
                            </div>
                        </div>

                        <%# string.IsNullOrWhiteSpace(Eval("Mensaje").ToString())
                            ? ""
                            : "<div class='turno-mensaje'>💬 " + Eval("Mensaje") + "</div>" %>

                        <div class="turno-fecha">
                            🕐 Solicitud enviada el <%# Eval("FechaSolicitud", "{0:dd/MM/yyyy HH:mm}") %>
                        </div>
                    </div>

                    <!-- Acción calificar -->
                    <div class="turno-acciones">
                        <asp:Button Text="⭐ Calificar"
                            CssClass="btn-calificar"
                            ID="Button"
                            runat="server"
                            OnCommand="CalificarPrestador"
                            CommandName="Calificar"
                            CommandArgument='<%# Eval("IdTurno") %>' Visible='<%#HabilitarBotonCalificar(Eval("Estado").ToString())%>' />
                        <!-- MENSAJE -->
                        <a href='<%# "Mensajes.aspx?conv=" + ObtenerOCrearConv(
                            Convert.ToInt32(Eval("IdTurno")),
                            Convert.ToInt32(Eval("IdCliente")),
                            Convert.ToInt32(Eval("IdPrestador"))) %>'
                            class="btn-mensajes">💬 Mensajes
                       </a>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </ContentTemplate>
</asp:UpdatePanel>
    </div>

    <!-- MODAL CALIFICAR -->
    <div class="modal fade" id="ModalCalificarPrestador" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content glass-modal">
                <div class="modal-header modal-header-glass">
                    <h5 class="modal-title modal-title-glass">Calificá el servicio</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body modal-body-glass">
                    <p>¿Cómo estuvo el servicio?</p>
                    <div class="star-rating">
                        <input type="radio" id="Estrella5" name="Puntuacion" value="5" onclick="CambiarPuntuacion(5)" /><label for="Estrella5">★</label>
                        <input type="radio" id="Estrella4" name="Puntuacion" value="4" onclick="CambiarPuntuacion(4)" checked="checked" /><label for="Estrella4">★</label>
                        <input type="radio" id="Estrella3" name="Puntuacion" value="3" onclick="CambiarPuntuacion(3)" /><label for="Estrella3">★</label>
                        <input type="radio" id="Estrella2" name="Puntuacion" value="2" onclick="CambiarPuntuacion(2)" /><label for="Estrella2">★</label>
                        <input type="radio" id="Estrella1" name="Puntuacion" value="1" onclick="CambiarPuntuacion(1)" /><label for="Estrella1">★</label>
                    </div>
                    <asp:HiddenField runat="server" Value="4" ID="PuntuacionDelPrestador" />
                    <p class="mt-2">Dejá un comentario (opcional)</p>
                    <textarea rows="4" style="resize: none; width: 100%; background: rgba(255,255,255,0.05); border: 1px solid rgba(255,255,255,0.15); border-radius: 10px; color: #e2e8f0; padding: 10px;"
                        id="TxtComentario" runat="server"></textarea>
                </div>
                <div class="modal-footer" style="border-top: 1px solid rgba(255,255,255,0.08);">
                    <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Volver</button>
                    <asp:Button Text="Calificar" CssClass="btn-calificar" runat="server" ID="BtnCalificar" OnClick="BtnCalificar_Click"/>
                </div>
            </div>
        </div>
    </div>

    <!-- MODAL TURNO CALIFICADO -->
    <div class="modal fade" id="ModalTurnoCalificado" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content glass-modal">
                <div class="modal-header modal-header-glass">
                    <h5 class="modal-title modal-title-glass">Turno calificado ✅</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body modal-body-glass">
                    <p>El turno se calificó correctamente.</p>
                </div>
                <div class="modal-footer" style="border-top: 1px solid rgba(255,255,255,0.08);">
                    <asp:Button Text="Confirmar" class="btn btn-outline-primary" data-bs-dismiss="modal" runat="server" ID="BtnConfirmarCalificacion" OnClick="BtnConfirmarCalificacion_Click"/>
                    <asp:Button Text="Volver al perfil" CssClass="btn-calificar" runat="server" ID="BtnVolverAlPerfil" OnClick="BtnVolverAlPerfil_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- MODAL ERROR CALIFICACIÓN -->
    <div class="modal fade" id="ModalTurnoNoCalificado" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content glass-modal">
                <div class="modal-header modal-header-glass">
                    <h5 class="modal-title modal-title-glass">Ocurrió un error ❌</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>
                <div class="modal-body modal-body-glass">
                    <p>Hubo un error al calificar el turno. Intentalo de nuevo más tarde.</p>
                </div>
                <div class="modal-footer" style="border-top: 1px solid rgba(255,255,255,0.08);">
                    <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Cerrar</button>
                    <asp:Button Text="Volver al perfil" CssClass="btn-calificar" runat="server" ID="BtnVolverAlPerfilNoCalificado" OnClick="BtnVolverAlPerfil_Click" />
                </div>
            </div>
        </div>
    </div>
    <script>
        function CambiarPuntuacion(Valor) {
            document.getElementById('<%= PuntuacionDelPrestador.ClientID %>').value = Valor;
        }
    </script>

</asp:Content>
