<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PerfilPrestador.aspx.cs" Inherits="Fixnet.PerfilPrestador" Async="true" %>

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
                    <h4 class="form-label text-start fw-bold">Zonas de trabajo</h4>
                    <label class="form-label">Seleccionar Provincia</label>
                    <asp:DropDownList runat="server" ID="ddlProvincia"
                        OnSelectedIndexChanged="ddlProvincia_SelectedIndexChanged"
                        AutoPostBack="true" CssClass="form-control">
                    </asp:DropDownList>
                </div>

                <div class="mb-3">
                    <label class="form-label">Seleccionar Municipio</label>
                    <asp:DropDownList runat="server" ID="ddlDepartamento"
                        OnSelectedIndexChanged="ddlDepartamento_SelectedIndexChanged"
                        AutoPostBack="true" CssClass="form-control">
                    </asp:DropDownList>
                </div>

                <div class="mb-3">
                    
                    <label class="form-label">Seleccionar Localidad</label>
                    <asp:DropDownList runat="server" ID="ddlLocalidad" 
                        OnSelectedIndexChanged="ddlLocalidad_SelectedIndexChanged"
                        CssClass="form-control" AutoPostBack="true">
                    </asp:DropDownList>
                </div>

                
                <asp:Repeater runat="server" ID="rptLocalidades"> 
                    <ItemTemplate>
                            <div class="mb-3">
                                <div class="row">
                                    <div class="col-6 justify-center">
                                        <%# Eval("Nombre") %>
                                    </div>
                                    <div class="col-6 justify-center">
                                        <asp:Button Text="Eliminar" 
                                            CssClass="btn btn-dark" 
                                            ID="Button" 
                                            runat="server" 
                                            OnCommand="EliminarLocalidades"
                                            CommandName="EliminarLocalidad" 
                                            CommandArgument='<%# Eval("Id") %>'/>
                                    </div>
                            </div>
                        
                        </div>
                    </ItemTemplate>
                </asp:Repeater>

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
