<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="Fixnet.Registro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/StyleRegistro.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="registro-container">
        <div class="registro-card">

            <h2 class="titulo">Crear cuenta</h2>

            <asp:Panel runat="server" DefaultButton="BtnRegistro">

                <!-- NOMBRE -->
                <div class="form-group floating">
                    <asp:TextBox ID="txtNombre" ClientIDMode="Static" runat="server" CssClass="form-control"></asp:TextBox>
                    <label>Nombre</label>
                    <span class="valid-icon">✔</span>
                    <span class="icon">👤</span>
                    <small class="error" id="errorNombre"></small>
                </div>

                <!-- APELLIDO -->
                <div class="form-group floating">
                    <asp:TextBox ID="txtApellido" ClientIDMode="Static" runat="server" CssClass="form-control"></asp:TextBox>
                    <label>Apellido</label>
                    <span class="valid-icon">✔</span>
                    <span class="icon">👤</span>
                    <small class="error" id="errorApellido"></small>
                </div>

                <!-- TEL -->
                <div class="form-group floating">
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" MaxLength="10" ClientIDMode="Static"></asp:TextBox>
                    <label>Teléfono (+54)</label>
                    <span class="valid-icon">✔</span>
                    <span class="icon">📱</span>
                    <small class="error" id="errorTelefono"></small>
                </div>

                <!-- EMAIL -->
                <div class="form-group floating">
                    <asp:TextBox ID="txtEmail" ClientIDMode="Static" runat="server" CssClass="form-control"></asp:TextBox>
                    <label>Email</label>
                    <span class="valid-icon">✔</span>
                    <span class="icon">📧</span>
                    <small class="error" id="errorEmail"></small>
                </div>

                <!-- PASSWORD -->
                <div class="form-group floating">
                    <asp:TextBox ID="txtPassword" ClientIDMode="Static" runat="server" CssClass="form-control" TextMode="Password"></asp:TextBox>
                    <label>Contraseña</label>
                    <span class="valid-icon">✔</span>
                    <span class="icon-password" onclick="togglePass('txtPassword')">👁</span>
                    <span class="icon">🔒</span>
                    <small class="error" id="errorPassword"></small>
                </div>

                <asp:Label ID="lblError" runat="server" CssClass="error-general" Visible="false"></asp:Label>

                <asp:Button ID="BtnRegistro" runat="server"
                    Text="Registrarse"
                    CssClass="btn-registro"
                    OnClick="BtnRegistro_Click" />


            </asp:Panel>
        </div>
    </div>

    <script src="scripts/Registro.js"></script>

</asp:Content>
