<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/master/AnonymousMaster.Master" CodeBehind="DataMigration.aspx.cs" Inherits="uGovernIT.Web.Account.DataMigration" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>


<asp:Content ID="MainContent" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <!-- Latest compiled and minified CSS -->
        <!-- Latest compiled and minified CSS -->
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />

    <!-- jQuery library -->
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>

    <!-- Popper JS -->
    <script src="https://cdnjs.cloudflare.com/ajax/libs/popper.js/1.16.0/umd/popper.min.js"></script>

    <!-- Latest compiled JavaScript -->
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <link href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.10.2/css/all.min.css" rel="stylesheet" />

    <style>
        .migration-template{
            margin: 0 auto;
            width:1000px;
            padding-top:50px;
            padding-bottom:30px;
        }
        .row{
            margin:0;
        }
        .page-title{
            float:left;
            width:100%;
        }
        .inputBox{
            width:100%;
            border-radius:4px;
            border:1px solid #808080;
            padding:10px;
        }
        label{
            color:#4A6EE2;
            font-weight:400;
        }
        .marginTop-15{
            margin-top:20px;
        }
        .sec-title{
            padding-left:15px;
            font-weight:500;
            color:#232323;
            font-size:18px;
            margin-top:35px;
        }
        .fright{
            float:right;
        }
        .btn-blue-primary{
            background: #4A6EE2;
            color: #FFF;
            border: 1px solid #4A6EE2 ;
            border-radius: 4px;
            padding: 5px 13px 5px 13px;
            font-size: 12px;
            font-weight: 400;
            margin-left:10px;
        }
        .btn-blue-secondary{
            background: #FFF;
            color: #4A6EE2;
            border: 1px solid #4A6EE2 ;
            border-radius: 4px;
            padding: 5px 13px 5px 13px;
            font-size: 12px;
            font-weight: 400;
        }
        .hr-title{
            width: 100px;
            float: left;
            margin-top: 5px;
            margin-bottom: 48px;
            border-top: 5px solid #4A6EE3;
            margin-left: 0;
        }
        [type="radio"]:checked,
        [type="radio"]:not(:checked) {
            position: absolute;
            left: -9999px;
        }
        [type="radio"]:checked + label,
        [type="radio"]:not(:checked) + label
        {
            position: relative;
            padding-left: 28px;
            cursor: pointer;
            line-height: 20px;
            display: inline-block;
            color: #666;
        }
        [type="radio"]:checked + label:before,
        [type="radio"]:not(:checked) + label:before {
            content: '';
            position: absolute;
            left: 0;
            top: 0;
            width: 18px;
            height: 18px;
            border: 1px solid #ddd;
            border-radius: 100%;
            background: #fff;
        }
        [type="radio"]:checked + label:after,
        [type="radio"]:not(:checked) + label:after {
            content: '';
            width: 10px;
            height: 10px;
            background: #4A6EE2;
            position: absolute;
            top: 4px;
            left: 4px;
            border-radius: 100%;
            -webkit-transition: all 0.2s ease;
            transition: all 0.2s ease;
        }
        [type="radio"]:not(:checked) + label:after {
            opacity: 0;
            -webkit-transform: scale(0);
            transform: scale(0);
        }
        [type="radio"]:checked + label:after {
            opacity: 1;
            -webkit-transform: scale(1);
            transform: scale(1);
        }
        .panel-custom{
            border-color: #d1d1d1;
            box-shadow:none;
        }
        .panel-custom .panel-heading{
            background: #fafafa;
        }
        .panelTitle i{
            float:right;
        }
        /*.chkBox{
            display: block;
            margin-bottom: 15px;
        }*/
        .chkBox input {
            padding: 0;
            height: initial;
            width: initial;
            margin-bottom: 0;
            display: none;
            cursor: pointer;
        }

        .chkBox label {
            position: relative;
            cursor: pointer;
        }

        .chkBox label:before {
            content:'';
            -webkit-appearance: none;
            background-color: transparent;
            border: 2px solid #0079bf;
            box-shadow: 0 1px 2px rgba(0, 0, 0, 0.05), inset 0px -15px 10px -12px rgba(0, 0, 0, 0.05);
            padding: 7px;
            display: inline-block;
            position: relative;
            vertical-align: middle;
            cursor: pointer;
            margin-right: 5px;
        }

        .chkBox input:checked + label:after {
            content: '';
            display: block;
            position: absolute;
            top: 4px;
            left: 6px;
            width: 6px;
            height: 10px;
            border: solid #0079bf;
            border-width: 0 2px 2px 0;
            transform: rotate(45deg);
        }

        .mandatory {
            color: Red;
        }
    </style>
   <script type="text/javascript">
       $(function () {
           showhide();
           $('.clsmigrationddl').change(function () {
                showhide();
           });
       });

       function showhide()
       {
           if ($('.clsmigrationddl').val() == 'dtd') {
               $('.clsSrcTargetTenant').show();
               $('.clsspenable').hide();
           }
           else if ($('.clsmigrationddl').val() == 'std') {
               $('.clsSrcTargetTenant').hide();
               $('.clsspenable').show();
           }
       }
   </script>
    <div class="migration-template">
        <div class="row">
            <h3 class="page-title">Migration Utilities</h3>
            <hr class="hr-title" />
        </div>
        <div class="row">
            <div class="col-md-6 col-sm-6 col-xs-12">
                <input type="radio" id="showform" name="showform" checked="checked"/>
                <label for="showform">Default Template</label>
            </div>
            <div class="col-md-6 col-sm-6 col-xs-12">
                 <input type="radio" id="uploadtemplate" name="uploadtemplate" />
                 <label for="uploadtemplate">Upload</label>
            </div>
        </div>
        <div class="marginTop-15" id="showFormContent">
            <div class="panel-group">
              <%--Gernral section start--%>
              <div class="panel panel-custom">
                <div class="panel-heading">
                  <h1 class="panel-title">
                    <a href="#general" data-toggle="collapse" class="panelTitle">General
                        <i class="fas fa-angle-down"></i>
                    </a>
                  </h1>
                </div>
                <div class="collapse show" id="general">
                  <div class="panel-body">
                    <!-- Form Start -->
                    <div class="form-group">
                        <div class="row marginTop-15">
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <label>Migration</label>
                                <asp:DropDownList ID="ddlMigrationOptions" runat="server" AutoPostBack="false" CssClass="clsmigrationddl inputBox">
                                    <asp:ListItem Value="dtd">.Net to .Net</asp:ListItem>
                                    <asp:ListItem Value="std">Share Point to .Net</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </div>
                        <div class="row marginTop-15">
                            <div class="clsSrcTargetTenant col-md-6 col-sm-6 col-xs-12">
                                <label>Source Tenant<b class="mandatory"> *</b></label>
                                <asp:DropDownList ID="ddlSourceTenants" runat="server" AutoPostBack="false" CssClass="inputBox"></asp:DropDownList>
                            </div>
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                 <label>Target Tenant<b class="mandatory"> *</b></label>
                                 <asp:TextBox ID="txtTargetTenant" runat="server" CssClass="inputBox" ></asp:TextBox>
                            </div>
                        </div>
                         
                         <div class="row">
                            <h5 class="sec-title">Source</h5>
                            <div class="clsSrcTargetTenant col-md-12 col-sm-12 col-xs-12">
                                <label>Database Connection string<b class="mandatory"> *</b></label>
                                <asp:TextBox ID="txtSourceDbConnString" runat="server" CssClass="inputBox" ></asp:TextBox>
                            </div>
                            <div class="clsSrcTargetTenant col-md-12 col-sm-12 col-xs-12">
                                <label>Tenant Connection string<b class="mandatory"> *</b></label>
                                <asp:TextBox ID="txtSourceTenantDbConnString" runat="server" CssClass="inputBox" ></asp:TextBox>
                            </div>
                             <div class="clsspenable col-md-12 col-sm-12 col-xs-12">
                                <label>Url<b class="mandatory"> *</b></label>
                                <asp:TextBox ID="txtSPUrl" runat="server" CssClass="inputBox" ></asp:TextBox>
                            </div>
                             <div class="clsspenable col-md-12 col-sm-12 col-xs-12">
                                <label>User Name<b class="mandatory"> *</b></label>
                                <asp:TextBox ID="txtSPUserName" runat="server" CssClass="inputBox" ></asp:TextBox>
                            </div>
                              <div class="clsspenable col-md-12 col-sm-12 col-xs-12">
                                <label>Password<b class="mandatory"> *</b></label>
                                <asp:TextBox ID="txtSPPassword" runat="server" TextMode="Password" CssClass="inputBox" ></asp:TextBox>
                            </div>
                              <div class="clsspenable col-md-12 col-sm-12 col-xs-12">
                                <label>Domain<b class="mandatory"> *</b></label>
                                <asp:TextBox ID="txtSPDomain" runat="server" CssClass="inputBox" ></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <h5 class="sec-title">Target</h5>
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <label>Database Connection string<b class="mandatory"> *</b></label>
                                <asp:TextBox ID="txtTargetDbConnString" runat="server" CssClass="inputBox" ></asp:TextBox>
                            </div>
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <label>Tenant Connection string<b class="mandatory"> *</b></label>
                                <asp:TextBox ID="txtTargetTenantDbConnString" runat="server" CssClass="inputBox" ></asp:TextBox>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-4">
                                 <label>User Name<b class="mandatory"> *</b></label>
                                 <asp:TextBox ID="txtUsername" runat="server" CssClass="inputBox" ></asp:TextBox>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-4">
                                 <label>Password<b class="mandatory"> *</b></label>
                                 <asp:TextBox ID="txtPassword" runat="server" CssClass="inputBox" ></asp:TextBox>
                            </div>
                            <div class="col-md-4 col-sm-4 col-xs-4">
                                 <label>User Email<b class="mandatory"> *</b></label>
                                 <asp:TextBox ID="txtUserEmail" runat="server" CssClass="inputBox" ></asp:TextBox>
                            </div>
                        </div>
                    </div>
                <!-- Form End -->
              </div>
            </div>
          </div>

        <div class="panel panel-custom">
                <div class="panel-heading">
                  <h1 class="panel-title">
                    <a href="#admincofig" data-toggle="collapse" class="panelTitle">Admin Configuration
                          <i class="fas fa-angle-down"></i>
                    </a>
                  </h1>
                </div>
                <div class="collapse" id="admincofig">
                    <div class="panel-body">
                    <!-- Form Start -->
                    <asp:Panel ID="pnlConfig" runat="server"></asp:Panel>
                    
                    <!-- Form End -->
                </div>
            </div>
        </div>
         
        <div class="panel panel-custom">
            <div class="panel-heading">
                <h1 class="panel-title">
                <a href="#module" data-toggle="collapse" class="panelTitle">Modules
                        <i class="fas fa-angle-down"></i>
                </a>
                </h1>
            </div>
            <div class="collapse" id="module">
                <div class="panel-body">
                <!-- Form Start -->
                <asp:Panel ID="pnlModules" runat="server"></asp:Panel>
                    
                <!-- Form End -->
                </div>
            </div>
        </div>
    
                            
        </div>        
            <div class="row marginTop-15">
                <div class="col-md-12 col-sm-12 col-xs-12 marginTop-15">
                    <div class="fright">
                        <button class="btn-blue-secondary btn">Cancel</button>
                        <asp:Button ID="btnMigrateFromForm" runat="server" Text="Migrate" CssClass="btn-blue-primary btn" OnClick="btnMigrateFromForm_Click" />
                    </div>
                </div>
            </div>
        </div>
        <div class="marginTop-15" id="showUploadTemplate">
            <div class="row">
                <div class="col-md-6 col-sm-6 col-xs-12">
                    <label>Migration</label>
                    <asp:DropDownList ID="ddlMigrationOptionsFileUpload" runat="server" AutoPostBack="false" CssClass="inputBox">
	                    <asp:ListItem Value="dtd">.Net to .Net</asp:ListItem>
	                    <asp:ListItem Value="std">Share Point to .Net</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <label for="showform">Upload Template<b class="mandatory"> *</b></label>
                    <%--<input type="file" id="myFile" name="filename" />--%>
                <asp:FileUpload ID="DMFileUpload" runat="server" multiple="false" />
                <asp:RegularExpressionValidator ID="rvDMFileUpload" runat="server" ControlToValidate="DMFileUpload"
                    ErrorMessage="Only .json format are allowed." ForeColor="Red"
                    ValidationExpression="([a-zA-Z0-9\s_\\.\-:])+(.json|.JSON)$" Display="Dynamic"
                    ValidationGroup="savechanges" SetFocusOnError="true"></asp:RegularExpressionValidator>
                </div>
            </div>
            <div class="row marginTop-15">
                <div class="col-md-12 col-sm-12 col-xs-12 marginTop-15">
                    <div class="fright">
                        <button class="btn-blue-secondary btn">Cancel</button>            
                        <asp:Button ID="btnMigrateFromFile" runat="server" Text="Migrate" CssClass="btn-blue-primary btn" OnClick="btnMigrateFromFile_Click" />
                    </div>
                </div>
            </div>
    </div>
