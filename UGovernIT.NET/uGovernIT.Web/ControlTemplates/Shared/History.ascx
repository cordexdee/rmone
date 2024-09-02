<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="History.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.Shared.History" %>
<%--<asp:Table runat="server" ID="historyTable" Width="100%">
</asp:Table>--%>

<asp:Repeater runat="server" ID="historyTable">
    <ItemTemplate>  
        <div class="col-md-12 col-sm-12 col-xs-12 history_wrap">	
	        <div class="row comment-wrap">
		        <div class="col-md-2 col-sm-4 col-xs-4 imgPadding">
			        <div class=" history_img">
				        <img src="<%#Eval("Picture")%>" />
				        <label class="history_img_name"><%#Eval("createdBy")%></label>
			        </div>
		        </div>
				   <div class="col-md-2 col-sm-4 col-xs-12 date_time_padding">
			        <div class="history_date_time" title="<%#DateTime.Today.Subtract(DateTime.Parse(DataBinder.Eval(Container.DataItem, "created").ToString())).Days.ToString()%> day(s) ago">
				        <span><%#Eval("created")%></span>
			        </div>
		        </div>
		        <div class="col-md-8 col-sm-4 col-xs-8 comment-padding">
			        <div class="history_data">
				        <p>
                            <%#Eval("entry")%>
				        </p>
			        </div>
		        </div>
		     
	        </div>
        </div>
    </ItemTemplate>
     <AlternatingItemTemplate>
         <div class="col-md-12 col-sm-12 col-xs-12 history_wrap aproval-alternate-row">	
	        <div class="row comment-wrap">
		        <div class="col-md-2 col-sm-4 col-xs-4 imgPadding">
			        <div class=" history_img">
				        <img src="<%#Eval("Picture")%>" />
				        <label class="history_img_name"><%#Eval("createdBy")%></label>
			        </div>
		        </div>
				    <div class="col-md-2 col-sm-4 col-xs-12 date_time_padding">
			        <div class="history_date_time" title="<%#DateTime.Today.Subtract(DateTime.Parse(DataBinder.Eval(Container.DataItem, "created").ToString())).Days.ToString()%> day(s) ago">
				        <span><%#Eval("created")%></span>
			        </div>
		        </div>
		        <div class="col-md-8 col-sm-4 col-xs-8 comment-padding">
			        <div class="history_data">
				        <p>
                            <%#Eval("entry")%>
				        </p>
			        </div>
		        </div>
	        </div>
        </div>
     </AlternatingItemTemplate>
</asp:Repeater>

<asp:Label runat="server" ID="errorMessage" Visible="false"></asp:Label>