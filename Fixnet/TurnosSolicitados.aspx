<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="TurnosSolicitados.aspx.cs" Inherits="Fixnet.TurnosSolicitados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/StyleMisTurnos.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Repeater ID="rptTurnos" runat="server">
        <ItemTemplate>
            <div class="turno-card">
                <div class="card-body">

                    <h5 class="card-title d-flex justify-content-between align-items-center">

                        <%# Eval("Servicio") %>

                        <span class='badge-estado <%# ObtenerClaseEstado(Eval("Estado").ToString()) %>'>
                            <%# Eval("Estado") %>
                        </span>

                    </h5>

                    <p class="mb-1">
                        <strong>Cliente:</strong>
                        <%# Eval("Nombre") %> <%# Eval("Apellido") %>
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
                        <strong>Dirección:</strong>
                        <%# Eval("Direccion") %> - <%# Eval("Localidad") %>
                    </p>

                    <p class="mb-1">
                        <strong>Mensaje:</strong>
                        <%# Eval("Mensaje") %>
                    </p>

                    <p class="text-muted mt-2">
                        Solicitud:
                    <%# Eval("FechaSolicitud", "{0:dd/MM/yyyy HH:mm}") %>
                    </p>

                    <div class="mt-3 d-flex gap-2"
                        runat="server"
                        visible='<%# Eval("Estado").ToString() == "Pendiente" %>'>

                        <asp:Button
                            ID="btnAceptar"
                            runat="server"
                            Text="Aceptar"
                            CssClass="btn-aceptar"
                            CommandArgument='<%# Eval("IdTurno") %>'
                            OnCommand="AceptarTurno" />

                        <asp:Button
                            ID="btnRechazar"
                            runat="server"
                            Text="Rechazar"
                            CssClass="btn-rechazar"
                            CommandArgument='<%# Eval("IdTurno") %>'
                            OnCommand="RechazarTurno" />

                    </div>

                </div>
            </div>
        </ItemTemplate>
    </asp:Repeater>
</asp:Content>
