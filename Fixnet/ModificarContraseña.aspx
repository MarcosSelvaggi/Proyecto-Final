<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ModificarContraseña.aspx.cs" Inherits="Fixnet.CambiarContraseña" %>

<asp:Content ID="Content2" ContentPlaceHolderID="head" runat="server">
    <link href="styles/StyleModificarContrasenia.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="MainContent" runat="server">

    <div class="recovery-container">

        <div class="recovery-card">

            <h2 class="titulo">
                <i class="bi bi-shield-lock"></i>
                Modificar contraseña
            </h2>

            <asp:Panel runat="server" DefaultButton="BtnModificar">

                <!-- INPUT PASSWORD -->
                <div class="field">

                    <label>Nueva contraseña</label>

                    <div class="input-wrapper">
                        <asp:TextBox ID="txtPassword"
                            runat="server"
                            TextMode="Password"
                            ClientIDMode="Static"
                            CssClass="input" />

                        <div class="icons">
                            <i class="bi bi-key icon"></i>
                            <i class="bi bi-check2-circle valid-icon" id="checkPassword"></i>
                        </div>
                    </div>

                    <small id="errorPassword" class="error"></small>

                </div>

                <!-- ERROR BACK -->
                <asp:Label ID="lblError"
                    runat="server"
                    CssClass="error"
                    Visible="false" />

                <!-- BOTONES -->
                <div class="actions">

                    <asp:Button ID="BtnModificar"
                        runat="server"
                        Text="Modificar"
                        CssClass="btn-primary"
                        OnClick="ModificarContraseña_Click" />

                    <asp:Button ID="BtnVolver"
                        runat="server"
                        Text="Volver"
                        CssClass="btn-secondary"
                        OnClick="BtnVolver_Click" />

                </div>

            </asp:Panel>

        </div>

    </div>

    <div class="modal fade" id="successModal" tabindex="-1">
        <div class="modal-dialog modal-dialog-centered">

            <div class="modal-content glass-modal">

                <!-- HEADER -->
                <div class="modal-header modal-header-glass">

                    <div class="icon-box">
                        <i class="bi bi-check2-circle"></i>
                    </div>

                    <h5 class="modal-title modal-title-glass">Contraseña actualizada
                    </h5>

                </div>

                <!-- BODY -->
                <div class="modal-body modal-body-glass">
                    <p>Tu contraseña fue cambiada correctamente.</p>
                    <p class="muted">Ya podés iniciar sesión con la nueva contraseña.</p>
                </div>

            </div>

        </div>
    </div>
    <script src="scripts/RecuperarContrasenia.js"></script>
</asp:Content>
