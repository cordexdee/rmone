<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HelpCardDisplay.ascx.cs" Inherits="uGovernIT.Web.ControlTemplates.HelpCardDisplay" %>
<%@ Import Namespace="uGovernIT.Utility" %>

        <div>
             <div class="row">
                  <div class="helpCardPreview flashCard-container" runat="server" id="helpCardPreview">
                  </div>
             </div>                
        </div>   

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .flashCard-container{
		    width: 341px;
		    margin: 0px auto;
		    border: 1px solid #FFF;
		    height: 495px;
		    padding: 10px;
	        box-shadow: 0px 0px 4px #888888;
		}
.flashCard-container img{
			max-width: 100%;
		}

</style>

<script>

</script>
