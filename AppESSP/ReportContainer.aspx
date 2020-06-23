<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportContainer.aspx.cs" Inherits="AppESSP.ReportContainer" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=14.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
   <form id="form1" runat="server">
     <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeOut="56000"></asp:ScriptManager>
       <rsweb:ReportViewer AsyncRendering="false" ID="reportViewer" Height="1500px" Width="1000px" SizeToReportContent="true" runat="server"></rsweb:ReportViewer>
    </form>
</body>
</html>
