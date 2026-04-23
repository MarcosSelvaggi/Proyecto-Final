<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="MisTurnos.aspx.cs" Inherits="Fixnet.MisTurnos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/StyleMisTurnos.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Repeater ID="rptTurnos" runat="server">
        <ItemTemplate>
            <div class="card mb-3 shadow-sm">
                <div class="card-body">

                    <h5 class="card-title d-flex justify-content-between align-items-center">

                        <%# Eval("Servicio") %>

                        <span class='badge <%# ObtenerClaseEstado(Eval("Estado").ToString()) %>'>
                            <%# Eval("Estado") %>
                        </span>

                    </h5>

                    <p class="mb-1">
                        <strong>Prestador:</strong>
                        <%# Eval("Nombre") %> <%# Eval("Apellido") %>

                        <asp:Button Text="Calificar"
                            CssClass="btn btn-outline-primary"
                            ID="Button"
                            runat="server"
                            OnCommand="CalificarPrestador"
                            CommandName="Calificar"
                            CommandArgument='<%# Eval("IdTurno") %>' />                     
                    </p>

                    <p class="mb-1">
                        <strong>Teléfono:</strong>
                        <%# Eval("Telefono") %>
                    </p>

                    <p class="mb-1">
                        <strong>Email:</strong>
                        <%# Eval("Email") %>
                    </p>

                    <p class="mb-1">
                        <strong>Descripción:</strong>
                        <%# Eval("Descripcion") %>
                    </p>

                    <p class="mb-1">
                        <strong>Mensaje:</strong>
                        <%# Eval("Mensaje") %>
                    </p>

                    <p class="text-muted mt-2">
                        Solicitud enviada el:
                    <%# Eval("FechaSolicitud", "{0:dd/MM/yyyy HH:mm}") %>
                    </p>

                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>

    <!-- MODALES -->
    <!-- MODAL PARA CALIFICAR EL TURNO -->
    <div class="modal fade" id="ModalCalificarPrestador" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="Calificar Prestador" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Califica el servicio de <label Id="LblNombrePrestador"></label></h5> 
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <p>¿Cómo estuvo el servicio?</p>
                    <div class="star-rating">
                        <input type="radio" id="Estrella5" name="Puntuacion" value="5" onclick="CambiarPuntuacion(5)" /><label for="Estrella5">★</label>
                        <input type="radio" id="Estrella4" name="Puntuacion" value="4" onclick="CambiarPuntuacion(4)" checked="checked" /><label for="Estrella4">★</label>
                        <input type="radio" id="Estrella3" name="Puntuacion" value="3" onclick="CambiarPuntuacion(3)" /><label for="Estrella3">★</label>
                        <input type="radio" id="Estrella2" name="Puntuacion" value="2" onclick="CambiarPuntuacion(2)" /><label for="Estrella2">★</label>
                        <input type="radio" id="Estrella1" name="Puntuacion" value="1" onclick="CambiarPuntuacion(1)"  /><label for="Estrella1">★</label>
                    </div>

                    <asp:HiddenField runat="server" Value="4" ID="PuntuacionDelPrestador" />
                    
                    <p>Deja algún comentario (opcional)</p>
                    <textarea rows="5" style="resize:none;min-width:100%" id="TxtComentario" runat="server"></textarea>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Volver</button>
                    <asp:Button Text="Calificar" CssClass="btn btn-outline-success" runat="server" Id="BtnCalificar" OnClick="BtnCalificar_Click"/>
                </div>
            </div>
        </div>
    </div>

    <!-- MODAL DE TURNO CALIFICADO -->
    <div class="modal fade" id="ModalTurnoCalificado" tabindex="-1" aria-labelledby="Modal Turno Calificado" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="TituloTurnoCalificado">Turno calificado</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <p>El turno se ha calificado correctamente.</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-primary" data-bs-dismiss="modal">Confirmar</button>
                    <asp:Button Text="Volver al perfil" CssClass="btn btn-outline-success" runat="server" ID="BtnVolverAlPerfil" OnClick="BtnVolverAlPerfil_Click"/>
                </div>
            </div>
        </div>
    </div>


    <!-- MODAL NO SE PUDO CALIFICAR EL TURNO -->
    <div class="modal fade" id="ModalTurnoNoCalificado" tabindex="-1" aria-labelledby="Modal Turno No Calificado" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="TituloTurnoNoCalificado">Ocurrió un error</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <p>Hubo un error al calificar el turno, inténtalo de nuevo más tarde.</p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Cerrar</button>
                    <asp:Button Text="Volver al perfil" CssClass="btn btn-outline-success" runat="server" ID="BtnVolverAlPerfilNoCalificado" OnClick="BtnVolverAlPerfil_Click"/>
                </div>
            </div>
        </div>
    </div>

    <!-- SCRIPTS -->

    <script>
        function CambiarPuntuacion(Valor) {
            document.getElementById('<%= PuntuacionDelPrestador.ClientID %>').value = Valor;
        }
    </script>

</asp:Content>
