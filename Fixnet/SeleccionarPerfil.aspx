<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="SeleccionarPerfil.aspx.cs" Inherits="Fixnet.SeleccionarPerfil" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/StylePerfiles.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="container">
        <div class="justify-content-center py-2">
            <h1>Selecciona tu perfil</h1>
        </div>
        <div class="row">
            <div class="col-xl-6 col-md-12">
                <div class="imgSeleccionarPerfil">
                    <a href="/PerfilPrestador.aspx">
                        <img src="assets/img-proveedor-1.png" class="img-fluid" alt="Imagen proveedor" />
                    </a>
                </div>
                <div class="justify-content-center py-2">
                    <h2>Perfil proveedor</h2>
                </div>
            </div>
            <div class="col-xl-6 col-md-12">
                <div class="imgSeleccionarPerfil">
                    <a href="/PerfilCliente.aspx">
                        <img src="assets/img-usuario-1.png" class="img-fluid"  alt="Imagen cliente" />
                    </a>
                </div>
                <div class="justify-content-center py-2">
                    <h2>Perfil cliente</h2>
                </div>
            </div>
            
        </div>
    </div>

</asp:Content>
