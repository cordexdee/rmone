<%@ Page Language="C#" AutoEventWireup="true"  MasterPageFile="~/master/AnonymousMaster.Master" CodeBehind="MigrationTemplate.aspx.cs" Inherits="uGovernIT.Web.Account.MigrationTemplate" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Register Src="~/Account/OpenAuthProviders.ascx" TagPrefix="uc" TagName="OpenAuthProviders" %>
<%@ Import Namespace="uGovernIT.Utility" %>


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

    <style data-v="<%=UGITUtility.AssemblyVersion %>">
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
    </style>
   
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
                                <label>Source Tenant</label>
                                 <select class="inputBox">
                                    <option>Option 1</option>
                                    <option>Option 2</option>
                                </select>
                            </div>
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                 <label>Target Tenant</label>
                                 <select class="inputBox">
                                    <option>Option 1</option>
                                    <option>Option 2</option>
                                </select>
                            </div>
                        </div>
                         <div class="row marginTop-15">
                            <div class="col-md-6 col-sm-6 col-xs-12">
                                <label>Select</label>
                                <select class="inputBox">
                                    <option>DotNet to DotNet</option>
                                    <option>Share Point to DotNet</option>
                                </select>
                            </div>
                        </div>
                         <div class="row">
                            <h5 class="sec-title">Source</h5>
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <label>Database Connection string</label>
                                <input type="text" class="inputBox"/>
                            </div>
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <label>Tenant Connection string</label>
                                <input type="text" class="inputBox"/>
                            </div>
                        </div>
                        <div class="row">
                            <h5 class="sec-title">Target</h5>
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <label>Database Connection string</label>
                                <input type="text" class="inputBox"/>
                            </div>
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <label>Tenant Connection string</label>
                                <input type="text" class="inputBox"/>
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
                    <a href="#module" data-toggle="collapse" class="panelTitle">Modules
                          <i class="fas fa-angle-down"></i>
                    </a>
                  </h1>
                </div>
                <div class="collapse" id="module">
                  <div class="panel-body">
                    <!-- Form Start -->
                    <div class="form-group">
                        <div class="row marginTop-15">
                            <div class="col-md-4 col-sm-6 col-xs-12 chkBox">
                                <input type="checkbox" id="module1" />
                                <label for="module1">Module 1</label>
                            </div>
                            <div class="col-md-4 col-sm-6 col-xs-12 chkBox">
                                  <input type="checkbox" id="module2" />
                                <label for="module2">Module 2</label>
                            </div>
                            <div class="col-md-4 col-sm-6 col-xs-12 chkBox">
                                  <input type="checkbox" id="module3" />
                                <label for="module3">Module 3</label>
                            </div>
                        </div>
                         <div class="row marginTop-15">
                            <div class="col-md-4 col-sm-6 col-xs-12 chkBox">
                                <input type="checkbox" id="module4" />
                                <label for="module4">Module 4</label>
                            </div>
                            <div class="col-md-4 col-sm-6 col-xs-12 chkBox">
                                  <input type="checkbox" id="module5" />
                                <label for="module5">Module 5</label>
                            </div>
                            <div class="col-md-4 col-sm-6 col-xs-12 chkBox">
                                  <input type="checkbox" id="module6" />
                                <label for="module6">Module 6</label>
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
                    <div class="form-group">
                        <div class="row marginTop-15">
                            <div class="col-md-4 col-sm-6 col-xs-12 chkBox">
                                <input type="checkbox" id="config1" />
                                <label for="config1">Configuration 1</label>
                            </div>
                            <div class="col-md-4 col-sm-6 col-xs-12 chkBox">
                                  <input type="checkbox" id="config2" />
                                <label for="config2">Configuration 2</label>
                            </div>
                            <div class="col-md-4 col-sm-6 col-xs-12 chkBox">
                                  <input type="checkbox" id="config3" />
                                <label for="config3">Configuration 3</label>
                            </div>
                        </div>
                         <div class="row marginTop-15">
                            <div class="col-md-4 col-sm-6 col-xs-12 chkBox">
                                <input type="checkbox" id="config4" />
                                <label for="config4">Configuration 4</label>
                            </div>
                            <div class="col-md-4 col-sm-6 col-xs-12 chkBox">
                                  <input type="checkbox" id="config5" />
                                <label for="config5">Configuration 5</label>
                            </div>
                            <div class="col-md-4 col-sm-6 col-xs-12 chkBox">
                                  <input type="checkbox" id="config6" />
                                <label for="config6">Configuration 6</label>
                            </div>
                        </div>
                    </div>
                <!-- Form End -->
                </div>
            </div>
        </div>
            
        </div>
            
           
           
            <div class="row marginTop-15">
                <div class="col-md-12 col-sm-12 col-xs-12 marginTop-15">
                    <div class="fright">
                        <button class="btn-blue-secondary btn">Cancel</button>
                        <button class="btn-blue-primary btn">Save</button>
                    </div>
                </div>
            </div>
        </div>
        <div class="marginTop-15" id="showUploadTemplate">
            <div class="row">
                <div class="col-md-12 col-sm-12 col-xs-12">
                    <label for="showform">Upload Template</label>
                    <input type="file" id="myFile" name="filename" />
                </div>
            </div>
            <div class="row marginTop-15">
                <div class="col-md-12 col-sm-12 col-xs-12 marginTop-15">
                    <div class="fright">
                        <button class="btn-blue-secondary btn">Cancel</button>
                        <button class="btn-blue-primary btn">Save</button>
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
    </script>
</asp:Content>

