<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SetUgitTheme.ascx.cs" Inherits="uGovernIT.Web.SetUgitTheme" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>
<%@ Import Namespace="uGovernIT.Utility" %>

<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .textboxwidth {
        width: 220PX;
    }

    .saveclose {
        float: right;
        margin-top: 20px;
    }

    .ms-formbody {
        background: none repeat scroll 0 0 #E8EDED;
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .card {
        display: block;
        position: relative;
        height: 200px;
    }

        .card img {
            width: 240px;
            display: block;
            margin: auto;
            /* padding: 15px 0; */
            height: 192px;
        }

        .card .info {
            position: absolute;
            top: 0;
            left: 0;
            right: 0;
            bottom: 0;
            color: white;
            background-color: black;
            opacity: 0;
            text-align: center;
            font-size: 15pt;
            transition: all 0.4s ease-in-out;
            -webkit-transition: all 0.4s ease-in-out;
            -moz-transition: all 0.4s ease-in-out;
            -ms-transition: all 0.4s ease-in-out;
            -o-transition: all 0.4s ease-in-out;
        }

        .card:hover .info {
            opacity: 0.8;
        }

    .info .address {
        padding-top: 20px;
        font-size: 10pt;
    }

    .info p {
        margin-top: 45px;
    }

    .card .info p,
    .card .info span {
        opacity: 0;
        -webkit-transition: all 0.3s ease-out;
        -moz-transition: all 0.3s ease-out;
        -ms-transition: all 0.3s ease-out;
        -o-transition: all 0.3s ease-out;
        transition: all 0.3s ease-out;
        -moz-transform: scale(0);
        -webkit-transform: scale(0);
        -o-transform: scale(0);
        -ms-transform: scale(0);
        transform: scale(0);
    }

    .card:hover .info p,
    .card:hover .info span {
        opacity: 1;
        -moz-transform: scale(1);
        -webkit-transform: scale(1);
        -o-transform: scale(1);
        -ms-transform: scale(1);
        transform: scale(1);
    }

    .templateTable {
        border-collapse: collapse;
        width: 100%;
    }

        .templateTable td {
            border: solid 1px #C2D4DA;
            padding: 6px;
        }

            .templateTable td.value {
                font-weight: bold;
            }

    .imageCell {
        width: 160px;
    }

    .setpadding {
        padding-top: 3px !important;
    }
</style>

<dx:ASPxLoadingPanel ID="LoadingPanel" runat="server" Text="Please Wait ..." ClientInstanceName="LoadingPanel" Modal="True">
</dx:ASPxLoadingPanel>
<div style="margin: 20px">
    <div>
        <div style="float: left; margin: 3px 10px 10px 0px; font-size: 11px; font-weight: bold;">
            <asp:Label ID="lblFont" runat="server" Text="Fonts:"></asp:Label>
        </div>
        <div style="float: left">
            <dx:ASPxComboBox Border-BorderStyle="Solid" Border-BorderWidth="1px" Border-BorderColor="#000000" ShowImageInEditBox="true" EncodeHtml="false" ID="cmbFonts" runat="server" DisplayFormatString="dddd" TextFormatString="{1}" Width="300px" ClientInstanceName="cmbFonts"
                ItemStyle-Font-Size="Medium" ItemStyle-CssClass="ddlFonts">
                <ClientSideEvents SelectedIndexChanged="function(s,e){cmbFonts_onChange(s,e);}" Init="function(s,e){cmbFonts_Init(s,e);}" />

            </dx:ASPxComboBox>
        </div>
    </div>
    <div style="clear: both"></div>
    <div style="float: left; margin: 3px 10px 10px 0px; font-size: 11px; font-weight: bold;">
        <asp:Label ID="lblDevexTheme" runat="server" Text="Select Theme:"></asp:Label>
    </div>
    <dx:ASPxCardView ID="ugitThemesCardView" Styles-TitlePanel-Border-BorderColor="#ff0000" Styles-FocusedCard-Border-BorderWidth="10px" KeyFieldName="ThemeName" SettingsBehavior-EnableCustomizationWindow="false" Settings-ShowCustomizationPanel="false" SettingsBehavior-AllowFocusedCard="true" ClientInstanceName="cardView" runat="server" Width="100%" AutoGenerateColumns="true">
        <SettingsPager>

            <SettingsTableLayout ColumnCount="3" RowsPerPage="2" />
        </SettingsPager>


        <CardLayoutProperties Styles-LayoutGroupBox-Border-BorderStyle="Solid" Styles-LayoutGroup-Border-BorderWidth="10px" SettingsItems-ShowCaption="True">


            <SettingsItems ShowCaption="True"></SettingsItems>

            <Styles>
                <LayoutGroupBox>
                    <border borderstyle="Solid"></border>
                </LayoutGroupBox>

                <LayoutGroup>
                    <border borderwidth="10px"></border>
                </LayoutGroup>
            </Styles>


        </CardLayoutProperties>

        <Columns>

            <dx:CardViewColumn FieldName="Photo">
            </dx:CardViewColumn>
            <dx:CardViewColumn FieldName="ThemeName" />
            <dx:CardViewColumn FieldName="DevExTheme" />
            <dx:CardViewColumn FieldName="SPThemeName" />

        </Columns>



        <SettingsBehavior AllowFocusedCard="True"></SettingsBehavior>

        <Templates>
            <Card>

                <datarow>
                <div style="padding: 5px">
                    <table class="templateTable">
                        <tr>
                            <td class="imageCell" >
                                <dx:ASPxImage ID="Photo" Width="312px" Height="190px" runat="server" ImageUrl='<%# Eval("Photo") %>'></dx:ASPxImage>
                            </td>
                        </tr>
                       <tr>
                            <td style="vertical-align: top;">
                                <dx:ASPxLabel ID="lblNotes" Font-Bold="true" Font-Size="14px"  runat="server" Text='<%# Eval("SPThemeName") %>' />
                            </td>
                        </tr>
                       
                    </table>
                </div>
            </datarow>

            </Card>
        </Templates>

        <Styles>


            <FocusedCard>
                <border borderwidth="10px" bordercolor="#0099cc"></border>

            </FocusedCard>


        </Styles>

    </dx:ASPxCardView>
    <div class="saveclose">
        <asp:HiddenField ID="hdnFont" runat="server" />
        <dx:ASPxButton ID="btnSave" runat="server" Text="Save" ToolTip="Save" ValidationGroup="Save" OnClick="btnSave_Click">
            <Image Url="/content/images/save.png"></Image>
            <ClientSideEvents Click="function(s,e){LoadingPanel.Show();}" />
        </dx:ASPxButton>
        <dx:ASPxButton ID="btnCancel" runat="server" Text="Cancel" ToolTip="Cancel" OnClick="btnCancel_Click">
            <Image Url="/content/ButtonImages/cancelwhite.png"></Image>
        </dx:ASPxButton>
        <div>
            &nbsp;
        </div>
    </div>
</div>

<script type="text/javascript" data-v="<%=UGITUtility.AssemblyVersion %>">
    function cmbFonts_onChange(s, e) {
        try {

            var myHidden = document.getElementById('<%=hdnFont.ClientID %>');
            myHidden.value = cmbFonts.GetText();
          <%--  var value = cmbFonts.GetText();
            alert(value);
            var selectedText = cmbFonts.GetSelectedItem().value.split('#')[1];
            var headerText = "Header: " + selectedText.split('-')[0];
            var bodyText = "Body: " + selectedText.split('-')[1];

            $('#<%=hdnFont.ClientID%>').val(value);--%>
            //cmbFonts.SetText(headerText + ", " + bodyText);
        }
        catch (ex) {

        }
    }

    function cmbFonts_Init(s, e) {

        for (var i = 0; i < cmbFonts.listBox.itemsValue.length; i++) {
            var selectedFontFamily = cmbFonts.GetItem(i).text.split('-')[1];
            $($(".ddlFonts").get(i + 1)).css("font-family", selectedFontFamily);
        }
    }
</script>
