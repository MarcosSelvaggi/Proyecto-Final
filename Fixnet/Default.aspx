<%@ Page Title="Fixnet" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Fixnet.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/StyleDefault.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="home-container">
        <main>
            <div class="hero-section">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-xl-6 col-sm-12">
                            <div id="carouselProveedores" class="carousel slide carousel-fade" data-bs-ride="carousel">
                                <div class="carousel-indicators">
                                    <button type="button" data-bs-target="#carouselProveedores" data-bs-slide-to="0" class="active" aria-current="true" aria-label="Slide 1"></button>
                                    <button type="button" data-bs-target="#carouselProveedores" data-bs-slide-to="1" aria-label="Slide 2"></button>
                                    <button type="button" data-bs-target="#carouselProveedores" data-bs-slide-to="2" aria-label="Slide 3"></button>
                                </div>
                                <div class="carousel-inner">
                                    <div class="carousel-item active">
                                        <img src="assets/img-proveedor-1.png" class="d-block w-100" data-bs-interval="200" alt="...">
                                        <div class="carousel-caption">
                                            <h2>Mejores posibilidades</h2>
                                            <div class="d-none d-md-block">
                                                <h3>Aumenta las posibilidades de conseguir potenciales clientes con nuestro servicio.</h3>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="carousel-item">
                                        <img src="assets/img-proveedor-2.png" class="d-block w-100" alt="...">
                                        <div class="carousel-caption">
                                            <h2>Sin límites</h2>
                                            <div class="d-none d-md-block">
                                                <h3>Sin importar el rubro, ofrece tus servicios a nuestros clientes para potenciar su trabajo y aumentar tus ganancias.</h3>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="carousel-item">
                                        <img src="assets/img-proveedor-3.png" class="d-block w-100" alt="...">
                                        <div class="carousel-caption">
                                            <h2>Facilidad de conseguir clientes</h2>
                                            <div class="d-none d-md-block">
                                                <h3>Cientos de potenciales clientes a pocos clicks de distancia con nuestro servicio.</h3>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <button class="carousel-control-prev" type="button" data-bs-target="#carouselProveedores" data-bs-slide="prev">
                                    <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                                    <span class="visually-hidden">Previous</span>
                                </button>
                                <button class="carousel-control-next" type="button" data-bs-target="#carouselProveedores" data-bs-slide="next">
                                    <span class="carousel-control-next-icon" aria-hidden="true"></span>
                                    <span class="visually-hidden">Next</span>
                                </button>
                            </div>
                            <div class="btnPublicarServicios py-2">
                                <% if (Session["Usuario"] != null)
                                    { %>
                                <a href="/PerfilPrestador.aspx" class="PublicarServicios">Publicar servicios</a>
                                <% } %>
                                <% else
                                    {%>
                                <a href="/Logearse.aspx" class="PublicarServicios">Publicar servicios</a>
                                <% }%>
                            </div>
                        </div>
                        <div class="col-xl-6 col-sm-12">
                            <div id="carouselClientes" class="carousel slide carousel-fade" data-bs-ride="carousel">
                                <div class="carousel-indicators">
                                    <button type="button" data-bs-target="#carouselClientes" data-bs-slide-to="0" class="active" aria-current="true" aria-label="Slide 1"></button>
                                    <button type="button" data-bs-target="#carouselClientes" data-bs-slide-to="1" aria-label="Slide 2"></button>
                                    <button type="button" data-bs-target="#carouselClientes" data-bs-slide-to="2" aria-label="Slide 3"></button>
                                </div>
                                <div class="carousel-inner">
                                    <div class="carousel-item active">
                                        <img src="assets/img-usuario-1.png" class="d-block img-fluid" alt="...">
                                        <div class="carousel-caption">
                                            <h2>Desde la comodidad de tu casa</h2>
                                            <div class="d-none d-md-block">
                                                <h3>Encontrá al profesional indicado desde la falicidad de tu ordenador o celular con pocos clicks.</h3>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="carousel-item">
                                        <img src="assets/img-usuario-2.png" class="d-block img-fluid" alt="...">
                                        <div class="carousel-caption">
                                            <h2>Todos los rubros</h2>
                                            <div class="d-none d-md-block">
                                                <h3>Busca al profesional indicado para cualquier tipo de arreglo necesario con nuestro servicio.</h3>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="carousel-item">
                                        <img src="assets/img-usuario-3.png" class="d-block w-100" alt="...">
                                        <div class="carousel-caption">
                                            <h2>Encontrá al profesional indicado</h2>
                                            <div class="d-none d-md-block">
                                                <h3>¿Necesitas ayuda encontrando al profesional indicado?</h3>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <button class="carousel-control-prev" type="button" data-bs-target="#carouselClientes" data-bs-slide="prev">
                                    <span class="carousel-control-prev-icon" aria-hidden="true"></span>
                                    <span class="visually-hidden">Previous</span>
                                </button>
                                <button class="carousel-control-next" type="button" data-bs-target="#carouselClientes" data-bs-slide="next">
                                    <span class="carousel-control-next-icon" aria-hidden="true"></span>
                                    <span class="visually-hidden">Next</span>
                                </button>
                            </div>
                            <div class="btnPublicarServicios py-2">
                                <% if (Session["Usuario"] != null)
                                    { %>
                                <a href="/SolicitarTurno.aspx" class="PublicarServicios">Buscar profesionales</a>
                                <% } %>
                                <% else
                                    {%>
                                <a href="/Logearse.aspx" class="PublicarServicios">Buscar profesionales</a>
                                <% }%>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </main>
    </div>
</asp:Content>
