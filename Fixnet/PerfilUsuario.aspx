<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master"
AutoEventWireup="true" CodeBehind="PerfilUsuario.aspx.cs"
Inherits="Fixnet.PerfilUsuario" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<style>
    .avatar {
        width: 70px;
        height: 70px;
        font-weight: bold;
        font-size: 22px;
    }
</style>

<div class="container py-4" style="max-width:900px;">

    <!-- HERO -->
    <div class="card text-white bg-primary mb-4 shadow-sm">
        <div class="card-body d-flex align-items-center gap-3">
            
            <div class="avatar rounded-circle bg-success d-flex align-items-center justify-content-center border border-3 border-white">
                <asp:Label ID="lblIniciales" runat="server" />
            </div>

            <div>
                <h5 class="mb-1">
                    <asp:Label ID="lblNombreCompleto" runat="server" />
                </h5>
                <small>
                    <asp:Label ID="lblEmail" runat="server" />
                    ·
                    <asp:Label ID="lblTelefono" runat="server" />
                </small>
            </div>

        </div>
    </div>

    <div class="row g-3">

        <!-- CLIENTE -->
        <div class="col-md-6">
            <div class="card border-primary shadow-sm h-100">

                <div class="card-header bg-primary text-white d-flex justify-content-between">
                    <span>📍 Perfil cliente</span>

                    <asp:Panel ID="pnlBadgeClienteOk" runat="server">
                        <span class="badge bg-success">Completo</span>
                    </asp:Panel>

                    <asp:Panel ID="pnlBadgeClienteVacio" runat="server">
                        <span class="badge bg-info text-dark">Sin completar</span>
                    </asp:Panel>
                </div>

                <div class="card-body">

                    <asp:Panel ID="pnlClienteDatos" runat="server">
                        <p><strong>Provincia:</strong> <asp:Label ID="lblProvincia" runat="server" /></p>
                        <p><strong>Departamento:</strong> <asp:Label ID="lblDepartamento" runat="server" /></p>
                        <p><strong>Localidad:</strong> <asp:Label ID="lblLocalidad" runat="server" /></p>
                        <p><strong>Dirección:</strong> <asp:Label ID="lblDireccion" runat="server" /></p>

                        <asp:Button ID="btnEditarCliente" runat="server" Text="Editar"
                            CssClass="btn btn-primary btn-sm"
                            OnClick="btnEditarCliente_Click" />
                    </asp:Panel>

                    <asp:Panel ID="pnlClienteVacio" runat="server">
                        <p class="text-primary mb-1">Todavía no completaste tu perfil.</p>
                        <small class="text-muted">Completalo para acceder a los servicios.</small>

                        <div class="mt-3">
                            <asp:Button ID="btnCrearCliente" runat="server" Text="Completar perfil"
                                CssClass="btn btn-primary btn-sm"
                                OnClick="btnCrearCliente_Click" />
                        </div>
                    </asp:Panel>

                </div>
            </div>
        </div>

        <!-- PRESTADOR -->
        <div class="col-md-6">
            <div class="card border-success shadow-sm h-100">

                <div class="card-header bg-success text-white d-flex justify-content-between">
                    <span>🔧 Perfil prestador</span>

                    <asp:Panel ID="pnlBadgePrestadorOk" runat="server">
                        <span class="badge bg-light text-success">Activo</span>
                    </asp:Panel>

                    <asp:Panel ID="pnlBadgePrestadorVacio" runat="server">
                        <span class="badge bg-warning text-dark">Sin completar</span>
                    </asp:Panel>
                </div>

                <div class="card-body">

                    <asp:Panel ID="pnlPrestadorDatos" runat="server">
                        <p><strong>Descripción:</strong> <asp:Label ID="lblDescripcion" runat="server" /></p>
                        <p><strong>Zonas:</strong> <asp:Label ID="lblZonas" runat="server" /></p>

                        <asp:Button ID="btnEditarPrestador" runat="server" Text="Editar"
                            CssClass="btn btn-success btn-sm"
                            OnClick="btnEditarPrestador_Click" />
                    </asp:Panel>

                    <asp:Panel ID="pnlPrestadorVacio" runat="server">
                        <p class="text-success mb-1">Todavía no completaste tu perfil.</p>
                        <small class="text-muted">Completalo para ofrecer servicios.</small>

                        <div class="mt-3">
                            <asp:Button ID="btnCrearPrestador" runat="server" Text="Completar perfil"
                                CssClass="btn btn-success btn-sm"
                                OnClick="btnCrearPrestador_Click" />
                        </div>
                    </asp:Panel>

                </div>
            </div>
        </div>

    </div>
</div>

</asp:Content>