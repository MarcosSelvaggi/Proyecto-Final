<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="BuscarPrestadores.aspx.cs" Inherits="Fixnet.BuscarPrestadores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/StyleBuscarPrestadores.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:ScriptManager runat="server" />

    <div class="container py-4 buscador-container">

        <div class="filtros-box mb-4">
            <div class="row g-3 align-items-end">

                <div class="col-12">
                    <h4 class="titulo-seccion">Encontrá un prestador</h4>
                    <small class="subtitulo-seccion">Filtrá por servicio y precio según tu zona.</small>
                </div>

                <div class="col-md-4">
                    <label class="form-label label-dark">Servicio</label>
                    <asp:DropDownList runat="server" ID="DdlServicio"
                        CssClass="form-control input-dark"
                        OnSelectedIndexChanged="DdlServicio_SelectedIndexChanged"
                        AutoPostBack="true" />
                </div>

                <div class="col-md-5" id="contenedorSlider" runat="server" visible="false">
                    <label class="form-label label-dark">
                        Precio máximo: <span id="sliderLabel"></span>
                    </label>
                    <input type="range" class="form-range slider-dark"
                        id="sliderPrecio"
                        min="0" max="50000" step="500" value="50000"
                        oninput="filtrarPorPrecio(this.value)" />
                </div>

                <div class="col-md-3 text-md-end">
                    <asp:Label runat="server" ID="LblContador"
                        CssClass="badge contador-resultados"
                        Visible="false" />
                </div>

            </div>
        </div>

        <!-- RESULTADOS -->
        <div class="row g-4" id="gridPrestadores">
            <asp:Repeater runat="server" ID="RptPrestadores">
                <ItemTemplate>
                    <div class="col-md-6 col-lg-4 prestador-item"
                        data-precio="<%# Eval("PrecioServicio") %>">

                        <div class="card prestador-card h-100">
                            <div class="card-body d-flex flex-column">

                                <!-- HEADER -->
                                <div class="d-flex align-items-center gap-3 mb-2">

                                    <%# ObtenerAvatar(
                                        Eval("FotoPerfil"),
                                        Eval("NombreUsuario").ToString(),
                                        Eval("ApellidoUsuario").ToString()
                                    ) %>

                                    <div>
                                        <div class="nombre-prestador">
                                            <%# Eval("NombreUsuario") %> <%# Eval("ApellidoUsuario") %>
                                        </div>
                                        <div class="email-prestador">
                                            <%# Eval("EmailUsuario") %>
                                        </div>
                                    </div>

                                </div>

                                <!-- DESCRIPCIÓN -->
                                <p class="descripcion-prestador">
                                    <asp:Label Text='<%# Eval("CalificacionPrestador") %>' runat="server" Visible='<%# MostrarCalificaciones(float.Parse(Eval("CalificacionPrestador").ToString())) %>' />
                                    <asp:Label Text=" ★" runat="server" Visible='<%# MostrarCalificaciones(float.Parse(Eval("CalificacionPrestador").ToString())) %>' CssClass="star-rating" />
                                    <br />
                                    <%# Eval("Prestador.DescripcionPrestador") %>
                                </p>

                                <!-- FOOTER -->
                                <div class="mt-auto pt-3 border-top border-secondary d-flex justify-content-between align-items-center">

                                    <span class="telefono-prestador">
                                        <i class="fa fa-phone me-2 text-success"></i>
                                        <%# Eval("TelefonoUsuario") %>
                                    </span>

                                    <span class="precio-badge">$<%# Eval("PrecioServicio", "{0:N0}") %>/h
                                    </span>
                                </div>
                                <a href='SolicitarTurno.aspx?id=<%# Eval("IdUsuario") %>'
                                    class="btn btn-accion mt-3">Solicitar Turno
                                </a>
                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>
        <div id="mensajeVacio" class="mensaje-vacio">
            No hay prestadores disponibles para los filtros seleccionados.
        </div>
    </div>

    <script>
        function filtrarPorPrecio(valor) {
            document.getElementById('sliderLabel').textContent = '$' + parseInt(valor).toLocaleString('es-AR');
            var items = document.querySelectorAll('.prestador-item');
            var visibles = 0;
            items.forEach(function (item) {
                var precio = parseFloat(item.dataset.precio) || 0;
                var visible = precio <= parseFloat(valor);
                item.style.display = visible ? '' : 'none';
                if (visible) visibles++;
            });
            document.getElementById('mensajeVacio').style.display = visibles === 0 ? '' : 'none';
        }

        window.addEventListener('DOMContentLoaded', function () {
            var slider = document.getElementById('sliderPrecio');
            if (slider) {
                document.getElementById('sliderLabel').textContent =
                    '$' + parseInt(slider.value).toLocaleString('es-AR');
            }
            var items = document.querySelectorAll('.prestador-item');
            if (items.length === 0) {
                document.getElementById('mensajeVacio').style.display = '';
            }
        });
    </script>

</asp:Content>
