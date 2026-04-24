<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master"
    AutoEventWireup="true" CodeBehind="SeleccionarPerfil.aspx.cs"
    Inherits="Fixnet.SeleccionarPerfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/StyleSeleccionarPerfil.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container text-center mt-5">

        <h2>Seleccioná tu perfil</h2>

        <div class="perfil-selector">

            <a href="/PerfilCliente.aspx" class="perfil-card">
                <img src="assets/img-usuario-1.png" />
                <h4>Cliente</h4>
            </a>

            <a href="/PerfilPrestador.aspx" class="perfil-card">
                <img src="assets/img-proveedor-1.png" />
                <h4>Prestador</h4>
            </a>

        </div>

    </div>


</asp:Content>
