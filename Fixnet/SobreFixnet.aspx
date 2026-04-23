<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="SobreFixnet.aspx.cs" Inherits="Fixnet.WebForm1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
    
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/animate.css/4.1.1/animate.min.css"/>

    <style>
        :root {
            --fixnet-green: #28a745;
            --fixnet-blue: #0d6efd;
        }

        .bg-fixnet-green {
            background-color: var(--fixnet-green);
        }

        .bg-fixnet-blue {
            background-color: var(--fixnet-blue);
        }

        .hover-card:hover {
            transform: translateY(-8px);
            transition: 0.3s;
        }
    </style>

</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<section class="bg-fixnet-green text-white d-flex align-items-center justify-content-center text-center" style="height: 80vh;">
    <div class="container">
        <h1 class="display-3 fw-bold animate__animated animate__fadeInDown">
            Sobre Fixnet
        </h1>
        <p class="lead mt-3 animate__animated animate__fadeInUp">
            Conectamos personas con profesionales de confianza en un solo lugar
        </p>
    </div>
</section>

<!--  QUIÉNES SOMOS -->
<section class="py-5 text-center">
    <div class="container">
        <h2 class="fw-bold mb-4">¿Qué es Fixnet?</h2>
        <p class="text-muted fs-5">
            Fixnet es una plataforma donde podés encontrar prestadores de servicios como gasistas,
            electricistas y técnicos, todo centralizado en un solo lugar.
        </p>
    </div>
</section>

<!-- ⚙FUNCIONALIDADES -->
<section class="py-5 bg-fixnet-blue text-white">
    <div class="container">
        <div class="row text-center">

            <div class="col-md-4 mb-4">
                <div class="card border-0 shadow h-100 p-4 hover-card">
                    <i class="fa fa-search fa-3x text-primary mb-3"></i>
                    <h5 class="fw-bold">Buscar profesionales</h5>
                    <p class="text-muted">
                        Encontrá prestadores fácilmente según tus necesidades.
                    </p>
                </div>
            </div>

            <div class="col-md-4 mb-4">
                <div class="card border-0 shadow h-100 p-4 hover-card">
                    <i class="fa fa-calendar fa-3x text-success mb-3"></i>
                    <h5 class="fw-bold">Gestionar turnos</h5>
                    <p class="text-muted">
                        Los profesionales pueden administrar sus turnos de forma simple.
                    </p>
                </div>
            </div>

            <div class="col-md-4 mb-4">
                <div class="card border-0 shadow h-100 p-4 hover-card">
                    <i class="fa fa-users fa-3x text-danger mb-3"></i>
                    <h5 class="fw-bold">Solicitar servicios</h5>
                    <p class="text-muted">
                        Los clientes pueden pedir turnos y contratar servicios rápidamente.
                    </p>
                </div>
            </div>

        </div>
    </div>
</section>

<!-- MISIÓN -->
<section class="py-5 text-center">
    <div class="container">
        <h2 class="fw-bold mb-4">Nuestra misión</h2>
        <p class="text-muted fs-5">
            Hacer que encontrar y ofrecer servicios sea simple, rápido y seguro para todos.
        </p>
    </div>
</section>

<!-- CTA -->
<section class="py-5 bg-primary text-white text-center">
    <div class="container">
        <h2 class="fw-bold mb-3">¿Listo para empezar?</h2>
        <p class="mb-4">Registrate y comenzá a usar FixNet hoy mismo.</p>
        <a href="/Registro.aspx" class="btn btn-light btn-lg fw-bold">
            Crear cuenta
        </a>
    </div>
</section>

</asp:Content>