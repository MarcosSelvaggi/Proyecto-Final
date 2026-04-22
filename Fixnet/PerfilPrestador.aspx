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

                    <!-- Servicios que presta o que va a prestar-->
                    <div class="mb-3">
                        <label class="form-label">¿Qué servicios podés ofrecer?</label>

                        <div class="row">
                            <asp:Repeater ID="rptServicios" runat="server">
                                <ItemTemplate>
                                    <div class="col-md-6 mb-2">
                                        <div class="d-flex align-items-center justify-content-between p-2 border rounded">

                                            <div class="m-0">
                                                <asp:CheckBox ID="chkServicio" runat="server" CssClass="" />
                                                <label class="form-check-label ms-2">
                                                    <%# Eval("Nombre") %>
                                                </label>
                                            </div>

                                            <!-- Precio -->
                                            <div class="d-flex align-items-center">
                                                <span class="me-2 text-muted">$</span>
                                                <asp:TextBox ID="txtPrecio"
                                                    runat="server"
                                                    CssClass="form-control form-control-sm"
                                                    Style="width: 90px"
                                                    placeholder="Precio/h">
                                                </asp:TextBox>
                                            </div>

                                            <asp:HiddenField ID="hfIdServicio" runat="server" Value='<%# Eval("IdServicio") %>' />
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
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
                                .aDias {
                                    display: flex;
                                }
                            </style>

                            <div class="mb-3">

                                <asp:UpdatePanel runat="server">
                                    <ContentTemplate>

                                        <!-- DOMINGO -->
                                        <div class="d-flex align-items-center mb-2 border-bottom pb-2">
                                            <div style="width: 150px;">
                                                <asp:CheckBox runat="server" Text="Domingo" ID="CbxDomingo"
                                                    OnCheckedChanged="CbxChecked" AutoPostBack="true" />
                                            </div>
                                            <div id="HorariosDomingos" runat="server" visible="false"
                                                class="d-flex align-items-center gap-2">
                                                <span>Desde</span>
                                                <asp:TextBox runat="server" ID="HorarioInicioDomingo"
                                                    CssClass="form-control text-center" Style="width: 80px"
                                                    TextMode="Number" min="0" max="23" placeholder="8" />
                                                <span>Hasta</span>
                                                <asp:TextBox runat="server" ID="HorarioFinDomingo"
                                                    CssClass="form-control text-center" Style="width: 80px"
                                                    TextMode="Number" min="0" max="23" placeholder="18" />
                                            </div>
                                        </div>

                                        <!-- LUNES -->
                                        <div class="d-flex align-items-center mb-2 border-bottom pb-2">
                                            <div style="width: 150px;">
                                                <asp:CheckBox runat="server" Text="Lunes" ID="CbxLunes"
                                                    OnCheckedChanged="CbxChecked" AutoPostBack="true" />
                                            </div>
                                            <div id="HorariosLunes" runat="server" visible="false"
                                                class="d-flex align-items-center gap-2">
                                                <span>Desde</span>
                                                <asp:TextBox runat="server" ID="HorarioInicioLunes"
                                                    CssClass="form-control text-center" Style="width: 80px"
                                                    TextMode="Number" min="0" max="23" placeholder="8" />
                                                <span>Hasta</span>
                                                <asp:TextBox runat="server" ID="HorarioFinLunes"
                                                    CssClass="form-control text-center" Style="width: 80px"
                                                    TextMode="Number" min="0" max="23" placeholder="18" />
                                            </div>
                                        </div>

                                        <!-- MARTES -->
                                        <div class="d-flex align-items-center mb-2 border-bottom pb-2">
                                            <div style="width: 150px;">
                                                <asp:CheckBox runat="server" Text="Martes" ID="CbxMartes"
                                                    OnCheckedChanged="CbxChecked" AutoPostBack="true" />
                                            </div>
                                            <div id="HorariosMartes" runat="server" visible="false"
                                                class="d-flex align-items-center gap-2">
                                                <span>Desde</span>
                                                <asp:TextBox runat="server" ID="HorarioInicioMartes"
                                                    CssClass="form-control text-center" Style="width: 80px"
                                                    TextMode="Number" min="0" max="23" placeholder="8" />
                                                <span>Hasta</span>
                                                <asp:TextBox runat="server" ID="HorarioFinMartes"
                                                    CssClass="form-control text-center" Style="width: 80px"
                                                    TextMode="Number" min="0" max="23" placeholder="18" />
                                            </div>
                                        </div>

                                        <!-- MIÉRCOLES -->
                                        <div class="d-flex align-items-center mb-2 border-bottom pb-2">
                                            <div style="width: 150px;">
                                                <asp:CheckBox runat="server" Text="Miércoles" ID="CbxMiercoles"
                                                    OnCheckedChanged="CbxChecked" AutoPostBack="true" />
                                            </div>
                                            <div id="HorariosMiercoles" runat="server" visible="false"
                                                class="d-flex align-items-center gap-2">
                                                <span>Desde</span>
                                                <asp:TextBox runat="server" ID="HorarioInicioMiercoles"
                                                    CssClass="form-control text-center" Style="width: 80px"
                                                    TextMode="Number" min="0" max="23" placeholder="8" />
                                                <span>Hasta</span>
                                                <asp:TextBox runat="server" ID="HorarioFinMiercoles"
                                                    CssClass="form-control text-center" Style="width: 80px"
                                                    TextMode="Number" min="0" max="23" placeholder="18" />
                                            </div>
                                        </div>

                                        <!-- JUEVES -->
                                        <div class="d-flex align-items-center mb-2 border-bottom pb-2">
                                            <div style="width: 150px;">
                                                <asp:CheckBox runat="server" Text="Jueves" ID="CbxJueves"
                                                    OnCheckedChanged="CbxChecked" AutoPostBack="true" />
                                            </div>
                                            <div id="HorariosJueves" runat="server" visible="false"
                                                class="d-flex align-items-center gap-2">
                                                <span>Desde</span>
                                                <asp:TextBox runat="server" ID="HorarioInicioJueves"
                                                    CssClass="form-control text-center" Style="width: 80px"
                                                    TextMode="Number" min="0" max="23" placeholder="8" />
                                                <span>Hasta</span>
                                                <asp:TextBox runat="server" ID="HorarioFinJueves"
                                                    CssClass="form-control text-center" Style="width: 80px"
                                                    TextMode="Number" min="0" max="23" placeholder="18" />
                                            </div>
                                        </div>

                                        <!-- VIERNES -->
                                        <div class="d-flex align-items-center mb-2 border-bottom pb-2">
                                            <div style="width: 150px;">
                                                <asp:CheckBox runat="server" Text="Viernes" ID="CbxViernes"
                                                    OnCheckedChanged="CbxChecked" AutoPostBack="true" />
                                            </div>
                                            <div id="HorariosViernes" runat="server" visible="false"
                                                class="d-flex align-items-center gap-2">
                                                <span>Desde</span>
                                                <asp:TextBox runat="server" ID="HorarioInicioViernes"
                                                    CssClass="form-control text-center" Style="width: 80px"
                                                    TextMode="Number" min="0" max="23" placeholder="8" />
                                                <span>Hasta</span>
                                                <asp:TextBox runat="server" ID="HorarioFinViernes"
                                                    CssClass="form-control text-center" Style="width: 80px"
                                                    TextMode="Number" min="0" max="23" placeholder="18" />
                                            </div>
                                        </div>

                                        <!-- SÁBADO -->
                                        <div class="d-flex align-items-center mb-2">
                                            <div style="width: 150px;">
                                                <asp:CheckBox runat="server" Text="Sábado" ID="CbxSabados"
                                                    OnCheckedChanged="CbxChecked" AutoPostBack="true" />
                                            </div>
                                            <div id="HorariosSabados" runat="server" visible="false"
                                                class="d-flex align-items-center gap-2">
                                                <span>Desde</span>
                                                <asp:TextBox runat="server" ID="HorarioInicioSabados"
                                                    CssClass="form-control text-center" Style="width: 80px"
                                                    TextMode="Number" min="0" max="23" placeholder="8" />
                                                <span>Hasta</span>
                                                <asp:TextBox runat="server" ID="HorarioFinSabados"
                                                    CssClass="form-control text-center" Style="width: 80px"
                                                    TextMode="Number" min="0" max="23" placeholder="18" />
                                            </div>
                                        </div>

                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <label id="LblErrorHorariosPrestador" visible="false" runat="server" style="color: red"></label>
                        </div>
                    </div>

                    <!-- Un ejemplo para los términos y condiciones que vayamos a agregar (Estaría bueno) -->
                    <div class="mb-3 d-flex align-items-center">
                        <asp:CheckBox ID="chkAcepto" runat="server" />
                        <label class="ms-2">
                            Acepto los 
        <a href="#" data-bs-toggle="modal" data-bs-target="#modalTerminos">términos y condiciones
        </a>
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
        <!-- MODAL DE TERMINOS Y CONDICIONES -->
    </div>
    <div class="modal fade" id="modalTerminos" tabindex="-1" aria-labelledby="modalTerminosLabel" aria-hidden="true">
        <div class="modal-dialog modal-lg modal-dialog-scrollable">
            <div class="modal-content">

                <div class="modal-header">
                    <h5 class="modal-title" id="modalTerminosLabel">Términos y Condiciones</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal"></button>
                </div>

                <div class="modal-body">

                    <p>
                        <strong>1. Aceptación de los términos</strong><br />
                        Al registrarte como prestador en la plataforma, aceptás cumplir con estos términos y condiciones.
                    </p>

                    <p>
                        <strong>2. Responsabilidad del prestador</strong><br />
                        Sos responsable por la información que brindás: descripción, servicios, precios y zonas de trabajo.
                    </p>

                    <p>
                        <strong>3. Prestación del servicio</strong><br />
                        La plataforma actúa como intermediaria y no garantiza la contratación ni la calidad del servicio.
                    </p>

                    <p>
                        <strong>4. Información veraz</strong><br />
                        Debés proporcionar datos reales y actualizados. Cuentas con información falsa pueden ser suspendidas.
                    </p>

                    <p>
                        <strong>5. Precios</strong><br />
                        Los precios publicados son definidos por el prestador.
                    </p>

                    <p>
                        <strong>6. Disponibilidad</strong><br />
                        Debés respetar los horarios y zonas configuradas en tu perfil.
                    </p>

                    <p>
                        <strong>7. Uso de la plataforma</strong><br />
                        No se permite el uso para actividades ilegales o fraudulentas.
                    </p>

                    <p>
                        <strong>8. Modificaciones</strong><br />
                        Estos términos pueden actualizarse en cualquier momento.
                    </p>

                    <p>
                        <strong>9. Baja de cuenta</strong><br />
                        Se podrán suspender cuentas que incumplan las condiciones.
                    </p>

                </div>

                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cerrar</button>
                </div>

            </div>
        </div>
    </div>
</asp:Content>
