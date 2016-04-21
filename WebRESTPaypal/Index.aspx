<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="WebRESTPaypal.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>REST HATEOAS</title>
    <style>
        .hidden {
            display: none; 

        }
    </style>
    <script>
        function mostrarResponse()
        {
            var btnver = document.getElementById("btnVerocultarresponse");
            if (btnver.firstChild.data == "VER RESPONSE") {
                var element = document.getElementById("lbResponse");
                element.classList.remove("hidden");
                btnver.firstChild.data = "OCULTAR RESPONSE";
            }
            else {
                var element = document.getElementById("lbResponse");
                element.classList.add("hidden");
                btnver.firstChild.data = "VER RESPONSE";
            }
        }
        
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <center>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="updatepanel" runat="server">
            <ContentTemplate>
                <div style="text-align: center;">
                        <asp:Label ID="lbMetodo" runat="server" Text="..." Font-Size="Large"></asp:Label>
                        <br/>
                        <asp:Label ID="lbTarea" runat="server" Text="..." Font-Size="Large"></asp:Label>
                        <br/>
                        <button id="btnVerocultarresponse" type="button" onclick="mostrarResponse()">VER RESPONSE</button>
                        <br/>
                        <asp:Label ID="lbResponse" runat="server" Text="..." CssClass="hidden" ></asp:Label>
                        <br/>
                        <br/>
                        <asp:Label ID="Label1" runat="server" Text="Lista de HATEOAS en el Response"></asp:Label>
                        <br/>
                        <center>
                        <asp:ListView ID="lvLinks" runat="server" OnItemCommand="lvLinks_ItemCommand" >
                            <LayoutTemplate>
                                <table runat="server" class="table table-condensed" id="tblItems" style="border:0px solid black;"  >
                                <tr runat="server"  >
                                    <th runat="server" style="background:#ffd800;" >HREF</th>
                                    <th runat="server" style="background:#ffd800;">REL</th>
                                    <th runat="server" style="background:#ffd800;">METHOD</th>
                                </tr>
                                <tr runat="server" id="itemPlaceholder" />
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr runat="server" style="border-bottom:1px solid black;">
                                <td style="border-bottom:1px solid black;">
                                  <asp:Label ID="href" runat="Server" Text='<%#Eval("href") %>' />
                                </td>
                                <td style="border-bottom:1px solid black;">
                                  <asp:Label ID="rel" runat="Server" Text='<%#Eval("rel") %>' />
                                </td>
                                <td style="border-bottom:1px solid black;">
                                  <asp:LinkButton ID="LinkButton2" runat="Server" Text='<%#Eval("method") %>' CommandName="Execute" CommandArgument='<%#Eval("href").ToString()+","+Eval("rel")+","+Eval("method")%>' />
                                </td>
                              </tr>
                            </ItemTemplate>
                        </asp:ListView>
                        </center>
                        <br />
                </div>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="lvLinks" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:Label ID="lbEstado" runat="server" Text="" Font-Size="X-Large" ForeColor="#33CC33" Visible="false"></asp:Label>
        <br />
        <br />
        <asp:Button ID="btnCrearTarjeta" runat="server" Text="CREAR TARJETA" Visible="false" OnClick="btnCrearTarjeta_Click" />
        </center>
    </form>
</body>
</html>
