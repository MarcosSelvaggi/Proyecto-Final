<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ContraseñaOlvidada.aspx.cs" Inherits="Fixnet.ContraseñaOlvidada" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/StyleContacto.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:ScriptManager runat="server" />

    <!-- Panel Mail -->
        <div class="container2" id="divIngresarMail" runat="server">
            <h1>Ingrese su mail</h1>
            <div>
                <asp:TextBox ID="txtMail" class="input" TextMode="Email" placeholder="Ingrese su mail" runat="server" />
                <small id="mailNoEncontrado" class="text-danger" runat="server"></small>
                <asp:Button ID="btnRecuperar" class="btnEnviar" Text="Recuperar contraseña" OnClick="btnRecuperar_Click" runat="server" />
            </div>
            <div>
                <br />
                <asp:Button ID="btnVolver" Text="Volver" CssClass="btn btn-outline-dark" OnClick="btnVolver_Click" runat="server" />
                <p></p>
            </div>
        </div>

    <!-- Panel Código -->
        <div class="container2" id="divIngresarCodigo" runat="server">
            <h1>Ingresar código</h1>
            <div>
                <asp:TextBox ID="txtCodigoMail" ClientIDMode="Static" class="input" onkeypress="return soloNumeros(event)" MaxLength="6" runat="server" />
                <small id="smallCodigoIncorrecto" class="text-danger" runat="server"></small>
                <asp:Button ID="btnCodigoMail" class="btnEnviar" Text="Confirmar" OnClick="btnCodigoMail_Click" runat="server" />
            </div>
            <div>
                <br />
                <asp:Button Text="Volver" CssClass="btn btn-outline-dark" OnClick="btnVolver_Click" runat="server" />
            </div>
            <div>
            </div>
        </div>

    <!-- Panel Contraeña nueva-->
        <div class="container2" id="divIngresarContraseña" runat="server">
            <h1>La nueva contraseña</h1>
            <div>
                <asp:TextBox ID="txtContraseñaNueva" TextMode="Password" class="input" runat="server" />
                <small id="SmallCotraseñaNueva" class="text-danger" runat="server"></small>
                <asp:Button ID="btnCambiarContraseña" class="btnEnviar" Text="Confirmar" OnClick="btnCambiarContraseña_Click" runat="server" />
            </div>
            <div>
                <br />
                <asp:Button ID="btnCancelarCambioContrasena" Text="Volver" CssClass="btn btn-outline-dark" OnClick="btnVolver_Click" runat="server" />
            </div>
            <div>
            </div>
        </div>

    <div class="modal fade" id="contrasenaCambiadaModal" tabindex="-1" aria-labelledby="contrasenaCambiadaModal" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-dialog" style="min-width: 400px; width: 90%">
                <div class="modal-content">
                    <div class="modal-header bg-success text-white">
                        <h1 class="modal-title fs-5" runat="server">Contraseña cambiada</h1>
                    </div>
                    <div class="modal-body">
                        <p>Se ha cambiado correctamente su contraseña.</p>
                        <p>En unos segundos serás redirigido a tu perfil.</p>
                    </div>
                    <div class="modal-footer">
                        <a href="/PerfilUsuario.aspx" class="btn btn-dark ms-auto">Ir ahora</a>
                        <div class="col-4"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
