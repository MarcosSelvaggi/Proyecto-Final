<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="SobreFixnet.aspx.cs" Inherits="Fixnet.SobreFixnet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/StyleSobreFixnet.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <!-- HERO -->
    <section class="sobre-hero">
        <div class="container">
            <h1>Sobre FixNet</h1>
            <p>Conectamos personas con profesionales de confianza en un solo lugar.</p>
        </div>
    </section>

    <!-- QUÉ ES FIXNET -->
    <section class="sobre-que">
        <div class="container">
            <h2>¿Qué es FixNet?</h2>
            <p>
                FixNet es una plataforma donde podés encontrar prestadores de servicios como gasistas,
                electricistas y técnicos, todo centralizado en un solo lugar.
           
            </p>
        </div>
    </section>

    <!-- FUNCIONALIDADES -->
    <section class="sobre-funcionalidades">
        <div class="container">
            <h2>¿Qué podés hacer?</h2>
            <div class="func-grid">

                <div class="func-card">
                    <i class="fa fa-search text-info"></i>
                    <h5>Buscar profesionales</h5>
                    <p>Encontrá prestadores fácilmente según tus necesidades y zona.</p>
                </div>

                <div class="func-card">
                    <i class="fa fa-calendar text-success"></i>
                    <h5>Gestionar turnos</h5>
                    <p>Los profesionales pueden administrar sus turnos de forma simple.</p>
                </div>

                <div class="func-card">
                    <i class="fa fa-users text-warning"></i>
                    <h5>Solicitar servicios</h5>
                    <p>Los clientes pueden pedir turnos y contratar servicios rápidamente.</p>
                </div>

                <div class="func-card">
                    <i class="fa fa-comments text-info"></i>
                    <h5>Mensajería integrada</h5>
                    <p>Chateá directamente con el profesional antes de confirmar tu servicio.</p>
                </div>

            </div>
        </div>
    </section>

    <!-- MISIÓN -->
    <section class="sobre-mision">
        <div class="container">
            <div class="mision-box">
                <h2>Nuestra misión</h2>
                <p>
                    Hacer que encontrar y ofrecer servicios sea simple, rápido y seguro para todos.
                    Creemos en la transparencia, la confianza y en poner a las personas primero.
               
                </p>
            </div>
        </div>
    </section>

    <!-- CTA -->
    <section class="sobre-cta">
        <div class="container">
            <div class="cta-box">
                <h2>¿Listo para empezar?</h2>
                <p>Registrate y comenzá a usar FixNet hoy mismo.</p>
                <a href="/Registro.aspx" class="btn-cta">Crear cuenta</a>
            </div>
        </div>
    </section>
</asp:Content>