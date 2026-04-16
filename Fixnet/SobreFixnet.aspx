<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="SobreFixnet.aspx.cs" Inherits="Fixnet.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Animaciones -->
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/animate.css/4.1.1/animate.min.css"/>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<!-- 🌟 HERO -->
<section class="bg-dark text-white text-center d-flex align-items-center" style="height: 80vh;">
    <div class="container">
        <h1 class="display-3 fw-bold animate__animated animate__fadeInDown">Sobre Fixnet</h1>
        <p class="lead mt-3 animate__animated animate__fadeInUp">
            Conectamos personas con profesionales de confianza en un solo lugar
        </p>
    </div>
</section>

<!-- 💡 QUIÉNES SOMOS -->
<section class="py-5">
    <div class="container text-center">
        <h2 class="fw-bold mb-4">¿Qué es Fixnet?</h2>
        <p class="text-muted fs-5">
            Fixnet es una plataforma donde podés encontrar prestadores de servicios como gasistas, electricistas y técnicos,
            todo centralizado en un solo lugar.
        </p>
    </div>
</section>

<!-- ⚙️ FUNCIONALIDADES -->
<section class="py-5 bg-light">
    <div class="container">
        <div class="row text-center">

            <div class="col-md-4 mb-4">
                <div class="card border-0 shadow h-100 p-4 hover-card">
                    <div class="mb-3">
                        <i class="fa fa-search fa-3x text-primary"></i>
                    </div>
                    <h5 class="fw-bold">Buscar profesionales</h5>
                    <p class="text-muted">Encontrá prestadores fácilmente según tus necesidades.</p>
                </div>
            </div>

            <div class="col-md-4 mb-4">
                <div class="card border-0 shadow h-100 p-4 hover-card">
                    <div class="mb-3">
                        <i class="fa fa-calendar fa-3x text-success"></i>
                    </div>
                    <h5 class="fw-bold">Gestionar turnos</h5>
                    <p class="text-muted">Los profesionales pueden administrar sus turnos de forma simple.</p>
                </div>
            </div>

            <div class="col-md-4 mb-4">
                <div class="card border-0 shadow h-100 p-4 hover-card">
                    <div class="mb-3">
                        <i class="fa fa-users fa-3x text-danger"></i>
                    </div>
                    <h5 class="fw-bold">Solicitar servicios</h5>
                    <p class="text-muted">Los clientes pueden pedir turnos y contratar servicios rápidamente.</p>
                </div>
            </div>

        </div>
    </div>
</section>

<section class="py-5 text-center">
    <div class="container">
        <h2 class="fw-bold mb-4">Nuestra misión</h2>
        <p class="text-muted fs-5">
            Hacer que encontrar y ofrecer servicios sea simple, rápido y seguro para todos.
        </p>
    </div>
</section>


<section class="py-5 bg-primary text-white text-center">
    <div class="container">
        <h2 class="fw-bold mb-3">¿Listo para empezar?</h2>
        <p class="mb-4">Registrate y comenzá a usar Fixnet hoy mismo.</p>
        <a href="/Registro.aspx" class="btn btn-light btn-lg fw-bold">Crear cuenta</a>
    </div>
</section>


<style>
    .hover-card {
        transition: transform 0.3s ease, box-shadow 0.3s ease;
    }

    .hover-card:hover {
        transform: translateY(-10px);
        box-shadow: 0 10px 25px rgba(0,0,0,0.2);
    }
</style>

</asp:Content>