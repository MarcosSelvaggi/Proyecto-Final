<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="BuscarPrestadores.aspx.cs" Inherits="Fixnet.BuscarPrestadores" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <!-- ESTILOS PARA QUE QUEDE MAS LINDO -->
    <style>
        .card-prestador { transition: box-shadow .15s; }
        .card-prestador:hover { box-shadow: 0 4px 12px rgba(0,0,0,.1); }
        .avatar-iniciales {
            width: 44px; height: 44px; border-radius: 50%;
            background: #e0e7ff; color: #4338ca;
            display: flex; align-items: center; justify-content: center;
            font-weight: 500; font-size: 15px; flex-shrink: 0;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container py-4">

        <div class="row g-3 align-items-end mb-4">
            <div class="col-12">
                <h4 class="mb-0">Encontrá un prestador</h4>
                <small class="text-muted">Filtrá por servicio y precio según tu zona.</small>
            </div>

            <div class="col-md-4">
                <label class="form-label small text-muted mb-1">Servicio</label>
                <asp:DropDownList runat="server" ID="DdlServicio" CssClass="form-select"
                    OnSelectedIndexChanged="DdlServicio_SelectedIndexChanged" AutoPostBack="true" />
            </div>

            <div class="col-md-5" id="contenedorSlider" runat="server" visible="false">
                <label class="form-label small text-muted mb-1">
                    Precio máximo por hora: <span id="sliderLabel"></span>
                </label>
                <input type="range" class="form-range" id="sliderPrecio"
                    min="0" max="50000" step="500" value="50000"
                    oninput="filtrarPorPrecio(this.value)" />
            </div>

            <div class="col-md-3 text-md-end">
                <asp:Label runat="server" ID="LblContador" CssClass="badge bg-primary fs-6" Visible="false" />
            </div>
        </div>

        <!-- REPEATER PRESTADORES -->
        <div class="row g-3" id="gridPrestadores">
            <asp:Repeater runat="server" ID="RptPrestadores">
                <ItemTemplate>
                    <div class="col-md-6 col-lg-4 prestador-item"
                         data-precio="<%# Eval("PrecioServicio") %>">
                        <div class="card h-100 card-prestador">
                            <div class="card-body d-flex flex-column gap-2">

                                <div class="d-flex align-items-center gap-3">
                                    <div class="avatar-iniciales">
                                        <%# ObtenerIniciales(Eval("NombreUsuario").ToString(), Eval("ApellidoUsuario").ToString()) %>
                                    </div>
                                    <div>
                                        <div class="fw-medium"><%# Eval("NombreUsuario") %> <%# Eval("ApellidoUsuario") %></div>
                                        <div class="small text-muted"><%# Eval("EmailUsuario") %></div>
                                    </div>
                                </div>

                                <p class="small text-muted mb-0"><%# Eval("Prestador.DescripcionPrestador") %></p>

                                <div class="d-flex justify-content-between align-items-center mt-auto pt-2 border-top">
                                    <span class="small text-muted">
                                        Tel: <strong><%# Eval("TelefonoUsuario") %></strong>
                                    </span>
                                    <span class="badge bg-light text-dark border fw-medium">
                                        $<%# Eval("PrecioServicio", "{0:N0}") %>/h
                                    </span>
                                </div>

                                <a href='SolicitarTurno.aspx?id=<%# Eval("IdUsuario") %>'
                                   class="btn btn-outline-primary btn-sm">
                                    Solicitar Turno
                                </a>

                            </div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <div id="mensajeVacio" class="text-center text-muted py-5" style="display:none">
            No hay prestadores disponibles para los filtros seleccionados.
        </div>

    </div>

    <!-- SLIDER PARA EL PRECIO-->
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
        });
    </script>

</asp:Content>

