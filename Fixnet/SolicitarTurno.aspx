<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true"
    CodeBehind="SolicitarTurno.aspx.cs" Inherits="Fixnet.SolicitarTurno" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/StyleSolicitarTurno.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container py-4 turno-container">

        <!-- HEADER PERFIL -->
        <div class="perfil-box mb-4">

            <div class="d-flex align-items-center gap-3">
                <asp:Image ID="ImgFotoPrestador" runat="server"
                    CssClass="avatar-foto-grande" AlternateText="Foto"
                    Visible="false" />
                <div class="avatar-grande" id="divInicialesPrestador" runat="server">
                    <asp:Label runat="server" ID="LblIniciales" />
                </div>

                <div class="flex-grow-1">
                    <h4 style="text-align: left !important;" class="nombre-prestador mb-1">
                        <asp:Label runat="server" ID="LblNombre" />
                    </h4>

                    <div class="sub-info">
                        <asp:Label runat="server" ID="LblEmail" />
                        <span class="mx-1">•</span>
                        <asp:Label runat="server" ID="LblTelefono" />
                    </div>
                </div>
            </div>

        </div>

        <!-- DESCRIPCION -->
        <div class="seccion-box mb-4">
            <h6 class="seccion-title">Sobre el prestador</h6>

            <p class="descripcion-texto">
                <asp:Label runat="server" ID="LblDescripcion" />
            </p>
        </div>

        <!-- SERVICIOS -->
        <div class="seccion-box mb-4">
            <h6 class="seccion-title">Servicios y precios</h6>
            <asp:Repeater runat="server" ID="RptServicios">
                <ItemTemplate>
                    <div class="servicio-item">
                        <div class="d-flex align-items-center gap-2">
                            <asp:RadioButton ID="rbServicio" runat="server" GroupName="Servicios" />
                            <span class="servicio-nombre"><%# Eval("NombreServicio") %></span>
                        </div>
                        <span class="precio-servicio">$<%# Eval("Precio", "{0:N0}") %>/h
                        </span>
                        <asp:HiddenField ID="hfIdServicio" runat="server"
                            Value='<%# Eval("IdServicio") %>' />
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <!-- ZONAS -->
        <div class="seccion-box mb-4">

            <h6 class="seccion-title">Zonas donde trabaja</h6>

            <div class="zonas-wrapper">
                <asp:Repeater runat="server" ID="RptZonas">
                    <ItemTemplate>
                        <span class="zona-badge"><%# Container.DataItem %></span>
                    </ItemTemplate>
                </asp:Repeater>
            </div>

        </div>

        <!-- HORARIOS -->
        <div class="seccion-box mb-4">

            <h6 class="seccion-title">Disponibilidad</h6>

            <asp:Repeater runat="server" ID="RptHorarios">
                <ItemTemplate>

                    <div class="horario-item">

                        <span class="dia"><%# Eval("Dia") %></span>

                        <asp:Panel runat="server" Visible='<%# (bool)Eval("Trabaja") %>'>
                            <span class="horario">
                                <%# Eval("HoraInicio") %>:00 - <%# Eval("HoraFin") %>:00
                            </span>
                        </asp:Panel>

                        <asp:Panel runat="server" Visible='<%# !(bool)Eval("Trabaja") %>'>
                            <span class="no-disponible">No disponible</span>
                        </asp:Panel>

                    </div>

                </ItemTemplate>
            </asp:Repeater>

        </div>
        <!-- BOTON -->
        <div class="text-end">
            <asp:Button ID="BtnSolicitar" runat="server"
                Text="Solicitar turno"
                CssClass="btn-principal"
                OnClick="BtnSolicitar_Click"
                UseSubmitBehavior="false" />
        </div>

    </div>
    <!-- MODAL MENSAJE -->
    <div class="modal fade" id="modalMensaje" tabindex="-1">
        <div class="modal-dialog modal-dialog-centered">

            <div class="modal-content glass-modal">

                <!-- HEADER -->
                <div class="modal-header modal-header-glass">

                    <div class="icon-box">
                        <i class="bi bi-chat-dots"></i>
                    </div>

                    <h5 class="modal-title mb-0">Mensaje al prestador
                    </h5>

                    <button type="button"
                        class="btn-close btn-close-white ms-auto"
                        data-bs-dismiss="modal">
                    </button>

                </div>

                <!-- BODY -->
                <div class="modal-body modal-body-glass">

                    <label class="form-label mb-2" style="color: #94a3b8; font-size: 13px;">
                        Escribí un mensaje opcional
                    </label>

                    <asp:TextBox ID="txtMensaje" runat="server"
                        CssClass="form-control input-dark"
                        TextMode="MultiLine"
                        Rows="4"
                        placeholder="Ej: Hola, me gustaría coordinar horario..." />

                </div>

                <!-- FOOTER -->
                <div class="modal-footer border-0">

                    <asp:Button ID="BtnConfirmarSolicitud" runat="server"
                        Text="Enviar solicitud"
                        CssClass="BtnEnviar"
                        OnClick="BtnConfirmarSolicitud_Click" />

                </div>
            </div>

        </div>
    </div>
    <!-- MODAL SISTEMA -->
    <div class="modal fade" id="modalMensajeSistema" tabindex="-1">
        <div class="modal-dialog modal-dialog-centered">

            <div class="modal-content glass-modal">

                <div class="modal-header modal-header-glass" id="modalHeader">
                    <h5 class="modal-title d-flex align-items-center gap-2">
                        <span id="modalIcon"></span>
                        Aviso
                    </h5>
                    <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
                </div>

                <div class="modal-body modal-body-glass">
                    <asp:Label ID="LblMensajeSistema" runat="server" />
                </div>

                <div class="modal-footer">
                    <button type="button" class="BtnEnviar" data-bs-dismiss="modal">
                        OK
                    </button>
                </div>

            </div>

        </div>
    </div>
    <script>
        document.addEventListener('change', function (e) {
            if (e.target && e.target.type === 'radio') {
                document.querySelectorAll('input[type="radio"]').forEach(function (r) {
                    r.checked = false;
                });
                e.target.checked = true;
            }
        });
    </script>
</asp:Content>
