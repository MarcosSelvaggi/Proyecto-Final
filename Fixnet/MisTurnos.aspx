<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="MisTurnos.aspx.cs" Inherits="Fixnet.MisTurnos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Repeater ID="rptTurnos" runat="server">
    <ItemTemplate>
        <div class="card mb-3 shadow-sm">
            <div class="card-body">

                <h5 class="card-title">
                    <%# Eval("Servicio") %>
                </h5>

                <p class="mb-1">
                    <strong>Prestador:</strong>
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
</asp:Content>
