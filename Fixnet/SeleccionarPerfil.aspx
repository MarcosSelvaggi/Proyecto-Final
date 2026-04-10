<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master"
AutoEventWireup="true" CodeBehind="SeleccionarPerfil.aspx.cs"
Inherits="Fixnet.SeleccionarPerfil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/StylePerfiles.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="container text-center mt-5">

    <h2>Seleccioná tu perfil</h2>

    <div class="row mt-4">

        <div class="col-md-6">
            <a href="/PerfilCliente.aspx">
                <img src="assets/img-usuario-1.png" class="img-fluid" />
                <h4>Cliente</h4>
            </a>
        </div>

        <div class="col-md-6">
            <a href="/PerfilPrestador.aspx">
                <img src="assets/img-proveedor-1.png" class="img-fluid" />
                <h4>Prestador</h4>
            </a>
        </div>

    </div>

</div>

</asp:Content>