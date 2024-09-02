<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BusinessStrategy_Viewer.ascx.cs" Inherits="uGovernIT.Report.DxReport.BusinessStrategy_Viewer" %>
<%@ Register Assembly="DevExpress.Web.v22.1, Version=22.1.6.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<%@ Register Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.WebControls" TagPrefix="asp" %>

<%@ Import Namespace="uGovernIT.Utility" %>
<%@ Import Namespace="uGovernIT.Manager" %>


<style type="text/css">
    .projectGroup {
        display: table;
        margin: 0 auto;
    }

    .bordercss {
        border-left: 4px solid rgb(12, 197, 219);
        border-right: 4px solid rgb(12, 197, 219);
        border-bottom: 4px solid rgb(12, 197, 219);
    }

    .setascurve {
        border-top-right-radius: 4em;
        background: #ffffff;
        border-color: #216098;
        cursor: pointer;
    }

    .grouped {
        width: 300px;
    }

    .data-cell {
        margin-bottom: 0px;
        text-align: center;
    }

        .data-cell .gray {
            padding: 4px;
            background-color: #f3f3f3;
        }


        .data-cell .red {
            padding: 4px;
            background-color: #FDCACA;
        }

        .data-cell .green {
            padding: 4px;
            background-color: #a2e6a2;
        }

        .data-cell .yellow {
            padding: 4px;
            background-color: yellow;
        }

    .titlecss {
        font-weight: bold;
        padding: 10px 0px 10px 0px;
        text-align: center;
    }

    .totalrowcss {
        text-align: center;
    }

    .circle-cell {
        fill: #847b7b;
    }

    .circle-text {
        fill: #ffffff;
    }

    .layoutgroup {
        padding: 0px 10px 10px 10px;
        width: 300px;
    }

    .layoutgroup-cell {
        padding: 0px 1px;
    }

    .scrum-title {
        font-weight: bold;
    }

    .backward-icon {
        float: left;
        padding-top: 20px;
        cursor: pointer;
    }

    .child-container {
        cursor: pointer;
        border-color: #f3f3f3;
    }

    .pnlparentcss {
        width: 100%;
        height: 435px;
        padding: 0px 0px 0px 10px;
    }

    .flipblue {
        background-color: rgb(12, 197, 219);
        float: left;
    }

    .flipgray {
        background-color: #f3f3f3;
        padding: 0px;
        float: left;
        width: 100%;
    }

    .flipgreen {
        background-color: #a2e6a2;
    }

    .flipred {
        background-color: #FDCACA;
    }

    .flipyellow {
        background-color: yellow;
    }

    .flipgray > div > span {
        padding: 5px;
        width: 100%;
        float: left;
    }

    .flipblue > span {
        padding: 5px;
        width: 100%;
        float: left;
    }

    .flipalign {
        float: right;
        margin-top: -54px;
        margin-right: 26px;
        cursor: pointer;
    }

    .checkbox-Messagecontainer {
        margin-top: 4px;
        text-align: center;
        width: 100%;
    }

    .checkbox-container {
        display: inline-block;
        margin: 0 auto;
        border: 1px solid;
        width: 360px;
        height: 28px;
        border-color: #f3f3f3;
    }

    .aligneditimage {
        margin-right: 22px !important;
        /*margin-top:4px;*/
    }

    .button-bg {
        padding-bottom: 10px;
    }

    .showhideimage {
        display: none;
    }

    .ms-formbody {
        /*background: none repeat scroll 0 0 #E8EDED;*/
        border-top: 1px solid #A5A5A5;
        padding: 3px 6px 4px;
        vertical-align: top;
    }

    .ms-formlabel {
        text-align: right;
        width: 190px;
        vertical-align: top;
    }

    .ms-long {
        font-family: Verdana,sans-serif;
        font-size: 8pt;
        width: 386px;
    }

    .ms-standardheader {
        text-align: right;
    }

    .checkbox-container label {
        margin-bottom: -3px;
        margin-left: 2px;
    }

    .button-bg {
        color: white;
        float: left;
        margin: 1px;
        padding: 5px;
        cursor: pointer;
        background: #4A6EE2 no-repeat left top;
        border-radius: 5px;
        -webkit-border-radius: 5px;
        border: 1px solid #a9acb5;
    }

        .button-bg:hover {
            background: #4A6EE2 no-repeat left top;
        }
</style>

