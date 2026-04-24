<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Logearse.aspx.cs" Inherits="Fixnet.Logearse" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/StyleRegistro.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <div class="registro-container">
            <div class="registro-card">

                <h2 class="titulo">Iniciar sesión</h2>

                <asp:Panel ID="Panel" DefaultButton="BtnEnviar" runat="server">

                    <!-- EMAIL -->
                    <div class="floating">
                        <asp:TextBox
                            ID="txtMail"
                            runat="server"
                            ClientIDMode="Static"
                            CssClass="form-control"
                            TextMode="Email">
                        </asp:TextBox>
                        <label>Email</label>
                        <span class="icon">📧</span>
                        <small id="mailErrorMsj" class="error"></small>
                    </div>

                    <!-- PASSWORD -->
                    <div class="floating">
                        <asp:TextBox
                            ID="txtPass"
                            runat="server"
                            ClientIDMode="Static"
                            CssClass="form-control"
                            TextMode="Password">
                        </asp:TextBox>
                        <label>Contraseña</label>
                        <span class="icon">🔒</span>
                        <span class="icon-password2" onclick="togglePass('txtPass')">👁</span>
                        <small id="contrasenaErrorMsj" class="error"></small>
                    </div>

                    <!-- ERROR GENERAL -->
                    <asp:Label
                        ID="lblError"
                        runat="server"
                        CssClass="error-general"
                        Visible="false">
                    </asp:Label>

                    <!-- BOTON -->
                    <asp:Button
                        ID="BtnEnviar"
                        runat="server"
                        Text="Iniciar sesión"
                        CssClass="btn-registro"
                        OnClick="BtnEnviar_Click" />

                </asp:Panel>

                <!-- LINKS -->
                <div style="margin-top: 20px; text-align: center;">
                    <span>¿Olvidaste tu contraseña? </span>
                    <a href="/ContraseñaOlvidada.aspx" class="hover-underline">Recuperar</a>
                </div>

                <div style="margin-top: 10px; text-align: center;">
                    <span>¿No tenés cuenta? </span>
                    <a href="/Registro.aspx" class="hover-underline">Registrate</a>
                </div>

            </div>
        </div>

        <!-- MODAL -->
        <div class="modal fade" id="contraseñaIncorrectaModal" tabindex="-1">
            <div class="modal-dialog modal-dialog-centered">
                <div class="modal-dialog" style="min-width: 400px; width: 90%">
                    <div class="modal-content">
                        <div class="modal-header bg-danger text-white">
                            <h1 class="modal-title fs-5">❌ Ocurrió un error</h1>
                        </div>
                        <div class="modal-body">
                            <p>Mail o contraseña incorrectos</p>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-dark ms-auto" data-bs-dismiss="modal">Entendido</button>
                            <div class="col-4"></div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </main>

</asp:Content>
