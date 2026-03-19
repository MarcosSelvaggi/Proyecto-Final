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
                    <div class="input-group">
                        <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" TextMode="Email"></asp:TextBox>
                    </div>
                </div>

                <div class="col-md-12">
                    <label class="form-label">Contraseña</label>
                    <asp:TextBox ID="txtPassword" CssClass="form-control" runat="server" TextMode="Password"></asp:TextBox>
                </div>

                <div class="col-12 text-center mt-4">
                    <div class="btnEnviar">
                        <asp:Button ID="btnRegistro" runat="server" Text="Registrarse" CssClass="BtnEnviar" OnClick="BtnRegistro_Click"/>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal para manejar los problemas con el registro-->
    <div class="modal fade" id="problemaAlRegistrarseModal" tabindex="-1" aria-labelledby="problemaAlRegistrarseModal" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-dialog" style="min-width: 400px; width: 90%">
                <div class="modal-content">
                    <div class="modal-header bg-danger text-white">
                        <h1 class="modal-title fs-5" id="problemaAlRegistrarseModalH1" runat="server">❌ Ocurrió un error</h1>
                    </div>
                    <div class="modal-body">
                        <p id="pDatosErroneosModal" runat="server"></p>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-dark ms-auto" data-bs-dismiss="modal">Entendido</button>
                        <div class="col-4"></div>
                        <!--Horrible, pero funciona-->
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal para manejar el registro exitoso-->
    <div class="modal fade" id="usuarioRegistradoModal" tabindex="-1" aria-labelledby="usuarioRegistradoModal" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-dialog" style="min-width: 400px; width: 90%">
                <div class="modal-content">
                    <div class="modal-header bg-success text-white">
                        <h1 class="modal-title fs-5" runat="server">Registro exitoso</h1>
                    </div>
                    <div class="modal-body">
                        <p>Se ha registrado exitosamente en la plataforma.</p>
                        <p>En unos segundos serás redirigido a tu perfil.</p>
                    </div>
                    <div class="modal-footer">
                        <a href="/Perfil.aspx" class="btn btn-dark ms-auto">Ir ahora</a>
                        <div class="col-4"></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
