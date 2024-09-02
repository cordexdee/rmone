<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/master/Main.Master" CodeBehind="ImportExcelWizard.aspx.cs" Inherits="uGovernIT.Web.ImportExcelWizard" %>
<%@ MasterType VirtualPath="~/master/Main.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderContainer" runat="server">
<script src="https://cdnjs.cloudflare.com/ajax/libs/FileSaver.js/1.3.8/FileSaver.js"></script>
<script src="https://cdnjs.cloudflare.com/ajax/libs/exceljs/3.3.1/exceljs.js"></script>
    <style>
        .dx-datagrid .dx-row > td[id="dx-col-14"] {
            text-align: left !important;
        }
        .dx-datagrid .dx-row > td[aria-describedby="dx-col-14"] {
            text-align: center !important;
        }
        .activeText {
            color: #2AB7C9;
            background-color: #fff;
            padding-left: 6px;
            padding-right: 6px;
            border-radius: 3px;
        }
        .importButton {
            background-color: #4A6EE2 !important;
            color: #FFF;
            border: 0;
            border-radius: 4px;
            font-size: 12px;
            font-family: 'Poppins', sans-serif;
            font-weight: 500;
        }
        .importButton .dx-button-content {
            padding: 5px 8px;
        }

        .dx-datagrid-header-panel .dx-toolbar {
            padding-left: 4px;
            padding-right: 10px;
        }

        .dx-button-has-text .dx-icon {
            margin-right: 5px;
        }

        #uploadbutton {
            margin-top: 50px;
            margin-right: 20px;
            float: right;
        }

        #companyUploadBtn{
            margin-top: 50px;
            margin-right: 20px;
            float: right;
        }

        
        #allocationUploadBtn{
            margin-top: 50px;
            margin-right: 20px;
            float: right;
        }

        #fileuploader-container {
            border: 1px solid #d3d3d3;
            /*margin: 20px 20px 0 20px;*/
            width:100%;
        }

        #companyContainer{
            border: 1px solid #d3d3d3;
            /*margin: 20px 20px 0 20px;*/
            width:100%;
        }

        #allocationContainer{
            border: 1px solid #d3d3d3;
            /*margin: 20px 20px 0 20px;*/
            width:100%;
        }

        .container_one {
            width: 50%;
            border: 1px solid grey;
            height: 200px;
            padding: 20px;
            margin: 20px;
        }

        .container_two {
            width: 50%;
            border: 1px solid grey;
            height: 200px;
            padding: 20px;
            margin: 20px;
        }

        .container_three {
            width: 50%;
            border: 1px solid grey;
            height: 200px;
            padding: 20px;
            margin: 20px;
        }
    </style>
    <script>
        var baseUrl = ugitConfig.apiBaseUrl;

        $(document).ready(function() {
            
        });

        
    </script>
 <style>
     .svcDashboard_addTicketBtn {
         background: #4A6EE2;
         color: #FFFFFF;
         font-weight: 500;
         font-family: Poppins;
         font-size: 12px;
         text-align: left;
         width: auto;
         /* height: 38px; */
         /* margin-left: 8px; */
         padding: 5px 0px;
     }
 </style>

    <div class="container_one">
        <div><h3>Upload Project Meta Data</h3></div>
        <div id="fileuploader-container">
            <div id="file-uploader">
                   <dx:ASPxUploadControl ID="updExcelFile" runat="server" Width="100%"></dx:ASPxUploadControl>            
            </div>
          </div>
          <div id="uploadbutton">
              <dx:ASPxButton ID="btnImportExcel" runat="server" Text="Import" OnClick="btnImportExcel_Click" CssClass="svcDashboard_addTicketBtn"></dx:ASPxButton>
          </div>
    </div>
    <div><dx:ASPxLabel ID="lblProjectData"  runat="server"></dx:ASPxLabel></div>
    <br />
    <div class="container_two">
        <div><h3>Upload Company Info</h3></div>
        <div id="companyContainer">
            <div id="companyUploader">
                   <dx:ASPxUploadControl ID="updCompanyInfo" runat="server" Width="100%"></dx:ASPxUploadControl>            
            </div>
          </div>
          <div id="companyUploadBtn">
              <dx:ASPxButton ID="btnImportcompanyinfo" runat="server" Text="Import" OnClick="btnImportcompanyinfo_Click" CssClass="svcDashboard_addTicketBtn"></dx:ASPxButton>
          </div>
    </div>
    <div class="container_three">
        <div><h3>Upload Allocation Info</h3></div>
        <div id="allocationContainer">
            <div id="allocationUploader">
                <dx:ASPxUploadControl ID="updAllocationInfo" runat="server" Width="100%"></dx:ASPxUploadControl>
            </div>
        </div>
        <div id="allocationUploadBtn">
            <dx:ASPxButton ID="btnAllocationInfo" runat="server" Text="Import" CssClass="svcDashboard_addTicketBtn" OnClick="btnAllocationInfo_Click"></dx:ASPxButton>
        </div>
    </div>
    <div>
        <dx:ASPxButton ID="btnUpdateResourceToProjectAllocInfo" runat="server" Text="Update Resource To Projects" CssClass="svcDashboard_addTicketBtn" OnClick="btnUpdateResourceToProjectAllocInfo_Click" ToolTip="Utility to Assign Resource to User Columns in Projects, Opportunities" ></dx:ASPxButton>
    </div>
    <div><dx:ASPxLabel ID="lblcompanyinfo"  runat="server"></dx:ASPxLabel></div>
</asp:Content>