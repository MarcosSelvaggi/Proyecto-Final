<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="BuscarPrestadores.aspx.cs" Inherits="Fixnet.BuscarPrestadores" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="container">
        <div class="row">
            <div class="col-4">
                <h3>Seleccioná el servicio</h3>
            </div>
            <div class="col-8">
                <asp:DropDownList runat="server" ID="DdlServicio" OnSelectedIndexChanged="DdlServicio_SelectedIndexChanged" AutoPostBack="true">
                </asp:DropDownList>
            </div>
            <div class="row">
                <asp:Repeater runat="server" ID="RptPrestadores">
                    <ItemTemplate>
                        <div class="col-12 text-start">
                            <h3><p><%# Eval("Nombre") %> <%# Eval("Apellido") %></p>
                            </h3>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
                
            </div>

        </div>
    </div>



</asp:Content>
