<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ModificarContraseña.aspx.cs" Inherits="Fixnet.CambiarContraseña" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/StyleRegistro.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container2">
        <div style="max-width: 700px; width: 100%;">
            <h2 class="text-center mb-4"><i class="bi bi-caret-right-fill"></i>Modificar contraseña<i class="bi bi-caret-left-fill"></i></h2>
            <asp:Panel runat="server" DefaultButton="BtnModificar">
                <div class="row g-3">
                    <div class="col-md-12">
                        <label class="form-label">Nuevo contraseña</label>
                        <asp:TextBox ID="txtPassword" CssClass="form-control" runat="server" TextMode="Password"></asp:TextBox>
                    </div>

                    <div class="col-12 text-center mt-4">
                        <div class="row">
                            <div class="col-2"></div>
                            <div class="col-4">
                                <div class="btnEnviar">
                                    <asp:Button ID="BtnModificar" runat="server" Text="Modificar" CssClass="BtnEnviar" OnClick="ModificarContraseña_Click" />
                                </div>
                            </div>
                            <div class="col-4">
                                <div class="btnEnviar">
                                    <asp:Button ID="BtnVolver" runat="server" Text="Volver" CssClass="btn btn-outline-dark" OnClick="BtnVolver_Click" />
                                </div>
                            </div>
                        </div>

                        <!-- MENSAJE DE ERROR -->
                        <asp:Label
                            ID="lblError"
                            runat="server"
                            ForeColor="Red"
                            Font-Bold="true"
                            Visible="false"
                            CssClass="mt-3">
                        </asp:Label>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </div>

    <!-- MODALES -->

    <div class="modal fade" id="ModificarContraseñaModal" tabindex="-1" aria-labelledby="ModificarContraseña" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-dialog" style="min-width: 400px; width: 90%">
                <div class="modal-content">
                    <div class="modal-header bg-success text-white">
                        <h1 class="modal-title fs-5" runat="server">Contraseña actualizada correctamente</h1>
                    </div>
                    <div class="modal-body">
                        <p>Se ha actualizado correctamente tu contraseña.</p>
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
