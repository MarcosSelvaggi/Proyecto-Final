<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PerfilCliente.aspx.cs" Inherits="Fixnet.PerfilCliente" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <!-- FORM -->
    <asp:Panel ID="pnlFormulario" runat="server">
        <div class="container">
            <asp:Panel runat="server" DefaultButton="btnActualizarInformacion">
                <div class="row">
                    <div class="mb-3">
                        <label class="form-label">Seleccionar Provincia</label>
                        <asp:DropDownList runat="server" ID="ddlProvincia"
                            OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </div>

                    <div class="mb-3">
                        <label class="form-label">Seleccionar Municipio</label>
                        <asp:DropDownList runat="server" ID="ddlDepartamento"
                            OnSelectedIndexChanged="ddlDepartamento_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </div>

                    <div class="mb-3">
                        <label class="form-label">Seleccionar Localidad</label>
                        <asp:DropDownList runat="server" ID="ddlLocalidad"
                            OnSelectedIndexChanged="ddlLocalidad_SelectedIndexChanged"
                            AutoPostBack="true">
                        </asp:DropDownList>
                    </div>

                    <div class="mb-3">
                        <label class="form-label">Dirección</label>
                        <asp:TextBox runat="server" CssClass="form-control" ID="txtDireccion">
                        </asp:TextBox>
                    </div>

                    <div class="col-12">
                        <asp:Button ID="btnActualizarInformacion" runat="server"
                            Text="Actualizar información"
                            CssClass="BtnActualizar"
                            OnClick="btnActualizarInformacion_Click" />

                        <asp:Label ID="lblErrorDireccion" runat="server"
                            CssClass="text-danger"
                            Visible="false">
                        </asp:Label>
                    </div>
                </div>
            </asp:Panel>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlOk" runat="server">
        <div class="container">
            <h5 class="text-success mt-3">Datos cargados correctamente. Podés modificarlos cuando quieras.</h5>
        </div>
    </asp:Panel>

</asp:Content>
