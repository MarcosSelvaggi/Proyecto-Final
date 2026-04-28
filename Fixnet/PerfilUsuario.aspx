<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master"
    AutoEventWireup="true" CodeBehind="PerfilUsuario.aspx.cs"
    Inherits="Fixnet.PerfilUsuario" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/StylePerfilUsuario.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="perfil-container">

        <!-- HEADER -->
        <div class="perfil-header">

            <div class="perfil-user">

                <!-- AVATAR -->
                <asp:Image ID="imgFotoPerfil" runat="server"
                    CssClass="avatar-foto"
                    Visible="false" />

                <div class="avatar" id="divIniciales" runat="server">
                    <asp:Label ID="lblIniciales" runat="server" />
                </div>

                <!-- INFO -->
                <div class="perfil-info">
                    <h2 class="nombre">
                        <asp:Label ID="lblNombreCompleto" runat="server" />
                    </h2>

                    <div class="subinfo">
                        <i class="fa fa-envelope"></i>
                        <asp:Label ID="lblEmail" runat="server" />
                        <span>-</span>
                        <i class="fa fa-phone"></i>
                        <asp:Label ID="lblTelefono" runat="server" />
                    </div>

                    <!-- ACCIONES -->
                    <div class="perfil-actions">
                        <asp:Button CssClass="btn-glass"
                            Text="Editar información"
                            ID="BtnModificarInformacionPersonal"
                            runat="server"
                            OnClick="ModificarInformacionPersonal_Click" />

                        <asp:Button CssClass="btn-glass"
                            Text="Cambiar contraseña"
                            ID="BtnModificarContraseña"
                            runat="server"
                            OnClick="ModificarContraseña_Click" />
                    </div>
                </div>

            </div>

        </div>

        <!-- GRID -->
        <div class="perfil-grid">

            <!-- CLIENTE -->
            <div class="perfil-card cliente">

                <div class="card-title">
                    <span><i class="bi bi-geo-alt-fill"></i> Perfil cliente</span>

                    <asp:Panel ID="pnlBadgeClienteOk" runat="server">
                        <span class="badge ok">Completo</span>
                    </asp:Panel>

                    <asp:Panel ID="pnlBadgeClienteVacio" runat="server">
                        <span class="badge warn">Incompleto</span>
                    </asp:Panel>
                </div>

                <div class="card-body">

                    <asp:Panel ID="pnlClienteDatos" runat="server">

                        <p>
                            <span>Provincia:</span>
                            <asp:Label ID="lblProvincia" runat="server" />
                        </p>
                        <p>
                            <span>Departamento:</span>
                            <asp:Label ID="lblDepartamento" runat="server" />
                        </p>
                        <p>
                            <span>Localidad:</span>
                            <asp:Label ID="lblLocalidad" runat="server" />
                        </p>
                        <p>
                            <span>Dirección:</span>
                            <asp:Label ID="lblDireccion" runat="server" />
                        </p>

                        <asp:Button ID="btnEditarCliente" runat="server"
                            Text="Editar"
                            CssClass="btn-primary"
                            OnClick="btnEditarCliente_Click" />

                    </asp:Panel>

                    <asp:Panel ID="pnlClienteVacio" runat="server">
                        <p>Completá tu perfil para acceder a servicios.</p>

                        <asp:Button ID="btnCrearCliente" runat="server"
                            Text="Completar"
                            CssClass="btn-primary"
                            OnClick="btnCrearCliente_Click" />
                    </asp:Panel>

                </div>
            </div>

            <!-- PRESTADOR -->
            <div class="perfil-card prestador">

                <div class="card-title">
                    <span><i class="bi bi-tools"></i> Perfil prestador</span>

                    <asp:Panel ID="pnlBadgePrestadorOk" runat="server">
                        <span class="badge ok">Activo</span>
                    </asp:Panel>

                    <asp:Panel ID="pnlBadgePrestadorVacio" runat="server">
                        <span class="badge warn">Inactivo</span>
                    </asp:Panel>
                </div>

                <div class="card-body">

                    <asp:Panel ID="pnlPrestadorDatos" runat="server">

                        <p>
                            <span>Descripción:</span>
                            <asp:Label ID="lblDescripcion" runat="server" />
                        </p>
                        <p>
                            <span>Zonas:</span>
                            <asp:Label ID="lblZonas" runat="server" />
                        </p>

                        <asp:Button ID="btnEditarPrestador" runat="server"
                            Text="Editar"
                            CssClass="btn-success"
                            OnClick="btnEditarPrestador_Click" />

                    </asp:Panel>

                    <asp:Panel ID="pnlPrestadorVacio" runat="server">
                        <p>Completá tu perfil para ofrecer servicios.</p>

                        <asp:Button ID="btnCrearPrestador" runat="server"
                            Text="Completar"
                            CssClass="btn-success"
                            OnClick="btnCrearPrestador_Click" />
                    </asp:Panel>

                </div>
            </div>

        </div>

    </div>
</asp:Content>
