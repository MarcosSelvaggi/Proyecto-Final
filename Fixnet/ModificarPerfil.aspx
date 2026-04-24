<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ModificarPerfil.aspx.cs" Inherits="Fixnet.ModificarPerfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/StyleModificarPerfil.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.3/font/bootstrap-icons.min.css">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="recovery-container">

        <div class="recovery-card">

            <h2 class="titulo">
                <i class="bi bi-person-gear"></i>
                Modificar Información Personal
            </h2>

            <asp:Panel runat="server" DefaultButton="BtnModificar">

                <!-- FOTO -->
                <div class="field text-center">

                    <label>Foto de perfil</label>

                    <!-- CONTENEDOR VISUAL DEL AVATAR -->
                    <div class="avatar-wrapper">

                        <asp:Image ID="imgFotoActual" runat="server"
                            CssClass="avatar-foto"
                            AlternateText="Foto de perfil"
                            Visible="false" />

                        <asp:Panel ID="divInicialesWrapper" runat="server" CssClass="avatar-foto">
                            <asp:Label ID="lblIniciales" runat="server" />
                        </asp:Panel>

                        <img id="previewFoto"
                            class="avatar-foto"
                            style="display: none;" />

                    </div>

                    <!-- INPUT -->
                    <asp:FileUpload ID="fuFoto" runat="server"
                        CssClass="input"
                        onchange="previewImagen(this)" />

                    <asp:Label ID="lblErrorFoto" runat="server"
                        CssClass="error"
                        Visible="false" />

                </div>

                <!-- NOMBRE -->
                <div class="field">
                    <label>Nombre</label>
                    <asp:TextBox ID="txtNombre" runat="server" CssClass="input" ClientIDMode="Static" />
                    <small id="errorNombre" class="error"></small>
                </div>

                <!-- APELLIDO -->
                <div class="field">
                    <label>Apellido</label>
                    <asp:TextBox ID="txtApellido" runat="server" CssClass="input" ClientIDMode="Static" />
                    <small id="errorApellido" class="error"></small>
                </div>

                <!-- TELEFONO -->
                <div class="field">
                    <label>Teléfono</label>
                    <asp:TextBox ID="txtTelefono" runat="server" CssClass="input" ClientIDMode="Static" MaxLength="10" TextMode="Phone" />
                    <small id="errorTelefono" class="error"></small>
                </div>

                <!-- EMAIL -->
                <div class="field">
                    <label>Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="input" TextMode="Email" ClientIDMode="Static" />
                    <small id="errorEmail" class="error"></small>
                </div>

                <!-- BOTONES -->
                <div class="actions mt-3">

                    <asp:Button ID="BtnModificar" runat="server" ClientIDMode="Static"
                        Text="Guardar cambios"
                        CssClass="btn-primary"
                        OnClick="BtnModificarPerfil_Click" />

                    <asp:Button ID="BtnVolver" runat="server"
                        Text="Volver"
                        CssClass="btn-secondary"
                        OnClick="BtnVolver_Click" />

                </div>

                <asp:Label ID="lblError" runat="server"
                    CssClass="error d-block mt-2"
                    Visible="false" />

            </asp:Panel>

        </div>
    </div>

    <!-- MODAL -->
    <div class="modal fade" id="successModal" tabindex="-1">
        <div class="modal-dialog modal-dialog-centered">

            <div class="modal-content glass-modal">

                <div class="modal-header modal-header-glass">

                    <div class="icon-box">
                        <i class="bi bi-check2-circle"></i>
                    </div>

                    <h5 class="modal-title modal-title-glass">Perfil actualizado</h5>

                </div>

                <div class="modal-body modal-body-glass">
                    <p>Tu información fue actualizada correctamente.</p>
                    <p class="muted">Serás redirigido al perfil...</p>
                </div>

            </div>

        </div>
    </div>

    <script src="scripts/ModificarPerfil.js"></script>
</asp:Content>