<script type="text/javascript">
    var currentselectedkey = '';
    var titlearray = new Array();

    function ShowBusinessChildData(s, e) {
        showtickets('bs');
    }

    function ShowInitiativeGroupData(s, e) {
        imgback.SetClientVisible(true);
        currentselectedkey = s.GetCardKey(e.visibleIndex);
        hdninitiativevalues.Set("currentselectedkey", currentselectedkey);
        hdninitiativevalues.Set("currentLevel", "INI");
        s.GetCardValues(e.visibleIndex, 'Title', getFieldsValue);


    }

    function ShowProjectChildData(s, e) {
        showtickets('pro');
    }

    function ShowInitiativeChildData(s, e) {
        showtickets('bi')
    }

    function ShowProjectGroupData(s, e) {
        currentselectedkey = s.GetCardKey(e.visibleIndex);
        hdninitiativevalues.Set("currentselectedkey", currentselectedkey);
        hdninitiativevalues.Set("currentLevel", "PROJ");
        s.GetCardValues(e.visibleIndex, 'Title', getProjectData);
    }

    function openrespectiveproject(projectid, title) {
        if (projectid != "") {
            var pmmurl = '<%=pmmUrl%>';
            var nprUrl = '<%=nprUrl%>';
            var url = "";
            if (pmmurl != "" && nprUrl != "") {
                if (projectid.match('^PMM')) {
                    pmmurl = pmmurl + "?TicketId=" + projectid;
                    url = pmmurl;
                }
                else {
                    nprUrl = nprUrl + "?TicketId=" + projectid;
                    url = nprUrl
                }
                var tickettitle = projectid + ": " + title;
                window.parent.UgitOpenPopupDialog(url, "", tickettitle, 90, 90);

            }
        }
    }

    function openTicket(s, e) {
        var title = s.GetCardKey(e.visibleIndex);

        if (title != "") {
            var nodes = $.parseHTML(title);
            if (nodes.length > 0) {
                var link = nodes[0];
                eval(link.href);
            }
        }

    }

    function GoBackward(s, e) {
        var drildownstr = $('#<%=hdnlabeltext.ClientID%>').val() //$('.scrum-title').html();
        var hdnarraystr = $('#<%=hdnInitiative.ClientID%>').val();
        var bsexist = '<%=IsBusinessStrategyExist%>';
        if (bsexist.toLowerCase() == "false")
            hdninitiativevalues.Set("currentLevel", "INI");
        else
            hdninitiativevalues.Set("currentLevel", "BI");

        if (drildownstr == "") {
            imgback.SetClientVisible(false);
        }
        else {
            var drilarray = drildownstr.split('`#~')
            var hdnarray = hdnarraystr.split('`#~');
            if (drilarray.length > 1) {
                $('.scrum-title').html(drilarray[0].trim());


                if ($('.scrum-title').html().trim() == '')
                    imgback.SetClientVisible(false);
                else
                    imgback.SetClientVisible(true);

                $('#<%=hdnlabeltext.ClientID%>').val(drilarray[0].trim());
                $('#<%=hdnInitiative.ClientID%>').val(hdnarray[0].trim());
                var strategy = drilarray[0].trim();
                crdInitiative.SetClientVisible(true);
                crdInitiativeChilds.SetClientVisible(true);
                crdProjectGroup.SetClientVisible(false);
                innerProjectData.SetClientVisible(false);
                crdInitiative.SetClientVisible(true);
                crdInitiativeChilds.SetClientVisible(true);
                hdninitiativevalues.Set("currentLevel", "INI");
            }
            else if (hdnarray.length == 1) {
                $('.scrum-title').html("");
                $('#<%=hdnInitiative.ClientID%>').val('');
                $('#<%=hdnlabeltext.ClientID%>').val('');
                imgback.SetClientVisible(false);
                crdInitiative.SetClientVisible(false);
                crdInitiativeChilds.SetClientVisible(false);
                crdProjectGroup.SetClientVisible(false);
                innerProjectData.SetClientVisible(false);
                var bsexist = '<%=IsBusinessStrategyExist%>';
                if (bsexist.toLowerCase() == 'false') {
                    crdBusinessStrategy.SetClientVisible(false);
                    crdBusinessStrategyChilds.SetClientVisible(false);
                    crdInitiative.SetClientVisible(true);
                    crdInitiativeChilds.SetClientVisible(true);
                    hdninitiativevalues.Set("currentLevel", "INI");
                }
                else {
                    crdBusinessStrategy.SetClientVisible(true);
                    crdBusinessStrategyChilds.SetClientVisible(true);
                    hdninitiativevalues.Set("currentLevel", "BI");
                }
            }
        }
        ShowHideAddNewIcon();
    }

    function stopeventbubbling(s, e) {
        $(".cssprojectlink").click(function (event) {
            event.stopPropagation();
        });
    }

    function ShowGroupflipview(s, e, c, key) {
        e.htmlEvent.stopPropagation();
        if (hndkeepflipviewtrack.Contains("currentkey")) {
            var storedKey = hndkeepflipviewtrack.properties.dxpcurrentkey;
            if (storedKey == key)
                hndkeepflipviewtrack.Set("currentkey", "");
            else
                hndkeepflipviewtrack.Set("currentkey", key);
        }
        else {
            hndkeepflipviewtrack.Set("currentkey", key);
        }
        c.PerformCallback();

        if (key == "PRO") {
            crdInitiativeChilds.PerformCallback();
        }
    }

    function showtickets(key) {
        var isbsExist = '<%=IsBusinessStrategyExist%>';
        var titleval = $('#<%=hdnInitiative.ClientID%>').val();
        var filter = 'ALL';
        filter = 'businessstrategy_' + filter;
        var bs = '';
        var bsIn = '';
        if (key == 'bi') {
            bs = titleval.split('`#~')[0];

            if (!bs.startsWith('<') && !bs.endsWith('>')) {
                filter = bs.trim();
            }
            else
                filter = 'unassigned';
            filter = 'businessinitiative_' + filter;
        }
        else if (key == 'pro') {
            filter = '';
            if (isbsExist.toLowerCase() == 'true') {
                bs = titleval.split('`#~')[0];
                bsIn = titleval.split('`#~')[1];
            }
            else {
                bsIn = titleval.split('`#~')[0];
            }
            if (!bs.startsWith('<') && !bs.endsWith('>')) {
                filter = bs.trim();
            }
            else
                filter = 'unassigned';

            if (!bsIn.startsWith('<') && !bsIn.endsWith('>')) {
                filter = filter + '_' + bsIn.trim();
            }
            else
                filter = filter + '_unassigned';

            filter = 'project_' + filter
        }

        var url = '<%=filterUrl%>';
        url = url + '&filter=' + filter;

        var titlearray = $('#<%=hdnlabeltext.ClientID%>').val().split('`#~');

        if (titlearray.length > 0)
            title = titlearray.join(' > ');
        else
            title = 'All';

        var datafilter = 'CurrentProjects';
        if ($('#<%=FilterCheckBox_pa.ClientID%>').is(":checked"))
            datafilter = datafilter + '_PendingApprovalNPRs';
        if ($('#<%=FilterCheckBox_apr.ClientID%>').is(":checked"))
            datafilter = datafilter + '_ApprovedNPRs';
        url = url + '&datafilter=' + datafilter + "&BSExist=" + isbsExist;
        window.parent.UgitOpenPopupDialog(url, "", title, 90, 90);

    }

    $(function () {
        var bsexist = '<%=IsBusinessStrategyExist%>';

        if (bsexist.toLowerCase() == 'false') {

            crdBusinessStrategyChilds.SetClientVisible(false);
            crdBusinessStrategy.SetClientVisible(false);
            crdInitiative.SetClientVisible(true);
            crdInitiativeChilds.SetClientVisible(true);
        }
        $('#<%=hdnInitiative.ClientID%>').val("");

        $("#<%=FilterCheckBox_pa.ClientID%>").on('click', function () {
            var $cb = $(this);
            hdnkeepprojectstate.Set("FilterCheckBox_pa", $cb.is(':checked'));
        <%if (IsBusinessStrategyExist)
    { %>
            Reset();
        <%}
    else
    {%>
            Reset();
        <%}%>
        });

        $("#<%=FilterCheckBox_apr.ClientID%>").on('click', function () {
            var $cb = $(this);
            hdnkeepprojectstate.Set("FilterCheckBox_apr", $cb.is(':checked'));
        <%if (IsBusinessStrategyExist)
    { %>
            Reset();
        <%}
    else
    {%>
            Reset();
        <%}%>
        });

        $("#<%=FilterCheckBox_cp.ClientID%>").on('click', function () {
            var $cb = $(this);
            hdnkeepprojectstate.Set("FilterCheckBox_cp", $cb.is(':checked'));
        <%if (IsBusinessStrategyExist)
    { %>
            Reset();
        <%}
    else
    {%>
            Reset();
        <%}%>
        });

        ShowHideAddNewIcon();
    });

    function hidepopup(s, e) {
        addnewstrategy.Hide();
    }

    function showpopup() {
        addnewstrategy.Show();
        return false;
    }

    function ShowHideAddNewIcon() {
        var isuserauthorise = '<%=isPMO%>';

        if (isuserauthorise.toLowerCase() == 'false') {
            $('.addiconbusinessstrategy').hide();
            $('.addiconinitiative').hide();
            return;
        }

        $('.addiconbusinessstrategy').hide();
        $('.addiconinitiative').hide();
        var bsexist = '<%=IsBusinessStrategyExist%>';
        var hdnfield = $('#<%=hdnInitiative.ClientID%>').val();
        var hdnlabeltext = $('#<%=hdnlabeltext.ClientID%>').val().split('`#~');

        if (bsexist.toLowerCase() == 'false') {
            if (hdnfield == "") {
                $('.addiconbusinessstrategy').hide();
                $('.addiconinitiative').show();
            }
            else {
                $('.addiconbusinessstrategy').hide();
                $('.addiconinitiative').hide();
            }
        }
        else if (hdnlabeltext != "") {
            if (hdnlabeltext.length == 1) {
                $('.addiconbusinessstrategy').hide();
                $('.addiconinitiative').show();
            }
            else if (hdnlabeltext.length == 2) {
                $('.addiconbusinessstrategy').hide();
                $('.addiconinitiative').hide();
            }
        }
        else if (hdnlabeltext.length == 1 && hdnlabeltext[0] == '') {
            $('.addiconbusinessstrategy').show();
            $('.addiconinitiative').hide();
        }
        else if (hdnlabeltext.length == 1 && hdnlabeltext[0] != '') {
            $('.addiconbusinessstrategy').hide();
            $('.addiconinitiative').show();
        }
        else if (hdnlabeltext.length == 2) {
            $('.addiconbusinessstrategy').hide();
            $('.addiconinitiative').hide();
        }

        //Show/Hide AddNew button on filter change also
        var enableFilter = false;
        if ($("#<%=FilterCheckBox_pa.ClientID%>").is(":checked"))
            enableFilter = true;
        if ($("#<%=FilterCheckBox_apr.ClientID%>").is(":checked"))
            enableFilter = true;
        if ($("#<%=FilterCheckBox_cp.ClientID%>").is(":checked"))
            enableFilter = true;

        if (!enableFilter) {
            $('.addiconbusinessstrategy').hide();
            $('.addiconinitiative').hide();
            $('#<%=hdnlabeltext.ClientID%>').val('');
        }
    }

    function doSetVisibility(parent, child) {
        if (parent.cpCommonhas) {
            child.SetClientVisible(false);
        }
        else {
            child.SetClientVisible(true);
        }
    }

    function getFieldsValue(selectedFieldValue) {
        if (selectedFieldValue != null && selectedFieldValue.length > 0) {
            var performKey = '';
            var key = selectedFieldValue;
            if (key.startsWith('<') && key.endsWith('>')) {
                key = key.replace('<', ' ').trim();
                key = key.replace('>', ' ').trim();
                if (key.toLowerCase() == 'unassigned') {
                    currentselectedkey = '';
                    performKey = 'unassigned';
                }
                $('.scrum-title').html(key);
            }
            else {
                $('.scrum-title').html(key);
            }

            $('#<%=hdnlabeltext.ClientID%>').val(key);
            $('.scrum-title').html($('#<%=hdnlabeltext.ClientID%>').val());
            $('#<%=hdnInitiative.ClientID%>').val(currentselectedkey);

            if (performKey != '' && currentselectedkey == '')
                $('#<%=hdnInitiative.ClientID%>').val(performKey);

            crdBusinessStrategyChilds.SetClientVisible(false);
            crdBusinessStrategy.SetClientVisible(false);
            crdInitiative.SetClientVisible(true);
            crdInitiativeChilds.SetClientVisible(true);
            crdProjectGroup.SetClientVisible(false);
            innerProjectData.SetClientVisible(false);
            crdInitiative.PerformCallback(currentselectedkey);
            crdInitiativeChilds.PerformCallback(currentselectedkey);
            ShowHideAddNewIcon();
        }
        else {
            return;
        }
    }

    function getProjectData(selectedValues) {
        if (selectedValues != null && selectedValues.length > 0) {
            var scrumtitle = $('#<%=hdnlabeltext.ClientID%>').val();
            var hdnvaluetitle = $('#<%=hdnInitiative.ClientID%>').val();

            var performkey = '';
            var key = selectedValues;

            if (key.startsWith('<') && key.endsWith('>')) {
                key = key.replace('<', ' ').trim();
                key = key.replace('>', ' ').trim();

                if (key.toLowerCase() == 'unassigned') {
                    currentselectedkey = '';
                    performkey = 'unassigned';
                }
                if (scrumtitle.trim() == '') {
                    scrumtitle = key;
                }
                else
                    scrumtitle = scrumtitle + "`#~" + key;
            }
            else if (scrumtitle.trim() == '')
                scrumtitle = key;
            else
                scrumtitle = scrumtitle + "`#~" + key;

            $('#<%=hdnlabeltext.ClientID%>').val(scrumtitle);
            var labeltext = $('#<%=hdnlabeltext.ClientID%>').val();

            $('.scrum-title').html(labeltext.split('`#~').join('/'));

            if (hdnvaluetitle.trim() == '')
                hdnvaluetitle = currentselectedkey;
            else
                hdnvaluetitle = hdnvaluetitle + "`#~" + currentselectedkey;

            if (hdnvaluetitle.trim() == '' && performkey != '')
                hdnvaluetitle = performkey;


            $('#<%=hdnInitiative.ClientID%>').val(hdnvaluetitle);
            imgback.SetClientVisible(true);
            crdBusinessStrategyChilds.SetClientVisible(false);
            crdBusinessStrategy.SetClientVisible(false);
            crdInitiative.SetClientVisible(false);
            crdInitiativeChilds.SetClientVisible(false);
            crdProjectGroup.SetClientVisible(true);
            innerProjectData.SetClientVisible(true);

            crdProjectGroup.PerformCallback(currentselectedkey);
            innerProjectData.PerformCallback(currentselectedkey);
            ShowHideAddNewIcon();
        }
        else {
            return;
        }
    }

    function refreshcard(s, e) {
        if (s.cpPerformCallback) {
            s.cpPerformCallback = false;
            s.PerformCallback();
        }
        else if (s.cpPerformDeleteCallback) {
            s.cpPerformDeleteCallback = false;
            crdBusinessStrategyChilds.PerformCallback();
            crdBusinessStrategy.PerformCallback();
        }
        else {
            return false;
        }
    }

    function refreshcardinitiative(s, e) {
        hdninitiativevalues.Set("currentselectedkey", currentselectedkey);
        if (s.cpPerformCallback) {
            s.cpPerformCallback = false;
            s.PerformCallback();
        }
        else if (s.cpPerformDeleteCallback) {
            s.cpPerformDeleteCallback = false;
            crdInitiative.PerformCallback();
            s.PerformCallback();
        }
        else if (s.cpPerformCancelEditCallback) {
            s.cpPerformCancelEditCallback = false;
            s.PerformCallback();
        }
        else if (s.cpPerformStartEditCallback) {
            s.cpPerformStartEditCallback = false;
            //s.PerformCallback();
        }
        else {
            return false;
        }
    }

    function savevalues() {
        var bsvalue = $('#<%=ddlBusinessStrategy.ClientID%>').val();
        var inititle = $('#<%=txtTitle.ClientID%>').val();
        var projectnote = $('#<%=txtProjectNote.ClientID%>').val();
        var chkdelete = $('#<%=chkDeleted.ClientID%>').is(':checked');

        $('#<%=ddlBusinessStrategy.ClientID%>').val('');
        $('#<%=txtTitle.ClientID%>').val('');
        $('#<%=txtProjectNote.ClientID%>').val('');

        hdninitiativevalues.Set("bsvalue", bsvalue);
        hdninitiativevalues.Set("inititle", inititle);
        hdninitiativevalues.Set("projectnote", projectnote);
        hdninitiativevalues.Set("chkdelete", chkdelete);

        if (inititle != '') {
            aspxclientcallback.PerformCallback();
            addnewinitiativepopup.Hide();
        }
        else {
            alert('Please enter title');
            return false;
        }
    }

    function refreshinitiativeandchild(s, e) {
        if (s.cptitle)
            alert(s.cptitle + " created successfully");
        if (s.cpForInitiativeRefresh) {


            crdInitiativeChilds.PerformCallback();
            crdInitiative.PerformCallback();
        }
        else {
            return;
        }
    }

    $(document).on('ready', function () {
        //Show/Hide AddNew button
        ShowHideAddNewIcon();
    });

    function Reset() {
        var enableFilter = false;
        if ($("#<%=FilterCheckBox_pa.ClientID%>").is(":checked"))
            enableFilter = true;
        if ($("#<%=FilterCheckBox_apr.ClientID%>").is(":checked"))
            enableFilter = true;
        if ($("#<%=FilterCheckBox_cp.ClientID%>").is(":checked"))
            enableFilter = true;

        if (!enableFilter) {
            $('.addiconbusinessstrategy').hide();
            $('.addiconinitiative').hide();
            $('#<%=hdnlabeltext.ClientID%>').val('');
            $('#<%=hdnInitiative.ClientID%>').val('');
            $('.scrum-title').html('');
            imgback.SetClientVisible(false);
            crdInitiative.SetClientVisible(false);
            crdInitiativeChilds.SetClientVisible(false);
            crdProjectGroup.SetClientVisible(false);
            innerProjectData.SetClientVisible(false);
            crdBusinessStrategy.SetClientVisible(false);
            crdBusinessStrategyChilds.SetClientVisible(false);
            ShowHideAddNewIcon();
            return;
        }

        $('#<%=hdnlabeltext.ClientID%>').val('');
        $('#<%=hdnInitiative.ClientID%>').val('');
        $('.scrum-title').html('');
        imgback.SetClientVisible(false);
        crdInitiative.SetClientVisible(false);
        crdInitiativeChilds.SetClientVisible(false);
        crdProjectGroup.SetClientVisible(false);
        innerProjectData.SetClientVisible(false);
        ShowHideAddNewIcon();

    <%if (IsBusinessStrategyExist)
    { %>
        crdBusinessStrategy.SetClientVisible(true);
        crdBusinessStrategyChilds.SetClientVisible(true);
        crdBusinessStrategy.PerformCallback();
    <%}
    else
    {%>
        crdBusinessStrategy.SetClientVisible(false);
        crdBusinessStrategyChilds.SetClientVisible(false);
        crdInitiative.SetClientVisible(true);
        crdInitiativeChilds.SetClientVisible(true);
        crdInitiative.PerformCallback();
        crdInitiativeChilds.PerformCallback();
    <%}%>
    }

    function saveBIvalues() {
        addnewstrategy.PerformCallback();
    }

    function ShowBICreationMsg(s, e) {
        if (s.cpBsCreatedMessage) {
            alert(s.cpBsCreatedMessage + ' created successfully.');
            addnewstrategy.Hide();
            return;
        }
    }
