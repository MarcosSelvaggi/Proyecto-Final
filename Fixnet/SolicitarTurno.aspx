<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="SolicitarTurno.aspx.cs" Inherits="Fixnet.SolicitarTurno" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .avatar-grande {
            width: 64px;
            height: 64px;
            border-radius: 50%;
            background: #e0e7ff;
            color: #4338ca;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: 22px;
            font-weight: 500;
            flex-shrink: 0;
        }

        .horario-dia {
            min-width: 100px;
        }

        .badge-zona {
            font-size: 13px;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container py-4" style="max-width: 750px">

        <%-- Encabezado --%>
        <div class="d-flex align-items-center gap-3 mb-4">
            <div class="avatar-grande">
                <asp:Label runat="server" ID="LblIniciales" />
            </div>
            <div>
                <h4 class="mb-0">
                    <asp:Label runat="server" ID="LblNombre" />
                </h4>
                <small class="text-muted">
                    <asp:Label runat="server" ID="LblEmail" />
                    <asp:Label runat="server" ID="LblTelefono" />
                </small>
            </div>
        </div>

        <%-- Descripción --%>
        <div class="mb-4">
            <h6 class="text-muted text-uppercase" style="font-size: 11px; letter-spacing: .08em">Sobre el prestador</h6>
            <p class="mb-0">
                <asp:Label runat="server" ID="LblDescripcion" />
            </p>
        </div>

        <hr class="my-3" />

        <%-- Servicios --%>
        <div class="mb-4">
            <h6 class="text-muted text-uppercase mb-3" style="font-size: 11px; letter-spacing: .08em">Servicios y precios</h6>
            <asp:Repeater runat="server" ID="RptServicios">
                <ItemTemplate>
                    <div class="d-flex justify-content-between align-items-center py-2 border-bottom">

                        <div>
                            <asp:RadioButton ID="rbServicio" runat="server" GroupName="Servicios" />
                            <span><%# Eval("NombreServicio") %></span>
                        </div>

                        <span class="badge bg-light text-dark border fw-medium">$<%# Eval("Precio", "{0:N0}") %>/h
            </span>

                        <asp:HiddenField ID="hfIdServicio" runat="server"
                            Value='<%# Eval("IdServicio") %>' />
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <hr class="my-3" />

        <%-- Zonas --%>
        <div class="mb-4">
            <h6 class="text-muted text-uppercase mb-3" style="font-size: 11px; letter-spacing: .08em">Zonas donde trabaja</h6>
            <div class="d-flex flex-wrap gap-2">
                <asp:Repeater runat="server" ID="RptZonas">
                    <ItemTemplate>
                        <span class="badge bg-light text-dark border badge-zona"><%# Container.DataItem %></span>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>

        <hr class="my-3" />

        <%-- Horarios --%>
        <div class="mb-4">
            <h6 class="text-muted text-uppercase mb-3" style="font-size: 11px; letter-spacing: .08em">Disponibilidad</h6>
            <asp:Repeater runat="server" ID="RptHorarios">
                <ItemTemplate>
                    <div class="d-flex align-items-center py-2 border-bottom">
                        <span class="horario-dia fw-medium"><%# Eval("Dia") %></span>
                        <asp:Panel runat="server" Visible='<%# (bool)Eval("Trabaja") %>'>
                            <span class="text-muted small">
                                <%# Eval("HoraInicio") %>:00 hs &nbsp;–&nbsp; <%# Eval("HoraFin") %>:00 hs
                            </span>
                        </asp:Panel>
                        <asp:Panel runat="server" Visible='<%# !(bool)Eval("Trabaja") %>'>
                            <span class="text-muted small">No disponible</span>
                        </asp:Panel>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <hr class="my-3" />

        <%-- Botón turno  --%>
        <div class="text-end">
            <asp:Button ID="BtnSolicitar" runat="server"
                Text="Solicitar turno"
                CssClass="btn btn-primary"
                OnClick="BtnSolicitar_Click"
                UseSubmitBehavior="false" />
        </div>

    </div>
    <div class="modal fade" id="modalMensaje" tabindex="-1">
        <div class="modal-dialog">
            <div class="modal-content">

                <div class="modal-header">
                    <h5 class="modal-title">Mensaje al prestador</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>

                <div class="modal-body">
                    <asp:TextBox ID="txtMensaje" runat="server"
                        CssClass="form-control"
                        TextMode="MultiLine"
                        Rows="4"
                        placeholder="Escribí un mensaje opcional..." />
                </div>

                <div class="modal-footer">
                    <asp:Button ID="BtnConfirmarSolicitud" runat="server"
                        Text="Enviar solicitud"
                        CssClass="btn btn-success"
                        OnClick="BtnConfirmarSolicitud_Click" />
                </div>

            </div>
        </div>
    </div>

    <div class="modal fade" id="modalMensajeSistema" tabindex="-1">
    <div class="modal-dialog modal-dialog-centered">
        <div class="modal-content border-0">

            <div class="modal-header text-white" id="modalHeader">
                <h5 class="modal-title d-flex align-items-center gap-2">
                    <span id="modalIcon"></span>
                    <span>Aviso</span>
                </h5>
                <button type="button" class="btn-close btn-close-white" data-bs-dismiss="modal"></button>
            </div>

            <div class="modal-body">
                <asp:Label ID="LblMensajeSistema" runat="server" />
            </div>

            <div class="modal-footer">
                <button type="button" class="btn btn-primary" data-bs-dismiss="modal">
                    OK
                </button>
            </div>

        </div>
    </div>
</div>

</asp:Content>

