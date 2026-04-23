<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Contactanos.aspx.cs" Inherits="Fixnet.Contactanos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<!-- 🌟 HEADER -->
<section class="bg-primary text-white text-center py-5">
    <div class="container">
        <h1 class="fw-bold">Contáctanos</h1>
        <p class="lead">Estamos para ayudarte. Escribinos y te respondemos a la brevedad.</p>
    </div>
</section>

<!-- 📩 FORM + INFO -->
<section class="py-5">
    <div class="container">
        <div class="row">

            <!-- 📨 FORMULARIO -->
            <div class="col-lg-7 mb-4">
                <div class="card shadow border-0 p-4">

                    <h4 class="mb-3 fw-bold">Enviá un mensaje</h4>

                    <form>

                        <div class="mb-3">
                            <label class="form-label">Nombre</label>
                            <input type="text" class="form-control" placeholder="Tu nombre">
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Email</label>
                            <input type="email" class="form-control" placeholder="tu@email.com">
                        </div>

                        <div class="mb-3">
                            <label class="form-label">Mensaje</label>
                            <textarea class="form-control" rows="5" placeholder="Escribí tu mensaje..."></textarea>
                        </div>

                        <button type="submit" class="btn btn-primary w-100 fw-bold">
                            Enviar mensaje
                        </button>

                    </form>

                </div>
            </div>

            <!-- 📞 INFO DE CONTACTO -->
            <div class="col-lg-5">
                <div class="card shadow border-0 p-4 h-100">

                    <h4 class="mb-4 fw-bold">Información de contacto</h4>

                    <p class="mb-3">
                        <i class="fa fa-envelope me-2 text-primary"></i>
                        contacto@fixnet.com
                    </p>

                    <p class="mb-3">
                        <i class="fa fa-phone me-2 text-success"></i>
                        +54 11 1234-5678
                    </p>

                    <p class="mb-3">
                        <i class="fa fa-map-marker me-2 text-danger"></i>
                        Buenos Aires, Argentina
                    </p>

                    <hr>

                    <p class="text-muted">
                        Nos esforzamos por brindarte una solución rápida y efectiva a tu consulta.
Mientras tanto, podés revisar nuestras preguntas frecuentes o seguir explorando el sitio.
También podés ponerte en contacto con nosotros a través de nuestros canales de atención para recibir ayuda personalizada.
Nuestro equipo está comprometido en ofrecerte la mejor experiencia posible dentro de la plataforma.
Agradecemos tu confianza en FixNet y estamos para acompañarte en cada paso.
                    </p>

                </div>
            </div>

        </div>
    </div>
</section>

<!-- 📍 MAPA (opcional visual) -->
<section class="pb-5">
    <div class="container">
        <div class="ratio ratio-16x9 shadow">
            <iframe 
    src="https://maps.google.com/maps?q=Soldado%20de%20Malvinas%20278%20Villa%20Adelina%20Buenos%20Aires&z=16&output=embed"
    width="100%" 
    height="350" 
    style="border:0;" 
    allowfullscreen="" 
    loading="lazy">
</iframe>
        </div>
    </div>
</section>

</asp:Content>