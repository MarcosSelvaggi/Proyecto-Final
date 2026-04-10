<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master"
AutoEventWireup="true" CodeBehind="PerfilUsuario.aspx.cs"
Inherits="Fixnet.PerfilUsuario" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="container mt-4">

    <!-- USUARIO -->
    <div class="card mb-3">
        <div class="card-body">
            <h4>Usuario</h4>
            <p><b>Nombre:</b> <asp:Label ID="lblNombre" runat="server" /></p>
            <p><b>Apellido:</b> <asp:Label ID="lblApellido" runat="server" /></p>
            <p><b>Email:</b> <asp:Label ID="lblEmail" runat="server" /></p>
            <p><b>Teléfono:</b> <asp:Label ID="lblTelefono" runat="server" /></p>
        </div>
    </div>

    <!-- CLIENTE -->
    <div class="card mb-3">
        <div class="card-body">
            <h4>Cliente</h4>

            <asp:Panel ID="pnlClienteDatos" runat="server">
                <p><b>Provincia:</b> <asp:Label ID="lblProvincia" runat="server" /></p>
                <p><b>Departamento:</b> <asp:Label ID="lblDepartamento" runat="server" /></p>
                <p><b>Localidad:</b> <asp:Label ID="lblLocalidad" runat="server" /></p>
                <p><b>Dirección:</b> <asp:Label ID="lblDireccion" runat="server" /></p>

                <asp:Button ID="btnEditarCliente" runat="server"
                    Text="Editar"
                    CssClass="btn btn-primary"
                    OnClick="btnEditarCliente_Click" />
            </asp:Panel>

            <asp:Panel ID="pnlClienteVacio" runat="server">
                <p>No completaste perfil de cliente</p>

                <asp:Button ID="btnCrearCliente" runat="server"
                    Text="Completar"
                    CssClass="btn btn-success"
                    OnClick="btnCrearCliente_Click" />
            </asp:Panel>

        </div>
    </div>

    <!-- PRESTADOR -->
    <div class="card mb-3">
        <div class="card-body">
            <h4>Prestador</h4>

            <asp:Panel ID="pnlPrestadorDatos" runat="server">
                <p><b>Descripción:</b> <asp:Label ID="lblDescripcion" runat="server" /></p>
                <p><b>Zonas:</b> <asp:Label ID="lblZonas" runat="server" /></p>

                <asp:Button ID="btnEditarPrestador" runat="server"
                    Text="Editar"
                    CssClass="btn btn-primary"
                    OnClick="btnEditarPrestador_Click" />
            </asp:Panel>

            <asp:Panel ID="pnlPrestadorVacio" runat="server">
                <p>No completaste perfil de prestador</p>

                <asp:Button ID="btnCrearPrestador" runat="server"
                    Text="Completar"
                    CssClass="btn btn-success"
                    OnClick="btnCrearPrestador_Click" />
            </asp:Panel>

        </div>
    </div>

</div>

</asp:Content>