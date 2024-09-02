<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TicketHour.ascx.cs" Inherits="uGovernIT.Web.TicketHour" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .pcTicketHourspanel{
        float:left;
        height:100%;
        width:100%;
        padding:10px;
    }
</style>
<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function showTicketHoursPopUp() {
        $("#<%=hdnId.ClientID%>").val('');
        var dates = new Date();
        dtcTicketHoursDate.SetDate(dates);
        txtTicketHours.SetValue("");
        txtResolutionDescription.SetValue("");
        pcTicketHours.Show();
        return false;
    }
    function ShowPopUpID(id, date, hour, desc) {
        $("#<%=hdnId.ClientID%>").val(id);
        var dates = new Date(date);
        dtcTicketHoursDate.SetDate(dates);
        txtTicketHours.SetValue(hour);
        txtResolutionDescription.SetValue(desc);
        pcTicketHours.Show();
      
    }
    function validateTicketHours(groupName)
    {
        var isValid = Page_ClientValidate(groupName);
        if (isValid){
            AddNotification("Updating ..");
            HideAllButtons(); 
        }
        return isValid;
    }

    function addTicketHoursBt_PerformCallback(s, e) {
        var startDate = dtcTicketHoursDate.GetDate();
        var hours = txtTicketHours.GetValue();
        var description = txtResolutionDescription.GetValue();
        var ticketid = "<%=TicketId%>";
        var actualHourid = $("#<%=hdnId.ClientID%>").val();
      
        var dataVar = "{  'StartDate':'" + startDate.toJSON() + "', 'Hours': '" + hours + "', 'Description': '" + description + "', 'TicketID': '" + ticketid + "', 'ActualHourID': '" + actualHourid + "' }";

        $.ajax({
            type: "POST",
            url: "<%= ajaxPageURL %>/AddTicketHours",
            data: dataVar,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (message) {
                var resultJson = $.parseJSON(message);
                if (resultJson.status === 'done') {
                    pcTicketHours.Hide();
                    window.location.reload();
<%--                    var btRefreshPage = $('<%=btRefreshPage.ClientID%>');
                    btRefreshPage.trigger("click");--%>
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                
            }
        });
    }
</script>



<asp:Literal ID="ButtonControl" runat="server"></asp:Literal>

<div style="height: auto; padding:0px; width: 100%;" class="first_tier_nav">
<dx:ASPxPopupControl ID="pcTicketHours"  runat="server" CloseAction="CloseButton" Modal="True" Width="450px" AllowDragging="true" AllowResize="true"
                    ShowFooter="false" ShowHeader="true" CssClass="departmentPopup" HeaderText="Add Resolution" ClientInstanceName="pcTicketHours"
                   EnableViewState="false" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter" EnableHierarchyRecreation="True">
                    <ContentCollection>
                        <dx:PopupControlContentControl ID="PopupControlContentControl15" runat="server">

                                                     <div class="pcTicketHourspanel"  >
                                                        <table style="width: 100%;">
                                                               <tr>
                                                                   <td class="ms-formlabel">
                                                                       <div class="fleft">
                                                                           <asp:HiddenField ID="hdnId" runat="server" />
                                                                    <asp:Label CssClass="pcTicketHours_date" ID="Label3" runat="server" Text="Work Date:"></asp:Label>
                                                                        </div></td>
                                                                <td class="ms-formbody">
                                                                    <div class="fleft">
                                                                          <dx:ASPxDateEdit EditFormat="Date" ID="dtcTicketHoursDate" ClientInstanceName="dtcTicketHoursDate" runat="server" CssClassTextBox="datetimectr datetimectr111"  Width="300px" ></dx:ASPxDateEdit>
                                                                      </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="ms-formlabel">
                                                                    <asp:Label ID="lblTicketHours" runat="server" Text="Actual Hour(s)<b style='color:red;'>*</b>"></asp:Label>
                                                                </td>
                                                                <td  class="ms-formbody">
                                                                   <dx:ASPxSpinEdit runat="server" ID="txtTicketHours" ClientInstanceName="txtTicketHours" Width="300px" MinValue="0" MaxValue="999999"></dx:ASPxSpinEdit>
                                                                    <%--<asp:TextBox runat="server" TextMode="Number" ID="txtTicketHours" ValidationGroup="TicketHours"  Width="300px"  ></asp:TextBox>
                                                                    <asp:RequiredFieldValidator ID="rfvTicketHours" CssClass="errormsg-container" runat="server" ControlToValidate="txtTicketHours" ValidationGroup="TicketHours" ErrorMessage="Please enter Ticket hours" Display="Dynamic"></asp:RequiredFieldValidator>
                                                                    <asp:RangeValidator ID="revTicketHours" CssClass="errormsg-container" runat="server" ControlToValidate="txtTicketHours"
                                                                        ErrorMessage="Only Numbers(0-24) Allowed" Display="Dynamic" ValidationGroup="TicketHours"
                                                                        Type="Double"  MinimumValue="0" MaximumValue="24"></asp:RangeValidator>--%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td  class="ms-formlabel">
                                                                    <asp:Label ID="lblResolutionDescription" runat="server" Text="Description<b style='color:red;'>*</b>" ></asp:Label></td>
                                  
                                                                <td  class="ms-formbody">
                                                                    <dx:ASPxMemo ID="txtResolutionDescription" ClientInstanceName="txtResolutionDescription" runat="server" Width="300px" ValidationSettings-ValidationGroup="TicketHours">
                                                                        <ValidationSettings Display="Dynamic" ErrorDisplayMode="ImageWithText">
                                                                            <RequiredField IsRequired="true" ErrorText="Please enter resolution description" />
                                                                        </ValidationSettings>
                                                                    </dx:ASPxMemo>
                                                                    <%--<asp:TextBox runat="server" ID="txtResolutionDescription" CssClass="txtaddcomment" Width="300px" Columns="52" Rows="9" TextMode="MultiLine" ></asp:TextBox>
                                                                 <asp:RequiredFieldValidator ID="rfvResolutionDescription" CssClass="error" runat="server" ControlToValidate="txtResolutionDescription" ValidationGroup="TicketHours" ErrorMessage="Please enter resolution description" Display="Dynamic"></asp:RequiredFieldValidator>--%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td></td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkNotifyCommentRequestor" runat="server" Text="Notify Requestor" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2" class="buttoncell" >
                                                                    <div style="float:right;">
                                                                        <div style="float:left;" >
                                                                            <dx:ASPxButton ID="addTicketHoursBt" runat="server" AutoPostBack="false" ValidationGroup="TicketHours"  Text="Save" ImagePosition="Right">
                                                                                <Image Url="/Content/images/save.png"></Image>
                                                                                <ClientSideEvents Click="addTicketHoursBt_PerformCallback" />
                                                                            </dx:ASPxButton>
                                                                           </div>
                                                                        <div style="float:left;">
                                                                             <dx:ASPxButton ID="lnkbtn" runat="server"  Text="Cancel" AutoPostBack="true" ImagePosition="Right">
                                                                                <Image Url="/Content/images/cancel.png"></Image>
                                                                                <ClientSideEvents Click="function(s,e){pcTicketHours.Hide();}" />
                                                                            </dx:ASPxButton>
                                                                        </div>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                            </dx:PopupControlContentControl>
                        </ContentCollection>
                      </dx:ASPxPopupControl>
  <div>
  <ugit:ASPxGridView  ID="grid" Width="100%" runat="server" OnCustomCallback="grid_CustomCallback" ClientInstanceName="ActualHoursGrid" AutoGenerateColumns="false" KeyFieldName="ID" OnHtmlDataCellPrepared="dxGvACR_HtmlDataCellPrepared" >
      <Columns>           
          <dx:GridViewDataColumn FieldName="WorkDate" Caption="Work Date" ></dx:GridViewDataColumn>
           <dx:GridViewDataColumn FieldName="StageStep" Caption="Stage"></dx:GridViewDataColumn>
          <dx:GridViewDataColumn FieldName="ResourceUser" Caption="User"></dx:GridViewDataColumn>
           <dx:GridViewDataColumn FieldName="HoursTaken" Caption="Hours Taken"></dx:GridViewDataColumn>
           <dx:GridViewDataColumn FieldName="Comment" Caption="Comment"></dx:GridViewDataColumn>
          
         <dx:GridViewDataTextColumn FieldName="aEdit" Caption="Edit">
            <DataItemTemplate>
               <a id="editLink" runat="server" href=""> 
                   <img id="Imgedit" runat="server" src="~/Content/Images/edit-icon.png"/>
                </a>
            </DataItemTemplate>
        </dx:GridViewDataTextColumn>
           </Columns>
    </ugit:ASPxGridView>
  </div>
    
</div>
