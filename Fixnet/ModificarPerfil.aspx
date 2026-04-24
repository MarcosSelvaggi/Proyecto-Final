<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ModificarPerfil.aspx.cs" Inherits="Fixnet.ModificarPerfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/StyleRegistro.css" rel="stylesheet" />
     <script src="<%= ResolveUrl("~/Scripts/FotoPerfilPreview.js") %>"></script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container2">
        <div style="max-width: 700px; width: 100%;">
            <h2 class="text-center mb-4">
                <i class="bi bi-caret-right-fill"></i>Modificar Información Personal<i class="bi bi-caret-left-fill"></i>
            </h2>

            <asp:Panel runat="server" DefaultButton="BtnModificar">
                <div class="row g-3">

                    <!-- FOTO DE PERFIL (opcional) -->
                    <div class="col-12 text-center mb-2">
                        <div class="mb-2">
                            <!-- Muestra la foto actual o las iniciales si no tiene -->
                            <div id="divFotoActual" style="display:inline-block; position:relative;">
                                <asp:Image ID="imgFotoActual" runat="server"
                                    Style="width:100px; height:100px; border-radius:50%; object-fit:cover; border:3px solid #0d6efd; display:none;"
                                    AlternateText="Foto de perfil" />
                                <asp:Label ID="lblIniciales" runat="server"
                                    Style="width:100px; height:100px; border-radius:50%; background:#0d6efd; color:white;
                                           font-size:2rem; font-weight:bold; display:inline-flex;
                                           align-items:center; justify-content:center;" />
                            </div>
                        </div>

                        <label class="form-label d-block text-muted" style="font-size:0.9rem;">
                            Foto de perfil <span class="text-secondary">(opcional)</span>
                        </label>

                        <!-- Preview antes de guardar -->
                        <img id="previewFoto" src="#" alt="Preview"
                             style="display:none; width:100px; height:100px; border-radius:50%;
                                    object-fit:cover; border:3px solid #198754; margin-bottom:8px;" />

                        <asp:FileUpload ID="fuFoto" runat="server" CssClass="form-control form-control-sm"
                            Style="max-width:300px; margin:0 auto;"
                            onchange="previewImagen(this)" />

                        <small class="text-muted d-block mt-1">JPG, PNG o GIF · máx. 2 MB</small>

                        <asp:Label ID="lblErrorFoto" runat="server" CssClass="text-danger d-block mt-1" Visible="false" />
                    </div>

                    <div class="col-md-6">
                        <label class="form-label">Nombre</label>
                        <asp:TextBox ID="txtNombre" CssClass="form-control" runat="server" />
                    </div>

                    <div class="col-md-6">
                        <label class="form-label">Apellido</label>
                        <asp:TextBox ID="txtApellido" CssClass="form-control" runat="server" />
                    </div>

                    <div class="col-md-12">
                        <label class="form-label">Teléfono</label>
                        <div class="input-group">
                            <span class="input-group-text bg-dark text-white">+54</span>
                            <asp:TextBox ID="txtTeléfono" CssClass="form-control" runat="server"
                                onKeypress="return soloNumeros(event)" />
                        </div>
                    </div>

                    <div class="col-md-12">
                        <label class="form-label">Email</label>
                        <asp:TextBox ID="txtEmail" CssClass="form-control" runat="server" TextMode="Email" />
                    </div>

                    <div class="col-12 text-center mt-4">
                        <div class="row">
                            <div class="col-2"></div>
                            <div class="col-4">
                                <div class="btnEnviar">
                                    <asp:Button ID="BtnModificar" runat="server" Text="Modificar"
                                        CssClass="BtnEnviar" OnClick="BtnModificarPerfil_Click" />
                                </div>
                            </div>
                            <div class="col-4">
                                <div class="btnEnviar">
                                    <asp:Button ID="BtnVolver" runat="server" Text="Volver"
                                        CssClass="btn btn-outline-dark" OnClick="BtnVolver_Click" />
                                </div>
                            </div>
                        </div>

                        <asp:Label ID="lblError" runat="server" ForeColor="Red"
                            Font-Bold="true" Visible="false" CssClass="mt-3" />
                    </div>

                </div>
            </asp:Panel>
        </div>
    </div>

    <!-- MODAL ÉXITO -->
    <div class="modal fade" id="ModificarUsuarioModal" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content">
                <div class="modal-header bg-success text-white">
                    <h1 class="modal-title fs-5">Información actualizada correctamente</h1>
                </div>
                <div class="modal-body">
                    <p>Se ha actualizado correctamente su información personal.</p>
                    <p>En unos segundos serás redirigido a tu perfil.</p>
                </div>
                <div class="modal-footer">
                    <a href="/PerfilUsuario.aspx" class="btn btn-dark ms-auto">Ir ahora</a>
                </div>
            </div>
        </div>
    </div>


</asp:Content>