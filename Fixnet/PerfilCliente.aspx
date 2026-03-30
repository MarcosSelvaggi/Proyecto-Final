<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PerfilCliente.aspx.cs" Inherits="Fixnet.PerfilCliente" Async="true" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Main content -->
    <div>
        <% Dominio.Usuario UsuarioLogeado = (Dominio.Usuario)Session["Usuario"];
            if (UsuarioLogeado.Cliente.DireccionCliente == "No ingresado")
            {
        %>
        <div class="container">
            <div class="row">
                <div class="mb-3">
                    <label for="ddlProvincia" class="form-label">Seleccionar Provincia</label>
                    <asp:DropDownList runat="server" ID="ddlProvincia" OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                </div>
                <div class="mb-3">
                    <label for="ddlDepartamento" class="form-label">Seleccionar Municipio</label>
                    <asp:DropDownList runat="server" ID="ddlDepartamento" OnSelectedIndexChanged="ddlDepartamento_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                </div>
                <div class="mb-3">
                    <label for="ddlLocalidad" class="form-label">Seleccionar Localidad</label>
                    <asp:DropDownList runat="server" ID="ddlLocalidad"></asp:DropDownList>
                </div>
                <div class="mb-3">
                    <label for="txtDireccion" class="form-label">Dirección</label>
                    <asp:TextBox runat="server" CssClass="form-control" ID="txtDireccion"></asp:TextBox>
                </div>
                <div class="col-12">
                    <div>
                        <asp:Button ID="btnActualizarInformacion" runat="server" Text="Actualizar información" CssClass="BtnActualizar" OnClick="btnActualizarInformacion_Click" />
                    </div>
                </div>
            </div>
        </div>

        <%
            }
            else
            {
                %>
        <h1>Ta todo joya</h1>
        <%
            }
        %>
    </div>
</asp:Content>
