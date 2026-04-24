<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Contactanos.aspx.cs" Inherits="Fixnet.Contactanos" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <!-- HEADER -->
    <section class="text-center py-5">
        <div class="container">
            <h1 class="fw-bold text-info">Contáctanos</h1>
            <p class="lead text-light">Estamos para ayudarte. Escribinos y te respondemos a la brevedad.</p>
        </div>
    </section>

    <!-- FORM + INFO -->
    <section class="py-5">
        <div class="container">
            <div class="row">

                <!-- FORMULARIO -->
                <div class="col-lg-7 mb-4">
                    <div class="card shadow border-0 p-4 glass-modal">

                        <h4 class="mb-3 fw-bold">Enviá un mensaje</h4>

                        <div class="floating">
                            <label>Nombre</label>
                            <input type="text" id="inputNombre" class="form-control" placeholder=" " />
                        </div>

                        <div class="floating">
                            <label class="form-label">Asunto</label>
                            <input type="text" id="inputAsunto" class="form-control" placeholder="¿En qué te podemos ayudar?" />
                        </div>

                        <div class="floating">
                            <label class="form-label">Mensaje</label>
                            <textarea id="inputMensaje" class="form-control" rows="6"
                                placeholder="Escribí tu mensaje..."></textarea>
                            <small class="text-muted mt-1 d-block">* Recordá incluir tu mail u otros datos de contacto en el mensaje para que podamos responderte.
                            </small>
                        </div>

                        <span id="lblErrorContacto" class="text-danger mb-2 d-block"></span>

                        <button type="button" id="btnEnviarContacto" onclick="enviarContacto()"
                            class="btn btn-primary w-100 fw-bold">
                            Enviar mensaje
                        </button>

                    </div>
                </div>

                <!-- INFO DE CONTACTO -->
                <div class="col-lg-5">
                    <div class="card shadow border-0 p-4 h-100 glass-modal">

                        <h4 class="mb-4 fw-bold">Información de contacto</h4>

                        <p class="mb-3">
                        <p class="mb-3">
                            <i class="fa fa-envelope me-2 text-info"></i>
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
                        Nuestro equipo está comprometido en ofrecerte la mejor experiencia posible
                        dentro de la plataforma. Agradecemos tu confianza en FixNet.
                        </p>

                    </div>
                </div>

            </div>
        </div>
    </section>

    <!-- MAPA -->
    <section class="pb-5">
        <div class="container">
            <div class="ratio ratio-16x9 shadow glass-modal">
                <iframe
                    src="https://maps.google.com/maps?q=Soldado%20de%20Malvinas%20278%20Villa%20Adelina%20Buenos%20Aires&z=16&output=embed"
                    width="100%" height="350" style="border: 0;" allowfullscreen="" loading="lazy"></iframe>
            </div>
        </div>
    </section>

    <!-- MODAL: mensaje enviado OK -->
    <div class="modal fade" id="modalEnviado" tabindex="-1" aria-hidden="true">
        <div class="modal-dialog modal-dialog-centered">
            <div class="modal-content glass-modal">
                <div class="modal-header modal-header-glass">
                    <h5 class="modal-title modal-title-glass">¡Mensaje enviado!</h5>
                </div>
                <div class="modal-body modal-body-glass">
                    <p class="fs-5">Recibimos tu consulta. Te contactaremos a la brevedad.</p>
                </div>
                <div class="modal-footer">
                    <a href="/Default.aspx" class="btn btn-success px-4">Volver al inicio</a>
                </div>
            </div>
        </div>
    </div>
    <script src="scripts/EnviarMailScript.js"></script>

</asp:Content>