</div>
    <script type="text/javascript">
        document.getElementById("showform").addEventListener("click", formchecked);
        document.getElementById("uploadtemplate").addEventListener("click", uplodTempate);
        document.getElementById("showUploadTemplate").setAttribute("style", "display:none");
        function formchecked() {
            if (document.getElementById("showform").checked) {
                document.getElementById("uploadtemplate").checked = false;
                document.getElementById("showFormContent").setAttribute("style", "display:block");
                document.getElementById("showUploadTemplate").setAttribute("style", "display:none");
            } 
        }

        function uplodTempate() {
            if (document.getElementById("uploadtemplate").checked) {
                document.getElementById("showform").checked = false;
                document.getElementById("showFormContent").setAttribute("style", "display:none");
                document.getElementById("showUploadTemplate").setAttribute("style", "display:block");
            } 
        }

        $(document).ready(function () {

            $("#chkSelectAllConfig").change(function () {
                if (this.checked) {
                    $("input:checkbox[name=chkGrpConfig]").each(function () {
                        this.checked = true;
                    });
                } else {
                    $("input:checkbox[name=chkGrpConfig]").each(function () {
                        this.checked = false;
                    });
                }
            });

            $("input:checkbox[name=chkGrpConfig]").click(function () {
                if ($(this).is(":checked")) {
                    var isAllChecked = 0;

                    $("input:checkbox[name=chkGrpConfig]").each(function () {
                        if (!this.checked)
                            isAllChecked = 1;
                    });

                    if (isAllChecked == 0) {
                        $("#chkSelectAllConfig").prop("checked", true);
                    }
                }
                else {
                    $("#chkSelectAllConfig").prop("checked", false);
                }
            });


            $("#chkSelectAllModules").change(function () {
                if (this.checked) {
                    $("input:checkbox[name=chkGrpModules]").each(function () {
                        this.checked = true;
                    });
                } else {
                    $("input:checkbox[name=chkGrpModules]").each(function () {
                        this.checked = false;
                    });
                }
            });

            $("input:checkbox[name=chkGrpModules]").click(function () {
                if ($(this).is(":checked")) {
                    var isAllChecked = 0;

                    $("input:checkbox[name=chkGrpModules]").each(function () {
                        if (!this.checked)
                            isAllChecked = 1;
                    });

                    if (isAllChecked == 0) {
                        $("#chkSelectAllModules").prop("checked", true);
                    }
                }
                else {
                    $("#chkSelectAllModules").prop("checked", false);
                }
            });
        });

    </script>
</asp:Content>

