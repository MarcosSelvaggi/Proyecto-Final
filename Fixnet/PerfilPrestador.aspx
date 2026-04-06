<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PerfilPrestador.aspx.cs" Inherits="Fixnet.PerfilPrestador" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">
        <div class="form">
            <div class="col-8 py-2 justify_content_center">

                <h2>Configuración de tu perfil de prestador</h2>

                <!-- Descripción que quiera utilizar el Prestador -->
                <div class="mb-3">
                    <label class="form-label">Contanos un poco de vos</label>
                    <asp:TextBox CssClass="form-control" ID="txtDescripcion" runat="server"></asp:TextBox>
                </div>

                <!-- Servicios que presta o que va a prestar -->
                <div class="mb-3">
                    <label class="form-label">¿Qué servicios podés ofrecer?</label>

                    <asp:Repeater ID="rptServicios" runat="server">
                        <ItemTemplate>
                            <div class="mb-2">
                                <asp:CheckBox ID="chkServicio" runat="server" />
                                <asp:HiddenField ID="hfIdServicio" runat="server" Value='<%# Eval("IdServicio") %>' />
                                <%# Eval("Nombre") %>

                                <asp:TextBox ID="txtPrecio"
                                    runat="server"
                                    CssClass="form-control d-inline w-25 ms-2"
                                    placeholder="Precio/hora">
                                </asp:TextBox>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>

                <!-- Localidad/es en las que vaya a prestar el servicio -->
                <div class="mb-3">
                    <label class="form-label">Zona de trabajo</label>
                    <asp:DropDownList ID="ddlLocalidad" runat="server" CssClass="form-control">
                    </asp:DropDownList>
                </div>

                <!-- Un ejemplo para los términos y condiciones que vayamos a agregar (Estaría bueno) -->
                <div class="mb-3 form-check">
                    <asp:CheckBox ID="chkAcepto" runat="server" CssClass="form-check-input" Text="" />
                    <label class="form-check-label" for="<%= chkAcepto.ClientID %>">
                        Acepto los términos y condiciones
   
                    </label>
                </div>

                <!-- Botón -->
                <asp:Button ID="btnGuardarPrestador"
                    runat="server"
                    CssClass="btn btn-primary"
                    Text="Guardar"
                    OnClick="btnGuardarPrestador_Click" />

                <asp:Label ID="lblErrorPrestador"
                    runat="server"
                    CssClass="text-danger"
                    Visible="false">
                </asp:Label>

            </div>
        </div>
    </div>

</asp:Content>
