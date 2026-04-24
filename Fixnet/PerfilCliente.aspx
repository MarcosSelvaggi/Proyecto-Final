<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PerfilCliente.aspx.cs" Inherits="Fixnet.PerfilCliente" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/StylePerfilCliente.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="perfil-container">

        <div class="perfil-card">

            <h2>Mi dirección</h2>

            <!-- FORM -->
            <asp:Panel ID="pnlFormulario" runat="server">

                <asp:Panel runat="server" DefaultButton="btnActualizarInformacion">

                    <!-- PROVINCIA -->
                    <label>Provincia</label>
                    <asp:DropDownList runat="server" ID="ddlProvincia"
                        CssClass="form-control"
                        OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>

                    <!-- Departamento -->
                    <label>Departamento</label>
                    <asp:DropDownList runat="server" ID="ddlDepartamento"
                        CssClass="form-control"
                        OnSelectedIndexChanged="ddlDepartamento_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>

                    <!-- LOCALIDAD -->
                    <label>Localidad</label>
                    <asp:DropDownList runat="server" ID="ddlLocalidad"
                        CssClass="form-control"
                        OnSelectedIndexChanged="ddlLocalidad_SelectedIndexChanged"
                        AutoPostBack="true">
                    </asp:DropDownList>

                    <!-- DIRECCION -->
                    <label>Dirección</label>
                    <asp:TextBox runat="server"
                        CssClass="form-control"
                        ID="txtDireccion">
                    </asp:TextBox>

                    <!-- BOTON -->
                    <asp:Button ID="btnActualizarInformacion" runat="server"
                        Text="Actualizar información"
                        CssClass="BtnActualizar"
                        OnClick="btnActualizarInformacion_Click" />

                    <!-- ERROR -->
                    <asp:Label ID="lblErrorDireccion" runat="server"
                        CssClass="text-danger"
                        Visible="false">
                    </asp:Label>

                </asp:Panel>

            </asp:Panel>

            <!-- OK -->
            <asp:Panel ID="pnlOk" runat="server">
                <div class="perfil-ok">
                    ✔ Datos cargados correctamente. Podés modificarlos cuando quieras.
                </div>
            </asp:Panel>

        </div>

    </div>

</asp:Content>
