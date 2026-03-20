<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Registro.aspx.cs" Inherits="Fixnet.Registro" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/StyleRegistro.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container2">
        <div style="max-width: 700px; width: 100%;">
            <h2 class="text-center mb-4"><i class="bi bi-caret-right-fill"></i>Registro <i class="bi bi-caret-left-fill"></i></h2>

            <div class="row g-3">

                <div class="col-md-6">
                    <label class="form-label">Nombre</label>
                    <asp:TextBox ID="txtNombre" CssClass="form-control" runat="server"></asp:TextBox>
                </div>

                <div class="col-md-6">
                    <label class="form-label">Apellido</label>
                    <asp:TextBox ID="txtApellido" CssClass="form-control" runat="server"></asp:TextBox>
                </div>

                <div class="col-md-12">
                    <label class="form-label">Teléfono</label>
                    <div class="input-group">
                        <span class="input-group-text bg-dark text-white">+54</span>
                        <asp:TextBox ID="txtTeléfono" CssClass="form-control" runat="server" onKeypress="return soloNumeros(event)"></asp:TextBox>
                    </div>
                </div>

                <div class="col-md-12">
                    <label class="form-label">Email</label>
                    <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" TextMode="Email"></asp:TextBox>
                </div>

                <div class="col-md-12">
                    <label class="form-label">Contraseña</label>
                    <asp:TextBox ID="txtPassword" CssClass="form-control" runat="server" TextMode="Password"></asp:TextBox>
                </div>

                <div class="col-12 text-center mt-4">

                    <div class="btnEnviar">
                        <asp:Button ID="btnRegistro" runat="server" Text="Registrarse" CssClass="BtnEnviar" OnClick="BtnRegistro_Click"/>
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
        </div>
    </div>

</asp:Content>