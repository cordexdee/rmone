<%@ Control Language="C#" AutoEventWireup="true"  CodeBehind="TenantInfoSuperAdmin.ascx.cs" Inherits="uGovernIT.Web.TenantInfoSuperAdmin" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .homeGrid_dataRow td a {
        float: none;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
       
    function Deleteconfirmation(obj, AccountId) {
        /*var result = DevExpress.ui.dialog.confirm('Delete action will delete all client data. Are you sure you want to delete client ' + companyName + '?', 'Delete Tenant');*/
        var result = DevExpress.ui.dialog.confirm('Are you sure you want to delete the tenant "' + AccountId + '"? This will delete the tenant and all the data in it.', 'Delete Tenant');
        result.done(function (dialogResult) {

            if (dialogResult) {
                
                var resultConfirm = DevExpress.ui.dialog.confirm('Are you sure you want to delete tenant "' + AccountId + '"?', 'Delete Tenant');

                resultConfirm.done(function (dialogResultConfirm) {

                    if (dialogResultConfirm) {                        
                        adUserLoading.Show();
                        __doPostBack(obj.name, "OnClick");
                        return false;
                    }
                    else {
                        adUserLoading.Hide();
                       return false;
                    }
                });
            }
            else {
               return false;
            }
        });
        return false;
    }

    
    function resetConfirmation(obj, companyName) {

        var result = DevExpress.ui.dialog.confirm('Do you want to reset password ?', 'Reset Password');

        result.done(function (dialogResult) {

            if (dialogResult) {
                
                //var resultConfirm = DevExpress.ui.dialog.confirm('Are you sure you want to delete client ' + companyName + '?', 'Delete Client');

                //resultConfirm.done(function (dialogResultConfirm) {

                    //if (dialogResultConfirm) {                        
                        adUserLoading.Show();
                        __doPostBack(obj.name, "OnClick");
                        return false;
                    //}
                    //else {
                        
                    //}
               // });
            }
            else {

                adUserLoading.Hide();
                     return false;
               //return false;
            }
        });
        return false;
    }

</script>
<script data-v="<%=UGITUtility.AssemblyVersion %>">
    function UpdateGridHeight() {
        superAdminGrid.SetHeight(0);
        var containerHeight = ASPxClientUtils.GetDocumentClientHeight();
        if (document.body.scrollHeight > containerHeight)
            containerHeight = document.body.scrollHeight;
        superAdminGrid.SetHeight(containerHeight);
    }
    window.addEventListener('resize', function (evt) {
        if (!ASPxClientUtils.androidPlatform)
            return;
        var activeElement = document.activeElement;
        if (activeElement && (activeElement.tagName === "INPUT" || activeElement.tagName === "TEXTAREA") && activeElement.scrollIntoViewIfNeeded)
            window.setTimeout(function () { activeElement.scrollIntoViewIfNeeded(); }, 0);
    });

    function clientProfile() {

    }
</script>

<div class="row">
    <div class="col-md-6 col-sm-6 col-xs-12" style="padding-top:10px;"> 
        <h7 runat="server" id="hdTenantCreadted" class="tenantCount-title">Tenant Created :</h7>
        <asp:Label runat="server" ID ="lblTenantCreatedCount"></asp:Label>
    </div>
    <div class="col-md-6 col-sm-6 col-xs-12 statusLegend-Container">
        <div class="legendNormal-wrap">
            <div class="legend-normal legendIcon"></div>
            <div class="legendLabel">Low</div>
        </div> 
        <div class="legendHeigh-wrap">
            <div class="legend-High legendIcon"></div>
            <div class="legendLabel">High</div>
        </div> 
        <div class="legendCritical-wrap">
            <div class="legend-critical legendIcon"></div>
            <div class="legendLabel">Critical</div>
        </div> 
    </div>
</div>
<dx:ASPxLoadingPanel ID="adUserLoading" ClientInstanceName="adUserLoading" Modal="True" runat="server" Text="Please Wait..." CssClass="customeLoader"></dx:ASPxLoadingPanel>
    <ContentTemplate>
        <div class="col-md-12 col-sm-12 col-xs-12">
            <dx:ASPxGridView ID="superAdminGrid" runat="server" ClientInstanceName="superAdminGrid" CssClass="customgridview  homeGrid" Width="100%" KeyFieldName="ID"  OnHtmlDataCellPrepared="superAdminGrid_HtmlDataCellPrepared" SettingsPager-PageSize="20">
            <Columns>
                <%--<dx:GridViewCommandColumn ShowSelectCheckbox="true"></dx:GridViewCommandColumn>--%>
                
                <%--<dx:GridViewDataTextColumn FieldName ="CompanyName" Caption="Tenants" >--%>
                <dx:GridViewDataTextColumn FieldName ="AccountId" Caption="Tenants" >
                    <DataItemTemplate>
                        <asp:LinkButton ID="loginToCompany" runat="server" OnClick="loginToCompany_Click" Text='<%# Eval("AccountId") %>' ToolTip="Access Company" CssClass="superAdmin-gridLink" OnClientClick="aspnetForm.target ='_blank';"
                            CommandArgument='<%# string.Format("{0};{1};{2};{3}", Eval("TenantID"),Eval("TicketId"),Eval("IsSelfRegistration"),Eval("IsTenantExist"))%>' >
                        </asp:LinkButton>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn FieldName="IsSelfRegistration" Caption="Registration Type"/>
                <dx:GridViewDataTextColumn FieldName="Trials" Caption="Admin" SettingsHeaderFilter-DateRangeCalendarSettings-EnableMultiSelect="true"/>
                <%--<dx:GridViewDataTextColumn FieldName="CompanyName" Caption="Tenant Name" SettingsHeaderFilter-DateRangeCalendarSettings-EnableMultiSelect="true"/>--%>
                <dx:GridViewDataTextColumn FieldName="Email" Caption ="Email" />
                <dx:GridViewDataDateColumn FieldName="Date Requested">
                    <PropertiesDateEdit DisplayFormatString="MM/dd/yyyy">
                    </PropertiesDateEdit>
                </dx:GridViewDataDateColumn>
                <%--<dx:GridViewDataTextColumn FieldName="Approval By" />--%>
                <dx:GridViewDataDateColumn FieldName="Approved Date" >
                    <PropertiesDateEdit DisplayFormatString="MM/dd/yyyy"></PropertiesDateEdit>
                </dx:GridViewDataDateColumn>
                <dx:GridViewDataTextColumn FieldName="Actions">
                    <DataItemTemplate>
                        <asp:Label ID="lblCurrentRequest" runat="server" Text=""></asp:Label>
                        <img id="onBoardingMail" runat="server" src="~/Content/Images/MailTo16X16.png" alt="Edit" title="Mail" width="18" onserverclick="" visible="false" />
                        <img id="approveRequest" runat="server" src="~/Content/ButtonImages/approve.png" alt="Edit" title="Approve" width="18" onserverclick="" visible="false" />
                        <img id="rejectRequest" runat="server" src="~/Content/ButtonImages/reject.png" alt="Edit" title="Reject" width="18" onserverclick="" visible="false" />
                        <asp:ImageButton ID="onBoardingWorkFlow" runat="server" src="/Content/Images/NewAdmin/onboarding.png" alt="Edit" title="OnBoarding" width="18" OnClick="onBoardingWorkFlow_Click" CommandName="Registration" CommandArgument='<%# string.Format("{0};{1}", Eval("TicketId"), Eval("Email")) %>'  Enabled='<%# Eval("EnableOnboarding")%>'/>
                         <a runat="server" id="stat"   Width="15" ><img src="/Content/Images/statusBlue.png" title="Client Profile" class="superAdmin-gridDelIcon" /></a>
                         <asp:ImageButton CssClass="superAdmin-gridDelIcon" style ="margin-left:0px !important" Width="15" ID="deleteTenant" runat="server" OnInit="deleteTenant_Init" OnClick="DeleteTenant_Click" OnClientClick='<%# String.Format("javascript:return Deleteconfirmation(this,\"{0}\")", Convert.ToString( Eval("AccountId"))) %>' 
                             src="/Content/Images/grayDelete.png" ToolTip="Delete Client"
                             CommandArgument='<%# string.Format("{0};{1};{2};{3}", Eval("TenantID"),Eval("TicketId"),Eval("IsSelfRegistration"),Eval("IsTenantExist"))%>'  />

                         <asp:ImageButton CssClass="superAdmin-gridDelIcon" style ="margin-left:0px !important" Width="15" ID="ResetPassword" runat="server" OnInit="resetPassword_Init" OnClick="reset_Password_Click" OnClientClick='<%# String.Format("javascript:return resetConfirmation(this,\"{0}\")", Convert.ToString( Eval("CompanyName"))) %>' 
                             src="/Content/Images/resetpassword.png" ToolTip="Reset Password"
                             CommandArgument='<%# string.Format("{0};{1};{2};{3}", Eval("TenantID"),Eval("TicketId"),Eval("IsSelfRegistration"),Eval("IsTenantExist"))%>'  />
                         <%--<asp:ImageButton CssClass="superAdmin-gridDelIcon" ID="ImgStatus" runat="server" OnClick="ImgStatus_Click" Width="15" 
                             src="/Content/Images/statusBlue.png"
                             CommandArgument='<%# string.Format("{0}", Eval("TenantID"))%>'  />--%>
                       </DataItemTemplate> 
                </dx:GridViewDataTextColumn>
                 
                <dx:GridViewDataTextColumn  Caption="Tickets" CellStyle-CssClass="numberCell">
                    <DataItemTemplate>
                        <asp:Label ID="lblTicketCount" runat="server" Text='<%#  Eval("TicketCount")%>'  CssClass='<%#  Eval("Class_TicketCount")%>'></asp:Label>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
               
                <dx:GridViewDataTextColumn  Caption="Services" CellStyle-CssClass="numberCell">
                    <DataItemTemplate>
                        <asp:Label ID="lblTicketCount" runat="server" Text='<%# Eval("ServiceCount")%>' CssClass='<%# Eval("Class_ServiceCount")%>'></asp:Label>
                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Visible="false" FieldName="TenantId">
                    <DataItemTemplate>
                        

                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>

                <dx:GridViewDataTextColumn Visible="false">
                    <DataItemTemplate>
                        <img id="delete" runat="server" src="~/Content/Images/redNew_delete.png" alt="Edit" title="Delete" width="18" onserverclick="" />

                    </DataItemTemplate>
                </dx:GridViewDataTextColumn>
                <dx:GridViewDataTextColumn Visible="True"  Caption="Subscribed" Width="3%">
                    <DataItemTemplate>
                        <dx:ASPxPanel runat="server" ID="du" Visible='<%# Eval("IsSubscription") %>'>
                            <PanelCollection>
                                <dx:PanelContent>
                                    <span>
                                        <i class="fas fa-user-check orderPurchase-icon" title="Subscribed"></i>
                                    </span>
                                   <%-- <div class="containerBlink">
		                            <div class="blink"> Apply!
			                        <div class="arrowBlink">
			                            <div class="outerBlink"></div>
			                            <div class="innerBlink"></div>
		  	                        </div>
		                        </div>
	                        </div>--%>
                                </dx:PanelContent>
                            </PanelCollection>
                                    
                        </dx:ASPxPanel>
                        <%--<asp:ImageButton id="Subs" runat="server" src="~/Content/Images/redNew_delete.png" alt="Edit" title="Subscription" width="18" Visible='<%# Eval("IsSubscription") %>' />--%>

                    </DataItemTemplate>
                    </dx:GridViewDataTextColumn>
                
            </Columns>
            <settingsadaptivity adaptivitymode="HideDataCells" allowonlyoneadaptivedetailexpanded="true" ></settingsadaptivity>
            <Settings ShowFooter="false" ShowHeaderFilterButton="true"  />
            <SettingsBehavior AllowSort="false" AllowDragDrop="false" AutoExpandAllGroups="true" />
            <SettingsPopup>
                <HeaderFilter Height="350px" />
            </SettingsPopup>
            <settingscommandbutton>
                <ShowAdaptiveDetailButton ButtonType="Button"   Styles-Style-CssClass="homeGrid_openBTn"></ShowAdaptiveDetailButton>
                <HideAdaptiveDetailButton ButtonType="Button"  Styles-Style-CssClass="homeGrid_closeBTn"></HideAdaptiveDetailButton>
            </settingscommandbutton>
            <Styles AlternatingRow-CssClass="ms-alternatingstrong">
                <Row HorizontalAlign="Center" CssClass="homeGrid_dataRow" ></Row>
                <GroupRow Font-Bold="true" CssClass="homeGrid-groupRow"></GroupRow>
                <Header Font-Bold="true" HorizontalAlign="Center" CssClass="homeGrid_headerColumn"></Header>
                <%--<AlternatingRow CssClass="ms-alternatingstrong"></AlternatingRow>--%>
                <InlineEditCell HorizontalAlign="Center"></InlineEditCell>
            </Styles>

        </dx:ASPxGridView>
            <div class="mt-3">
                <a class="primary-linkBtn" href="/ApplicationRegistrationRequest/tenant" target="_blank">New Tenant</a>
                <a class="primary-linkBtn ml-2" href="/SuperAdmin/PotentialTenants.aspx" target="_blank">Potential Tenants</a>
            </div>

             <script type="text/javascript">
                ASPxClientControl.GetControlCollection().ControlsInitialized.AddHandler(function (s, e) {
                    UpdateGridHeight();
                });
                ASPxClientControl.GetControlCollection().BrowserWindowResized.AddHandler(function (s, e) {
                    UpdateGridHeight();
                });
            </script>
        </div>
    </ContentTemplate>

