<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PerfilPrestador.aspx.cs" Inherits="Fixnet.PerfilPrestador" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<asp:ScriptManager runat="server" />
    <div class="container">
        <asp:Panel runat="server" DefaultButton="btnGuardarPrestador">
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
                <asp:UpdatePanel runat="server">
                    <ContentTemplate>
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
                                                CommandArgument='<%# Eval("Id") %>' />
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        <label id="LblErrorLocalidades" visible="false" runat="server"></label>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <div class="container-fluid">
                    <div class="row">
                    <div class="col-12">
                        <h3>Seleccioná los días disponibles</h3>
                    </div>
                        <style>
                            .aDias{
                                display:flex;
                            }
                        </style>
                    
                    <div class="mb-3">
                        <asp:UpdatePanel runat="server">
                            <ContentTemplate>
                                <div>
                                    <asp:CheckBox runat="server" Text="Domingo" ID="CbxDomingo" OnCheckedChanged="CbxChecked" AutoPostBack="true" />
                                    <div id="HorariosDomingos" runat="server" visible="false">
                                        <label class="aDias" for="HorarioInicioDomingo">Desde:</label>
                                        <asp:TextBox runat="server" ID="HorarioInicioDomingo" Text="0" />
                                        <label class="aDias" for="HorarioFinDomingo">Hasta:</label>
                                        <asp:TextBox runat="server" ID="HorarioFinDomingo" Text="0" />
                                    </div>
                                </div>
                                <div>
                                    <asp:CheckBox runat="server" Text="Lunes" ID="CbxLunes" OnCheckedChanged="CbxChecked" AutoPostBack="true" />
                                    <div id="HorariosLunes" runat="server" visible="false">
                                        <label class="aDias" for="HorarioInicioLunes">Desde:</label>
                                        <asp:TextBox runat="server" ID="HorarioInicioLunes" Text="0" />
                                        <label class="aDias" for="HorarioFinLunes">Hasta:</label>
                                        <asp:TextBox runat="server" ID="HorarioFinLunes"  Text="0"/>
                                    </div>
                                </div>
                                <div>
                                    <asp:CheckBox runat="server" Text="Martes" ID="CbxMartes" OnCheckedChanged="CbxChecked" AutoPostBack="true" />
                                    <div id="HorariosMartes" runat="server" visible="false">
                                        <label class="aDias" for="HorarioInicioMartes">Desde:</label>
                                        <asp:TextBox runat="server" ID="HorarioInicioMartes" Text="0" />
                                        <label class="aDias" for="HorarioFinMartes">Hasta:</label>
                                        <asp:TextBox runat="server" ID="HorarioFinMartes" Text="0"/>
                                    </div>
                                </div>
                                <div>
                                    <asp:CheckBox runat="server" Text="Miércoles" ID="CbxMiercoles" OnCheckedChanged="CbxChecked" AutoPostBack="true" />
                                    <div id="HorariosMiercoles" runat="server" visible="false">
                                        <label class="aDias" for="HorarioInicioMiercoles">Desde:</label>
                                        <asp:TextBox runat="server" ID="HorarioInicioMiercoles" Text="0"/>
                                        <label class="aDias" for="HorarioFinMiercoles">Hasta:</label>
                                        <asp:TextBox runat="server" ID="HorarioFinMiercoles" Text="0"/>
                                    </div>
                                </div>
                                <div>
                                    <asp:CheckBox runat="server" Text="Jueves" ID="CbxJueves" OnCheckedChanged="CbxChecked" AutoPostBack="true" />
                                    <div id="HorariosJueves" runat="server" visible="false">
                                        <label class="aDias" for="HorarioInicioJueves">Desde:</label>
                                        <asp:TextBox runat="server" ID="HorarioInicioJueves" Text="0"/>
                                        <label class="aDias" for="HorarioFinJueves">Hasta:</label>
                                        <asp:TextBox runat="server" ID="HorarioFinJueves" Text="0" />
                                    </div>
                                </div>
                                <div>
                                    <asp:CheckBox runat="server" Text="Viernes" ID="CbxViernes" OnCheckedChanged="CbxChecked" AutoPostBack="true" />
                                    <div id="HorariosViernes" runat="server" visible="false">
                                        <label class="aDias" for="HorarioInicioViernes">Desde:</label>
                                        <asp:TextBox runat="server" ID="HorarioInicioViernes" Text="0"/>
                                        <label class="aDias" for="HorarioFinViernes">Hasta:</label>
                                        <asp:TextBox runat="server" ID="HorarioFinViernes" Text="0"/>
                                    </div>
                                </div>
                                <div>
                                    <asp:CheckBox runat="server" Text="Sábados" ID="CbxSabados" OnCheckedChanged="CbxChecked" AutoPostBack="true" />
                                    <div id="HorariosSabados" runat="server" visible="false">
                                        <label class="aDias" for="HorarioInicioSabados">Desde:</label>
                                        <asp:TextBox runat="server" ID="HorarioInicioSabados" Text="0"/>
                                        <label class="aDias" for="HorarioFinSabados">Hasta:</label>
                                        <asp:TextBox runat="server" ID="HorarioFinSabados" Text="0"/>
                                    </div>
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                        <label id="LblErrorHorariosPrestador" visible="false" runat="server" style="color:red"></label>
                </div>
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
            </asp:Panel>
    </div>

</asp:Content>
