<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master"
    AutoEventWireup="true" CodeBehind="Mensajes.aspx.cs"
    Inherits="Fixnet.Mensajes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="styles/StyleMensajes.css" rel="stylesheet" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="mensajes-layout">

        <!-- PANEL IZQUIERDO -->
        <div class="conv-panel" id="convPanel">

            <div class="conv-header">
                <h2 class="conv-titulo">Mensajes</h2>
            </div>

            <div class="conv-lista">
                <asp:Repeater runat="server" ID="RptConversaciones">
                    <ItemTemplate>

                        <div class='<%# EsSeleccionada(
                            Eval("IdConversacion") != DBNull.Value ? Convert.ToInt32(Eval("IdConversacion")) : 0
                        ) ? "conv-item activa" : "conv-item" %>'
                            data-conv='<%# Eval("IdConversacion") %>'
                            onclick='seleccionarConv(<%# Eval("IdConversacion") %>)'>

                            <%# ObtenerAvatarConv(
                            Eval("OtroFoto"),
                            Eval("OtroNombre") != DBNull.Value ? Eval("OtroNombre").ToString() : "",
                            Eval("OtroApellido") != DBNull.Value ? Eval("OtroApellido").ToString() : "",
                            Eval("NoLeidos") != DBNull.Value ? Convert.ToInt32(Eval("NoLeidos")) : 0) %>

                            <div class="conv-info">
                                <div class="conv-nombre">
                                    <%# Eval("OtroNombre") %> <%# Eval("OtroApellido") %>

                                    <%# (Eval("NoLeidos") != DBNull.Value && Convert.ToInt32(Eval("NoLeidos")) > 0)
                                    ? "<span class='badge-noLeidos'>" + Eval("NoLeidos") + "</span>"
                                    : "" %>
                                </div>

                                <div class="conv-servicio">
                                    <%# Eval("Servicio") %>
                                </div>

                                <div class="conv-ultimo">
                                    <%# TruncarTexto(Eval("UltimoMensaje")) %>
                                </div>
                            </div>

                            <div class="conv-fecha">
                                <%# FormatearFecha(Eval("FechaUltimo")) %>
                            </div>
                            <button class="btn-eliminar-conv"
                                onclick="eliminarConv(event, <%# Eval("IdConversacion") %>)"
                                title="Eliminar">
                                🗑</button>

                        </div>

                    </ItemTemplate>
                </asp:Repeater>

                <asp:Label runat="server" ID="LblSinConversaciones" Visible="false"
                    CssClass="sin-mensajes">
                No tenés conversaciones aún.<br />
                <small>Podés iniciar una desde el detalle de un turno.</small>
                </asp:Label>
            </div>
        </div>

        <!-- PANEL DERECHO -->
        <div class="chat-panel" id="chatPanel">

            <asp:Panel runat="server" ID="PnlVacio" CssClass="chat-vacio">
                <div class="chat-vacio-ico">💬</div>
                <p>Seleccioná una conversación para ver los mensajes</p>
            </asp:Panel>

            <asp:Panel runat="server" ID="PnlChat" Visible="false" CssClass="chat-activo">

                <div class="chat-header">
                    <button type="button" class="btn-volver" onclick="volverLista()">&#8592;</button>
                    <asp:Label runat="server" ID="LblChatNombre" CssClass="chat-header-nombre" />
                    <asp:Label runat="server" ID="LblChatServicio" CssClass="chat-header-servicio" />
                </div>

                <div class="chat-body" id="chatBody">
                    <asp:Repeater runat="server" ID="RptMensajes">
                        <ItemTemplate>

                            <div class='msg-wrapper <%# EsMio((int)Eval("IdEmisor")) ? "mio" : "suyo" %>'>

                                <%# !EsMio((int)Eval("IdEmisor"))
                                ? ObtenerAvatarMsg(
                                    Eval("FotoPerfil"),
                                    Eval("Nombre").ToString(),
                                    Eval("Apellido").ToString())
                                : "" %>

                                <div class="msg-burbuja">
                                    <span class="msg-texto"><%# Eval("Texto") %></span>
                                    <div class="msg-meta">
                                        <span class="msg-hora">
                                            <%# FormatearHora(Eval("FechaEnvio")) %>
                                        </span>
                                        <%# ObtenerVisto(Eval("Leido"), Eval("IdEmisor")) %>
                                    </div>
                                </div>

                            </div>

                        </ItemTemplate>
                    </asp:Repeater>
                </div>

                <div class="chat-input-area">
                    <asp:HiddenField runat="server" ID="HfConvId" />

                    <asp:TextBox runat="server" ID="TxtMensaje"
                        CssClass="chat-input"
                        placeholder="Escribí un mensaje..."
                        MaxLength="1000"
                        TextMode="MultiLine"
                        Rows="2" />

                    <button type="button"
                        id="BtnEnviar"
                        class="btn-enviar">
                        &#9658;
                    </button>
                </div>

            </asp:Panel>
        </div>

    </div>
    <script>
        console.log('Email usuario:', '<%= Session["Usuario"] != null ? ((Dominio.Usuario)Session["Usuario"]).EmailUsuario : "null" %>');
        console.log('Nombre usuario:', '<%= Session["Usuario"] != null ? ((Dominio.Usuario)Session["Usuario"]).NombreUsuario : "null" %>');
        var miIdUsuario = <%= Session["Usuario"] != null ? ((Dominio.Usuario)Session["Usuario"]).IdUsuario : 0 %>;
        var convActual = <%= ConvSeleccionada %>;
        var miNombre = '<%= Session["Usuario"] != null ? ((Dominio.Usuario)Session["Usuario"]).NombreUsuario : "" %>';
        var miApellido = '<%= Session["Usuario"] != null ? ((Dominio.Usuario)Session["Usuario"]).ApellidoUsuario : "" %>';
        var miFoto = '<%= Session["Usuario"] != null ? (((Dominio.Usuario)Session["Usuario"]).FotoPerfil ?? "") : "" %>';
        var ultimoMensajeCount = document.querySelectorAll('.msg-wrapper').length;
        var ultimoHash = '';
    </script>
    <script src="scripts/Mensajes.js"></script>
</asp:Content>
