<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PerfilPrestador.aspx.cs" Inherits="Fixnet.PerfilPrestador" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <% Dominio.Usuario Usuario = new Dominio.Usuario();
        Usuario = (Dominio.Usuario)Session["Usuario"];
        if (Usuario.Prestador.DescripcionPrestador == "No ingresado") //Con esto nos aseguramos que sea un perfil nuevo
        {%>
    <div class="container">
        <div class="form">
            <div class="col-8 py-2 justify_content_center">
                <h2>Vamos a realizar la configuración de tu perfil</h2>
                <div class="mb-3">
                    <label for="txtDescripcion" class="form-label">Contanos un poco de vos</label>
                    <asp:TextBox CssClass="form-control" ID="txtDescripcion" aria-describedby="Descripcion" runat="server"></asp:TextBox>
                </div>
                <div class="mb-3">
                    <label for="ddlServicios" class="form-label">¿Qué servicios podés ofrecer?</label>
                    <asp:DropDownList ID="ddlServicios" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlServicios_SelectedIndexChanged"></asp:DropDownList> 
                    <%if (Session["ServiciosOfrecidos" + !=  null])
                {

                        } %>
                </div>
                <div class="mb-3">
                    <input type="checkbox" class="form-check-input" id="exampleCheck1">
                    <label class="form-check-label" for="exampleCheck1">Check me out</label>
                </div>
                <button type="submit" class="btn btn-primary">Submit</button>
            </div>
        </div>
    </div>

    }<%
         }
         else
         {
    %>
    <h1>Algo</h1>
    <% }%>

</asp:Content>