</script>
<dx:ASPxHiddenField ID="hdnkeepNewCreatedbs" runat="server" ClientInstanceName="hdnkeepNewCreatedbs"></dx:ASPxHiddenField>
<dx:ASPxHiddenField ID="hdnkeepprojectstate" runat="server" ClientInstanceName="hdnkeepprojectstate"></dx:ASPxHiddenField>
<dx:ASPxLoadingPanel ID="pnlLoading" ClientInstanceName="pnlLoading" runat="server" Text="Please Wait.." Modal="true"></dx:ASPxLoadingPanel>
<dx:ASPxHiddenField ID="hdninitiativevalues" ClientInstanceName="hdninitiativevalues" runat="server"></dx:ASPxHiddenField>
<asp:Panel ID="pnlProject" runat="server" CssClass="pnlparentcss" Width="100%" Height="435px">
    <dx:ASPxHiddenField ID="hdncontainer" ClientInstanceName="hdncontainer" runat="server"></dx:ASPxHiddenField>
    <div class="checkbox-Messagecontainer">
        <div class="fleft" style="position: fixed;">
            <asp:Label ID="lbltitle" runat="server" CssClass="scrum-title"></asp:Label>
        </div>
        <div class="checkbox-container">
            <asp:HiddenField ID="hdnkeepFileters" runat="server" />
            <table style="margin-left:22px">
                <tr>
                    <td style="padding: 5px;">
                        <asp:CheckBox runat="server" ID="FilterCheckBox_pa" CssClass="itg-view" Text="Pending Approval" AutoPostBack="false" GroupName="ITGRadio"
                            Checked="false" />
                    </td>
                    <td style="padding: 5px;">
                        <asp:CheckBox runat="server" ID="FilterCheckBox_apr" CssClass="itg-view" Text="Ready To Start" GroupName="ITGRadio"
                            Checked="false" AutoPostBack="false" />
                    </td>
                    <td style="padding: 5px;">
                        <asp:CheckBox runat="server" ID="FilterCheckBox_cp" CssClass="itg-view" Text="In Progress" AutoPostBack="false" GroupName="ITGRadio"
                            Checked="true" />
                    </td>
                </tr>
            </table>
        </div>
    </div>

    <div>
        <div class="backward-icon">
            <dx:ASPxImage ID="imgback" ClientInstanceName="imgback" runat="server" ImageUrl="/Content/images/Back.png">
                <ClientSideEvents Click="function(s,e){GoBackward(s,e);}" />
            </dx:ASPxImage>
        </div>
        <div>
            <asp:HiddenField runat="server" ID="hdnInitiative" />
            <asp:HiddenField runat="server" ID="hdnlabeltext" />
            <asp:HiddenField runat="server" ID="hdnShowdHide" />

            <dx:ASPxHiddenField ID="hndkeepflipviewtrack" runat="server" ClientInstanceName="hndkeepflipviewtrack"></dx:ASPxHiddenField>
            <div style="width: 100%;">
                <a href="#BusinessStrategry" id="businessStrategryTag"></a>
                <a id="BusinessStrategry"></a>
                <div id="divBusinessStrategy" runat="server" class="projectGroup">
                    <dx:ASPxCardView ID="crdBusinessStrategy" runat="server" OnCustomCallback="crdBusinessStrategy_CustomCallback" OnCustomColumnDisplayText="crdBusinessStrategy_CustomColumnDisplayText" Border-BorderWidth="0px" Width="100%"
                        ClientInstanceName="crdBusinessStrategy" KeyFieldName="Title" SettingsPager-ShowEmptyCards="false">
                        <SettingsPager AlwaysShowPager="false" SettingsTableLayout-RowsPerPage="1" SettingsTableLayout-ColumnCount="1"></SettingsPager>
                        <ClientSideEvents CardClick="function(s,e){ShowBusinessChildData(s,e);}" EndCallback="function(s,e){ crdBusinessStrategyChilds.PerformCallback();}" />
                        <Styles>
                            <Card CssClass="setascurve grouped">
                            </Card>

                        </Styles>
                        <Columns>
                            <dx:CardViewColumn FieldName="BusinessStrategies">
                                <DataItemTemplate>
                                    <div>
                                        <div>
                                            <div>
                                                <asp:Panel ID="pnlBusinessStrategiesCount" runat="server" OnLoad="pnlBusinessStrategiesCount_Load">
                                                    <svg width="60" height="60">
                                                        <circle cx="30" cy="30" r="20" class="circle-cell"></circle>
                                                        <text id="txtBusinessStrategiesTotal" text-anchor="middle" x="30" y="34" class="circle-text" runat="server"></text>
                                                    </svg>
                                                </asp:Panel>

                                            </div>
                                            <div><span id="spnBusinessStrategies" runat="server"><%=bsTitle %></span></div>
                                        </div>
                                        <div class="flipalign">
                                            <dx:ASPxImage runat="server" ID="imgflip" Cursor="pointer" ClientInstanceName="imgflip" ImageUrl="/Content/Images/refresh-icon.png">
                                                <ClientSideEvents Click="function(s,e){ShowGroupflipview(s,e, crdBusinessStrategy,'BI');}" />
                                            </dx:ASPxImage>
                                        </div>
                                    </div>
                                </DataItemTemplate>
                            </dx:CardViewColumn>
                            <dx:CardViewTextColumn FieldName="TotalAmount" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0:C}K Total Amount" Caption="Total"></dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="AmountLeft" Caption="Amount Left" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0:C}K Left"></dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="AllMonth" Caption="Months" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0} Months Total"></dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="MonthLeft" Caption="Months Left" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0} Months Left"></dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="Issues" Caption="Issues" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0} Issues"></dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="RiskLevel" Caption="Risk Level" PropertiesTextEdit-EncodeHtml="false"></dx:CardViewTextColumn>
                        </Columns>
                        <CardLayoutProperties ColCount="2">
                            <Items>
                                <dx:CardViewColumnLayoutItem ColumnName="BusinessStrategies" CssClass=" titlecss totalrowcss" ShowCaption="False" ColSpan="2"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem CssClass="data-cell" ColumnName="TotalAmount" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem CssClass="data-cell" ColumnName="AmountLeft" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem CssClass="data-cell" ColumnName="AllMonth" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem CssClass="data-cell" ColumnName="MonthLeft" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem CssClass="data-cell" ColumnName="Issues" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem CssClass="data-cell" ColumnName="RiskLevel" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                            </Items>
                            <Styles LayoutGroup-CssClass="layoutgroup" LayoutGroup-Cell-CssClass="layoutgroup-cell">
                            </Styles>
                        </CardLayoutProperties>
                    </dx:ASPxCardView>
                </div>
            </div>

            <a href="#BusinessStrategries" id="businessStrategriesTag"></a>
            <a id="BusinessStrategries"></a>

            <div id="divBusinessStrategyExpand" runat="server" class="projectGroup">
                <div class="addiconbusinessstrategy">
                    <%if (IsBusinessStrategyExist)
                        { %>
                    <b style="text-align: right;">
                        <a onclick="addnewstrategy.Show()" title="Add Business Strategy">
                            <span class="button-bg" style="height: 28px;">
                                <b style="float: left; font-weight: normal; top: 0;">Add</b>
                                <i style="float: left; position: relative; top: -1px; left: 2px">
                                    <img src="/Content/images/add_icon.png" style="border: none;" title="" alt="" />
                                </i>

                            </span>
                        </a>
                    </b>

                    <%} %>
                </div>
                <div style="clear: both;"></div>
                <div style="float: left;">
                    <dx:ASPxCardView ID="crdBusinessStrategyChilds" CssClass="child-container" OnCommandButtonInitialize="crdBusinessStrategyChilds_CommandButtonInitialize" OnCustomCallback="crdBusinessStrategyChilds_CustomCallback"
                        OnCustomColumnDisplayText="crdBusinessStrategyChilds_CustomColumnDisplayText" OnCardDeleting="crdBusinessStrategyChilds_CardDeleting" runat="server" OnAfterPerformCallback="crdBusinessStrategyChilds_AfterPerformCallback"
                        ClientInstanceName="crdBusinessStrategyChilds" OnCardUpdating="crdBusinessStrategyChilds_CardUpdating"
                        EnableViewState="false" KeyFieldName="BusinessId" OnPageIndexChanged="crdBusinessStrategyChilds_PageIndexChanged"
                        SettingsPager-ShowEmptyCards="false">
                        <SettingsBehavior AllowSort="false" ConfirmDelete="true" />
                        <SettingsText ConfirmDelete="Are you sure you want to delete this?" />
                        <SettingsEditing Mode="PopupEditForm">
                        </SettingsEditing>
                        <SettingsPopup>
                            <EditForm Width="500px" />
                        </SettingsPopup>
                        <SettingsPager AlwaysShowPager="false" SettingsTableLayout-RowsPerPage="1" SettingsTableLayout-ColumnCount="4"></SettingsPager>
                        <SettingsCommandButton>
                            <EditButton ButtonType="Image" Image-ToolTip="Edit Business Strategy">
                                <Image Url="/Content/images/edit-icon.png">
                                </Image>
                            </EditButton>
                            <DeleteButton Image-ToolTip="Delete Business Strategy">
                                <Image Url="/Content/images/delete-icon-new.png"></Image>
                            </DeleteButton>

                        </SettingsCommandButton>
                        <ClientSideEvents CardClick="function(s,e){ShowInitiativeGroupData(s,e);}" EndCallback="function(s,e){ refreshcard(s,e);}" />
                        <Styles>
                            <EmptyCard Wrap="True"></EmptyCard>
                            <Card Height="100px" CssClass="setascurve"></Card>
                        </Styles>
                        <Columns>
                            <dx:CardViewTextColumn FieldName="Title" Caption="Project Description" Settings-AllowSort="True" SortOrder="Ascending" Settings-SortMode="DisplayText">
                            </dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="BusinessStrategyDescription" Caption="Description">
                            </dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="TotalAmount" PropertiesTextEdit-DisplayFormatString="{0:C}K Total Amount" PropertiesTextEdit-EncodeHtml="false" Caption="Total"></dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="AmountLeft" Caption="Amount Left" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0:C}K Left"></dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="AllMonth" Caption="Months" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0} Months Total"></dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="MonthLeft" Caption="Months Left" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0} Months Left"></dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="Issues" Caption="Issues" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0} Issues"></dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="RiskLevel" Caption="Risk Level" PropertiesTextEdit-EncodeHtml="false"></dx:CardViewTextColumn>
                        </Columns>
                        <CardLayoutProperties ColCount="2">
                            <Items>
                                <dx:CardViewColumnLayoutItem ColumnName="Title" ShowCaption="False" CssClass=" titlecss" HorizontalAlign="Center" ColSpan="1">
                                </dx:CardViewColumnLayoutItem>
                                <dx:CardViewCommandLayoutItem ShowEditButton="true" ShowDeleteButton="true" ButtonRenderMode="Image" ColSpan="1" CssClass="aligneditimage" HorizontalAlign="Right" />
                                <dx:CardViewColumnLayoutItem ColumnName="TotalAmount" CssClass="data-cell" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem ColumnName="AmountLeft" CssClass="data-cell" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem ColumnName="AllMonth" CssClass="data-cell" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem ColumnName="MonthLeft" CssClass="data-cell" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem ColumnName="Issues" CssClass="data-cell" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem ColumnName="RiskLevel" CssClass="data-cell" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                                <%--<dx:EditModeCommandLayoutItem HorizontalAlign="Right" ColSpan="2" />--%>
                            </Items>
                            <Styles LayoutGroup-CssClass="layoutgroup" LayoutGroup-Cell-CssClass="layoutgroup-cell">
                            </Styles>
                        </CardLayoutProperties>
                        <EditFormLayoutProperties ColCount="2">
                            <Items>
                                <dx:CardViewColumnLayoutItem ColumnName="Title" Caption="Title" ShowCaption="true" CssClass=" titlecss" ColSpan="2">
                                    <Template>
                                        <asp:Panel ID="pnlStartegyTitle" runat="server" OnLoad="pnlStartegyTitle_Load">
                                            <dx:ASPxTextBox ID="txtbstitle" runat="server" ClientInstanceName="txtbstitle" Width="450px">
                                                <ValidationSettings ValidateOnLeave="true" Display="Dynamic">
                                                    <RequiredField IsRequired="true" ErrorText="Please enter title" />
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </asp:Panel>
                                    </Template>
                                </dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem ColumnName="BusinessStrategyDescription" Caption="Description" ShowCaption="true" CssClass=" titlecss" ColSpan="2">
                                    <Template>
                                        <asp:Panel ID="pnlStartegy" runat="server" OnLoad="pnlStartegy_Load">
                                            <dx:ASPxMemo ID="aspxBsDescription" runat="server" Width="450px"></dx:ASPxMemo>
                                        </asp:Panel>
                                    </Template>
                                </dx:CardViewColumnLayoutItem>
                                <dx:EditModeCommandLayoutItem HorizontalAlign="Right" ColSpan="2" />
                            </Items>
                        </EditFormLayoutProperties>
                    </dx:ASPxCardView>
                </div>
            </div>

            <div style="width: 100%;">
                <a href="#ProjectInitivate" id="projectInitivateTag"></a>
                <a id="ProjectInitivate"></a>
                <div id="diivInitiative" runat="server" class="projectGroup">
                    <dx:ASPxCardView ID="crdInitiative" runat="server" ClientVisible="false" OnCustomCallback="crdInitiative_CustomCallback" OnCustomColumnDisplayText="crdInitiative_CustomColumnDisplayText" Border-BorderWidth="0px" Width="100%"
                        ClientInstanceName="crdInitiative" KeyFieldName="Title" SettingsPager-ShowEmptyCards="false">
                        <ClientSideEvents CardClick="function(s,e){ShowInitiativeChildData(s,e);}" EndCallback="function(s,e){doSetVisibility(crdInitiative,crdInitiativeChilds);}" />
                        <SettingsPager AlwaysShowPager="false" SettingsTableLayout-RowsPerPage="1" SettingsTableLayout-ColumnCount="1"></SettingsPager>
                        <Styles>
                            <Card CssClass="setascurve grouped">
                            </Card>
                        </Styles>
                        <Columns>
                            <dx:CardViewColumn FieldName="Initiative">
                                <DataItemTemplate>
                                    <div>
                                        <div>
                                            <asp:Panel ID="pnltoprow" runat="server" OnLoad="pnltoprow_Load">
                                                <div>
                                                    <asp:Panel ID="pnlInitiative" runat="server" OnLoad="pnlInitiative_Load">
                                                        <svg width="60" height="60">
                                                            <circle cx="30" cy="30" r="20" class="circle-cell"></circle>
                                                            <text id="txtInitiative" text-anchor="middle" x="30" y="34" class="circle-text" runat="server"></text>
                                                        </svg>
                                                    </asp:Panel>

                                                </div>
                                                <div><span id="spnInitiative" class="inisettext" runat="server"></span></div>
                                            </asp:Panel>
                                        </div>
                                        <div class="flipalign">
                                            <dx:ASPxImage runat="server" ID="imgflip" Cursor="pointer" ClientInstanceName="imgflip" ImageUrl="/Content/Images/refresh-icon.png">
                                                <ClientSideEvents Click="function(s,e){ShowGroupflipview(s, e, crdInitiative,'PRO');}" />
                                            </dx:ASPxImage>
                                        </div>
                                    </div>

                                </DataItemTemplate>
                            </dx:CardViewColumn>
                            <dx:CardViewTextColumn FieldName="TotalAmount" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0:C}K Total Amount" Caption="Total"></dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="AmountLeft" Caption="Amount Left" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0:C}K Left"></dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="AllMonth" Caption="Months" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0} Months Total"></dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="MonthLeft" Caption="Months Left" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0} Months Left"></dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="Issues" Caption="Issues" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0} Issues"></dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="RiskLevel" Caption="Risk Level" PropertiesTextEdit-EncodeHtml="false"></dx:CardViewTextColumn>
                        </Columns>
                        <CardLayoutProperties ColCount="2">
                            <Items>
                                <dx:CardViewColumnLayoutItem ColumnName="Initiative" ShowCaption="False" CssClass=" titlecss totalrowcss" ColSpan="2"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem CssClass="data-cell" ColumnName="TotalAmount" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem CssClass="data-cell" ColumnName="AmountLeft" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem CssClass="data-cell" ColumnName="AllMonth" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem CssClass="data-cell" ColumnName="MonthLeft" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem CssClass="data-cell" ColumnName="Issues" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem CssClass="data-cell" ColumnName="RiskLevel" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                            </Items>
                            <Styles LayoutGroup-CssClass="layoutgroup" LayoutGroup-Cell-CssClass="layoutgroup-cell">
                            </Styles>
                        </CardLayoutProperties>
                    </dx:ASPxCardView>

                </div>
            </div>
            <div id="div1" runat="server" class="projectGroup">
                <a href="#ProjectInitivates" id="projectInitivatesTag"></a>
                <a id="ProjectInitivates"></a>
                <div class="addiconinitiative">
                    <b style="text-align: right;">
                        <a onclick="addnewinitiativepopup.Show();" title="Add Initiative">
                            <%--<asp:LinkButton ID="lnkaddinitiative" OnClientClick="addnewinitiativepopup.Show();" runat="server" Text="&nbsp;&nbsp;Add New Item&nbsp;&nbsp;" ToolTip="Add Initiative">--%>
                            <span class="button-bg">
                                <b style="float: left; font-weight: normal;">Add</b>
                                <i style="float: left; position: relative; top: -1px; left: 2px">
                                    <img src="/Content/images/add_icon.png" style="border: none;" title="" alt="" />
                                    <%--<asp:ImageButton ID="btAddBusinessStrategy" OnClick="btAddBusinessStrategy_Click" runat="server" ToolTip="Add Business Strategy" ImageUrl="/_layouts/15/images/ugovernit/add_icon.png" />--%>
                                </i>
                            </span>
                        </a>
                    </b>
                </div>
                <div style="clear: both"></div>
                <div style="float: left;">
                    <dx:ASPxCardView ID="crdInitiativeChilds" CssClass="child-container" OnCommandButtonInitialize="crdInitiativeChilds_CommandButtonInitialize"
                        OnCardDeleting="crdInitiativeChilds_CardDeleting"
                        OnAfterPerformCallback="crdInitiativeChilds_AfterPerformCallback" ClientVisible="false"
                        OnCustomColumnDisplayText="crdInitiativeChilds_CustomColumnDisplayText"
                        OnCustomCallback="crdInitiativeChilds_CustomCallback" runat="server"
                        ClientInstanceName="crdInitiativeChilds" KeyFieldName="InitiativeId" OnCardUpdating="crdInitiativeChilds_CardUpdating"
                        EnableViewState="false" OnPageIndexChanged="crdInitiativeChilds_PageIndexChanged" SettingsPager-ShowEmptyCards="false">
                        <ClientSideEvents CardClick="function(s,e){ShowProjectGroupData(s,e);}" EndCallback="function(s,e){ refreshcardinitiative(s,e);}" />
                        <SettingsPager AlwaysShowPager="false" SettingsTableLayout-RowsPerPage="1" SettingsTableLayout-ColumnCount="4"></SettingsPager>
                        <SettingsBehavior AllowSort="false" ConfirmDelete="true" />
                        <SettingsText ConfirmDelete="Are you sure you want to delete this?" />
                        <SettingsEditing Mode="PopupEditForm"></SettingsEditing>
                        <Styles>
                            <EmptyCard Wrap="True"></EmptyCard>
                            <Card Height="100px" CssClass="setascurve"></Card>
                        </Styles>
                        <SettingsPopup>
                            <EditForm Width="500px" />
                        </SettingsPopup>
                        <SettingsCommandButton>
                            <EditButton ButtonType="Image" Image-ToolTip="Edit Initiative">
                                <Image Url="/Content/images/edit-icon.png">
                                </Image>
                            </EditButton>
                            <DeleteButton Image-ToolTip="Delete Initiative">
                                <Image Url="/Content/images/delete-icon-new.png"></Image>
                            </DeleteButton>
                        </SettingsCommandButton>
                        <Columns>
                            <dx:CardViewTextColumn FieldName="Title" Caption="Project Description" Settings-AllowSort="True" SortOrder="Ascending" Settings-SortMode="DisplayText"></dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="InitiativeDescription" Caption="Description">
                            </dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="TotalAmount" PropertiesTextEdit-DisplayFormatString="{0:C}K Total Amount" PropertiesTextEdit-EncodeHtml="false" Caption="Total"></dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="AmountLeft" Caption="Amount Left" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0:C}K Left"></dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="AllMonth" Caption="Months" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0} Months Total"></dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="MonthLeft" Caption="Months Left" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0} Months Left"></dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="Issues" Caption="Issues" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0} Issues"></dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="RiskLevel" Caption="Risk Level" PropertiesTextEdit-EncodeHtml="false"></dx:CardViewTextColumn>
                        </Columns>
                        <CardLayoutProperties ColCount="2">
                            <Items>
                                <dx:CardViewColumnLayoutItem ColumnName="Title" ShowCaption="False" CssClass=" titlecss" ColSpan="1"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewCommandLayoutItem ShowEditButton="true" ShowDeleteButton="true" ButtonRenderMode="Image" ColSpan="1" CssClass="aligneditimage" HorizontalAlign="Right" />
                                <dx:CardViewColumnLayoutItem ColumnName="TotalAmount" CssClass="data-cell" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem ColumnName="AmountLeft" CssClass="data-cell" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem ColumnName="AllMonth" CssClass="data-cell" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem ColumnName="MonthLeft" CssClass="data-cell" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem ColumnName="Issues" CssClass="data-cell" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem ColumnName="RiskLevel" CssClass="data-cell" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                                <%--<dx:EditModeCommandLayoutItem HorizontalAlign="Right" ColSpan="2" />--%>
                            </Items>
                            <Styles LayoutGroup-CssClass="layoutgroup" LayoutGroup-Cell-CssClass="layoutgroup-cell">
                            </Styles>
                        </CardLayoutProperties>
                        <EditFormLayoutProperties ColCount="2">
                            <Items>
                                <dx:CardViewColumnLayoutItem ColumnName="Title" Caption="Title" RequiredMarkDisplayMode="Required" ShowCaption="true" CssClass=" titlecss" ColSpan="2">
                                    <Template>
                                        <asp:Panel ID="pnlinitiativetitle" runat="server" OnLoad="pnlinitiativetitle_Load">
                                            <dx:ASPxTextBox ID="aspxinitiativetitle" runat="server" Width="450px">
                                                <ValidationSettings ValidateOnLeave="true" Display="Dynamic">
                                                    <RequiredField IsRequired="true" ErrorText="Please enter title" />
                                                </ValidationSettings>
                                            </dx:ASPxTextBox>
                                        </asp:Panel>
                                    </Template>
                                </dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem ColumnName="InitiativeDescription" Caption="Description" ShowCaption="true" CssClass=" titlecss" ColSpan="2">
                                    <Template>
                                        <asp:Panel ID="pnlinitiative" runat="server" OnLoad="pnlinitiative_Load1">
                                            <dx:ASPxMemo ID="aspxinitiativeDescription" runat="server" Width="450px"></dx:ASPxMemo>
                                        </asp:Panel>
                                    </Template>
                                </dx:CardViewColumnLayoutItem>
                                <dx:EditModeCommandLayoutItem HorizontalAlign="Right" ColSpan="2" />
                            </Items>
                        </EditFormLayoutProperties>
                    </dx:ASPxCardView>
                </div>
            </div>
            <div style="width: 100%;">
                <a href="#ProjectGroup" id="projectGroupTag"></a>
                <a id="ProjectGroup"></a>
                <div id="divProjectGroup" runat="server" class="projectGroup">
                    <dx:ASPxCardView ID="crdProjectGroup" ClientVisible="false" runat="server" OnCustomCallback="crdProjectGroup_CustomCallback" OnCustomColumnDisplayText="crdProjectGroup_CustomColumnDisplayText" Border-BorderWidth="0px" Width="100%"
                        ClientInstanceName="crdProjectGroup" KeyFieldName="Title" SettingsPager-ShowEmptyCards="false">
                        <SettingsPager AlwaysShowPager="false" SettingsTableLayout-RowsPerPage="1" SettingsTableLayout-ColumnCount="1"></SettingsPager>
                        <ClientSideEvents CardClick="function(s,e){ShowProjectChildData(s,e);}" EndCallback="function(s,e){doSetVisibility(crdProjectGroup,innerProjectData);}" />
                        <Styles>
                            <Card CssClass="setascurve grouped">
                            </Card>
                        </Styles>
                        <Columns>
                            <dx:CardViewColumn FieldName="TotalProject">
                                <DataItemTemplate>
                                    <div>
                                        <div>
                                            <div>
                                                <asp:Panel ID="pnlTotalProCount" runat="server" OnLoad="pnlTotalProCount_Load">
                                                    <svg width="60" height="60">
                                                        <circle cx="30" cy="30" r="20" class="circle-cell"></circle>
                                                        <text id="txtProjectTotal" text-anchor="middle" x="30" y="34" class="circle-text" runat="server"></text>
                                                    </svg>
                                                </asp:Panel>

                                            </div>
                                            <div><span id="spnProject" runat="server">Projects</span></div>
                                        </div>
                                        <div class="flipalign">
                                            <dx:ASPxImage runat="server" Cursor="pointer" ID="imgflip" ClientInstanceName="imgflip" ImageUrl="/Content/Images/refresh-icon.png">
                                                <ClientSideEvents Click="function(s,e){ShowGroupflipview(s, e,crdProjectGroup,'PROJ');}" />
                                            </dx:ASPxImage>
                                        </div>
                                    </div>

                                </DataItemTemplate>
                            </dx:CardViewColumn>
                            <dx:CardViewTextColumn FieldName="TotalAmount" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0:C}K Total Amount" Caption="Total"></dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="AmountLeft" Caption="Amount Left" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0:C}K Left"></dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="AllMonth" Caption="Months" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0} Months Total"></dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="MonthLeft" Caption="Months Left" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0} Months Left"></dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="Issues" Caption="Issues" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0} Issues"></dx:CardViewTextColumn>
                            <dx:CardViewTextColumn FieldName="RiskLevel" Caption="Risk Level" PropertiesTextEdit-EncodeHtml="false"></dx:CardViewTextColumn>
                        </Columns>
                        <CardLayoutProperties ColCount="2">
                            <Items>
                                <dx:CardViewColumnLayoutItem ColumnName="TotalProject" CssClass=" titlecss totalrowcss" ShowCaption="False" ColSpan="2"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem CssClass="data-cell" ColumnName="TotalAmount" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem CssClass="data-cell" ColumnName="AmountLeft" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem CssClass="data-cell" ColumnName="AllMonth" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem CssClass="data-cell" ColumnName="MonthLeft" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem CssClass="data-cell" ColumnName="Issues" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                                <dx:CardViewColumnLayoutItem CssClass="data-cell" ColumnName="RiskLevel" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                            </Items>
                            <Styles LayoutGroup-CssClass="layoutgroup" LayoutGroup-Cell-CssClass="layoutgroup-cell">
                            </Styles>
                        </CardLayoutProperties>
                    </dx:ASPxCardView>

                </div>
            </div>
            <div id="divprojectChild" runat="server" class="projectGroup">
                <a href="#ProjectList" id="projectListTag"></a>
                <a id="ProjectList"></a>
                <dx:ASPxCardView ID="innerProjectData" CssClass="child-container" ClientVisible="false" OnCustomColumnDisplayText="innerProjectData_CustomColumnDisplayText" OnCustomCallback="innerProjectData_CustomCallback" runat="server"
                    ClientInstanceName="innerProjectData"
                    EnableViewState="false" KeyFieldName="Title" OnPageIndexChanged="innerProjectData_PageIndexChanged" SettingsPager-ShowEmptyCards="false">
                    <ClientSideEvents CardClick="function(s,e){openTicket(s,e);}" EndCallback="function(s,e){stopeventbubbling(s,e);}" />
                    <SettingsPager AlwaysShowPager="false" SettingsTableLayout-RowsPerPage="1" SettingsTableLayout-ColumnCount="4"></SettingsPager>
                    <SettingsBehavior AllowSort="true" />
                    <Styles>
                        <EmptyCard Wrap="True"></EmptyCard>
                        <Card Height="100px" CssClass="setascurve"></Card>
                    </Styles>
                    <Columns>
                        <dx:CardViewTextColumn FieldName="Title" Caption="Project Description" PropertiesTextEdit-EncodeHtml="false" Settings-AllowSort="True" SortOrder="Ascending" Settings-SortMode="DisplayText"></dx:CardViewTextColumn>
                        <dx:CardViewTextColumn FieldName="TotalAmount" PropertiesTextEdit-DisplayFormatString="{0:C}K Total Amount" PropertiesTextEdit-EncodeHtml="false" Caption="Total"></dx:CardViewTextColumn>
                        <dx:CardViewTextColumn FieldName="AmountLeft" Caption="Amount Left" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0:C}K Left"></dx:CardViewTextColumn>
                        <dx:CardViewTextColumn FieldName="AllMonth" Caption="Months" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0} Months Total"></dx:CardViewTextColumn>
                        <dx:CardViewTextColumn FieldName="MonthLeft" Caption="Months Left" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0} Months Left"></dx:CardViewTextColumn>
                        <dx:CardViewTextColumn FieldName="Issues" Caption="Issues" PropertiesTextEdit-EncodeHtml="false" PropertiesTextEdit-DisplayFormatString="{0} Issues"></dx:CardViewTextColumn>
                        <dx:CardViewTextColumn FieldName="RiskLevel" Caption="Risk Level" PropertiesTextEdit-EncodeHtml="false"></dx:CardViewTextColumn>
                    </Columns>
                    <CardLayoutProperties ColCount="2">
                        <Items>
                            <dx:CardViewColumnLayoutItem ColumnName="Title" ShowCaption="False" CssClass=" titlecss" ColSpan="2"></dx:CardViewColumnLayoutItem>
                            <dx:CardViewColumnLayoutItem ColumnName="TotalAmount" CssClass="data-cell" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                            <dx:CardViewColumnLayoutItem ColumnName="AmountLeft" CssClass="data-cell" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                            <dx:CardViewColumnLayoutItem ColumnName="AllMonth" CssClass="data-cell" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                            <dx:CardViewColumnLayoutItem ColumnName="MonthLeft" CssClass="data-cell" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                            <dx:CardViewColumnLayoutItem ColumnName="Issues" CssClass="data-cell" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                            <dx:CardViewColumnLayoutItem ColumnName="RiskLevel" CssClass="data-cell" ShowCaption="False" ColSpan="1"></dx:CardViewColumnLayoutItem>
                        </Items>
                        <Styles LayoutGroup-CssClass="layoutgroup" LayoutGroup-Cell-CssClass="layoutgroup-cell">
                        </Styles>
                    </CardLayoutProperties>
                </dx:ASPxCardView>
            </div>
        </div>
    </div>
    <dx:ASPxPopupControl ClientInstanceName="addnewstrategy" ID="addnewstrategy" OnWindowCallback="addnewstrategy_WindowCallback" runat="server" Modal="True" Width="450px"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        HeaderText="Add New Business Strategy" AllowDragging="false" PopupAnimationType="None" EnableViewState="False">
        <ClientSideEvents Closing="function(s,e){ASPxClientEdit.ClearEditorsInContainerById('contentDiv');}" EndCallback="function(s,e){ShowBICreationMsg(s,e);}" />
        <ContentCollection>
            <dx:PopupControlContentControl ID="pccBusinessStrategy" runat="server">
                <dx:ASPxPanel ID="pnladdstrategy" ClientInstanceName="pnladdstrategy" runat="server">
                    <PanelCollection>
                        <dx:PanelContent>
                            <div>
                                <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">
                                    <tr>
                                        <td class="ms-formlabel" style="width: 0px !important;">
                                            <h3 class="ms-standardheader">Title<b style="color: Red;">*</b>
                                            </h3>
                                        </td>
                                        <td class="ms-formbody">
                                            <dx:ASPxTextBox ID="txtBSTitle" runat="server" ValidationSettings-RequiredField-IsRequired="true" ValidationSettings-RequiredField-ErrorText="Please enter title" ValidationSettings-ErrorDisplayMode="ImageWithTooltip" ValidationSettings-Display="Dynamic" ClientInstanceName="txtBSTitle"></dx:ASPxTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ms-formlabel" style="width: 0px !important;">
                                            <h3 class="ms-standardheader">Description</h3>
                                        </td>
                                        <td class="ms-formbody">
                                            <dx:ASPxMemo ID="txtBSDescription" Width="100%" runat="server" ClientInstanceName="txtBSDescription"></dx:ASPxMemo>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                            <div class="fright" style="margin: 10px 0px 10px 0px;">
                                <%--<asp:LinkButton ID="btnSaveBS" runat="server" OnClick="btnSaveBS_Click" Text="&nbsp;&nbsp;Save&nbsp;&nbsp;" ToolTip="Save">--%>
                                <a onclick="saveBIvalues()">
                                    <span class="button-bg" style="height: 28px;">
                                        <b style="float: left; font-weight: normal; top: 0;">Save</b>
                                        <i style="float: left; position: relative; top: -3px; left: 2px">
                                            <img src="/Content/ButtonImages/save.png" style="border: none;" title="" alt="" />
                                        </i>
                                    </span>
                                </a>
                                <%-- </asp:LinkButton>--%>

                                <a href="javascript:Void(0);" onclick="addnewstrategy.Hide();"
                                    title="Cancel">
                                    <span class="button-bg" style="height: 28px;">
                                        <b style="float: left; font-weight: normal; top: 0;">Cancel</b>
                                        <i style="float: left; position: relative; top: -3px; left: 2px">
                                            <img src="/Content/ButtonImages/cancelwhite.png" style="border: none;" title="" alt="" />
                                        </i>
                                    </span>
                                </a>
                            </div>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>

    <dx:ASPxPopupControl ClientInstanceName="addnewinitiativepopup" ID="addnewinitiativepopup" runat="server" Modal="True" Width="450px"
        PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter"
        HeaderText="Add New Initiative" AllowDragging="false" PopupAnimationType="None" EnableViewState="False">
        <ClientSideEvents Closing="function(s,e){ASPxClientEdit.ClearEditorsInContainerById('contentDiv');}" />
        <ContentCollection>
            <dx:PopupControlContentControl ID="popcontrolInitiative" runat="server">
                <dx:ASPxPanel ID="pnladdnewinitiative" ClientInstanceName="pnladdnewinitiative" runat="server">
                    <PanelCollection>
                        <dx:PanelContent>
                            <asp:Panel ID="pnlloadinitvalues" runat="server">
                                <div style="float: right; width: 98%; padding-left: 10px;">
                                    <table class="ms-formtable" cellpadding="0" cellspacing="0" style="border-collapse: collapse" width="100%">

                                        <tr>
                                            <td class="ms-formlabel">
                                                <h3 class="ms-standardheader"><%=bsTitle %>
                                                </h3>
                                            </td>
                                            <td class="ms-formbody">
                                                <asp:DropDownList runat="server" ID="ddlBusinessStrategy">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="trTitle" runat="server">
                                            <td class="ms-formlabel">
                                                <h3 class="ms-standardheader">Title<b style="color: Red;">*</b>
                                                </h3>
                                            </td>
                                            <td class="ms-formbody">
                                                <asp:TextBox ID="txtTitle" runat="server" Width="386px" />
                                                <div>
                                                    <asp:RequiredFieldValidator ID="rfvtxtTitle" ValidateEmptyText="true" Enabled="true" runat="server" ControlToValidate="txtTitle"
                                                        ErrorMessage="Enter Title" Display="Dynamic" ValidationGroup="Save"></asp:RequiredFieldValidator>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr id="tr1" runat="server">
                                            <td class="ms-formlabel">
                                                <h3 class="ms-standardheader">Project Note
                                                </h3>
                                            </td>
                                            <td class="ms-formbody">
                                                <asp:TextBox ID="txtProjectNote" TextMode="MultiLine" CssClass="ms-long" runat="server" Rows="6" cols="20" /></td>
                                        </tr>

                                        <tr id="tr11" runat="server">
                                            <td class="ms-formlabel">
                                                <h3 class="ms-standardheader">Delete
                                                </h3>
                                            </td>
                                            <td class="ms-formbody">
                                                <asp:CheckBox ID="chkDeleted" runat="server" Text="(Prevent use for new item)" />
                                            </td>
                                        </tr>

                                        <tr id="tr4" runat="server">
                                            <td colspan="2" class="ms-formlabel"></td>
                                        </tr>
                                    </table>
                                    <table width="100%">
                                        <tr id="tr2" runat="server">
                                            <td align="left" style="padding-top: 5px;"></td>
                                            <td align="right" style="padding-top: 5px;">
                                                <div style="float: right;">

                                                    <%--<asp:LinkButton ID="btnSave" runat="server" Text="&nbsp;&nbsp;Save&nbsp;&nbsp;" ToolTip="Save" ValidationGroup="Save" OnClick="btnSave_Click">--%>
                                                    <a onclick="savevalues()">
                                                        <span class="button-bg" style="height: 28px;">
                                                            <b style="float: left; font-weight: normal; top: 0;">Save</b>
                                                            <i style="float: left; position: relative; top: -3px; left: 2px">
                                                                <img src="/Content/ButtonImages/save.png" style="border: none;" title="" alt="" />
                                                            </i>
                                                        </span>
                                                    </a>
                                                    <%--<dx:ASPxButton ID="btntrigger" runat="server" AutoPostBack="false" ClientInstanceName="btntrigger" OnClick="btnSave_Click">
                                                    </dx:ASPxButton>--%>
                                                    <dx:ASPxCallback ID="aspxclientcallback" runat="server" ClientInstanceName="aspxclientcallback" OnCallback="aspxclientcallback_Callback">
                                                        <ClientSideEvents EndCallback="function(s,e){refreshinitiativeandchild(s,e);}" />
                                                    </dx:ASPxCallback>
                                                    <%--</asp:LinkButton>--%>
                                                    <a onclick="addnewinitiativepopup.Hide();">
                                                        <span class="button-bg" style="height: 28px;">
                                                            <b style="float: left; font-weight: normal; top: 0;">Cancel</b>
                                                            <i style="float: left; position: relative; top: -3px; left: 2px">
                                                                <img src="/Content/ButtonImages/cancelwhite.png" style="border: none;" title="" alt="" />
                                                            </i>
                                                        </span>
                                                    </a>

                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dx:ASPxPanel>
            </dx:PopupControlContentControl>
        </ContentCollection>
    </dx:ASPxPopupControl>
</asp:Panel>
