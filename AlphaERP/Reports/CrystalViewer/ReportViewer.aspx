<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ReportViewer.aspx.cs" Inherits="AlphaERP.Reports.CrystalViewer.ReportViewer" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>تقارير</title>
</head>
<body>
    <form id="form1" class ="alpha-erp-viewer" runat="server">
        <asp:ImageButton ID="AlphaPrint" runat="server" Height="20" Width="25"  style="position: absolute !important; z-index: 19 !important;  margin-left: 11px !important;  margin-top: 5px !important;cursor:pointer " OnClick="Print_Click" />
    <div>
        <CR:CrystalReportViewer ID="CrystalReportViewer" runat="server"   />
    </div>
    </form>
</body>
</html>
