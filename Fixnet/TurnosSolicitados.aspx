<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="TurnosSolicitados.aspx.cs" Inherits="Fixnet.TurnosSolicitados" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/StyleMisTurnos.css" rel="stylesheet" />
  <link href="styles/TurnosSolicitados.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container py-4 container-turnos" style="max-width: 780px;">

        <div class="page-header">
            <span style="font-size:2rem;">📋</span>
            <div>
                <h4>Turnos solicitados</h4>
                <p>Gestioná las solicitudes que recibiste de tus clientes.</p>
            </div>
        </div>

        <asp:Repeater ID="rptTurnos" runat="server">
            <ItemTemplate>
                <div class='turno-card <%# Eval("Estado").ToString() == "Aceptado" ? "turno-aceptado" : Eval("Estado").ToString() == "Rechazado" ? "turno-rechazado" : "" %>'>

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

                        <%# string.IsNullOrWhiteSpace(Eval("Mensaje").ToString())
                            ? ""
                            : "<div class='turno-mensaje'>💬 " + Eval("Mensaje") + "</div>" %>

                        <div class="turno-fecha">
                            🕐 Solicitud recibida el <%# Eval("FechaSolicitud", "{0:dd/MM/yyyy HH:mm}") %>
                        </div>
                    </div>

                    <!-- Acciones solo si está Pendiente -->
                    <div class="turno-acciones"
                        runat="server"
                        visible='<%# Eval("Estado").ToString() == "Pendiente" %>'>
                        <asp:Button
                            ID="btnAceptar"
                            runat="server"
                            Text="✔ Aceptar"
                            CssClass="btn-aceptar"
                            CommandArgument='<%# Eval("IdTurno") %>'
                            OnCommand="AceptarTurno" />
                        <asp:Button
                            ID="btnRechazar"
                            runat="server"
                            Text="✖ Rechazar"
                            CssClass="btn-rechazar"
                            CommandArgument='<%# Eval("IdTurno") %>'
                            OnCommand="RechazarTurno" />
                    </div>

                </div>
            </ItemTemplate>
        </asp:Repeater>

    </div>

</asp:Content>
