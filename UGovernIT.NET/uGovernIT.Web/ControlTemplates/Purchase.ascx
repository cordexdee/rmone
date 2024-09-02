<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Purchase.ascx.cs" Inherits="uGovernIT.Web.Purchase" %>


<script type="text/javascript">
  
 </script>


<div class="col-md-12 col-sm-12 col-xs-12 noPadding purchaseContainer">
    <div class="row">
        <div class="col-md-4 col-sm-4 hidden-xs"></div>
        <div class="col-md-3 col-sm-3 col-xs-12" style="padding-top:18px; padding-bottom:35px;">
            <div class="buy-container">
                <div class="buyContainer-wrap">
                    <div class="buyContainer-header">
                        <img src="../Content/Images/Service-Prime-Logo.svg" class="buy-servicePrimeLogo"/>
                    </div>
                    <div class="buyContainer-footer">
                        
                          <asp:Button runat="server" Text="Cancel"  ID="btnCancel" CssClass="AspSecondary-blueBtn" OnClick="btnCancel_Click"/>
                        <asp:Button runat="server" Text="Learn More"  ID="btnpurchase" OnClick="btnpurchase_Click" CssClass="AspPrimary-blueBtn"/>
                        <div class="successMsg-wrap">
                            <asp:Label Text="Your Request has been submitted succesfully. Helpdesk team will get back to you!" runat="server" ID="lblmsg" Visible="false"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div> 
</div>

            
