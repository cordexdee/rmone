<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ReportThumbnailMenu.ascx.cs" Inherits="uGovernIT.Web.ReportThumbnailMenu" %>
<%@ Import Namespace="uGovernIT.Utility" %>
<style data-v="<%=UGITUtility.AssemblyVersion %>">
    .cardThumb {
        display: block;
        background-color: #FFFFFF;
        box-shadow: 0 6px 14px #ddd;
        border-bottom-left-radius: 10px;
        border-bottom-right-radius: 10px;
        border: 0.2px solid #ccc;
        height: 100%;
        max-width: 325px;
    }

    .cardThumb:hover {
        box-shadow: 0 10px 14px #ccc;
    }

    .cardThumb .imgBox {
        height: 150px;
        overflow: hidden;
        border-bottom: 0.2px solid #ccc;
    }

    .cardThumb .imgBox .img {
        width: 100%;
        max-width: 100%;
        height: 100%;
    }

    .cardThumb .textBox {
        padding: 15px 10px;
        text-align: center;
    }

    .cardThumb .textBox h4 {
        color: #000;
        margin-top: 0;
        font-weight: 600;
        transition: color 0.4s ease;
        margin-bottom: 6px;
    }

    .cardThumb:hover .textBox h4 {
        color: #4A90E2;
    }

    .cardThumb .textBox p {
        color: #4b4b4b;
        margin-bottom: 0;
        font-size: 14px;
    }

    @media screen and (min-width: 992px) {
        .contentCenter {
            align-items: center !important;
            justify-content: center !important;
            display: flex;
        }
    }

</style>

<div class="row d-flex flex-wrap">

    <div class="col-xs-12 col-sm-4 col-md-3 pt-4">
        <a href="/Pages/Executive%20Analytics" class="cardThumb">
            <div class="imgBox">
                <img class="img" src="/Content/Images/executivedashboard.jpg" alt="Alternate Text" />
            </div>
            <div class="textBox">
                <h4>Executive Analytics</h4>
                <p>Executive Analytics</p>
            </div>
        </a>
    </div>

    <div class="col-xs-12 col-sm-4 col-md-3 pt-4">
        <a href="/Pages/Staff%20Analytics" class="cardThumb">
            <div class="imgBox">
                <img class="img" src="/Content/Images/staffdashboard.jpg" alt="Alternate Text" />
            </div>
            <div class="textBox">
                <h4>Staff Analytics</h4>
                <p>Staff Analytics</p>
            </div>
        </a>
    </div>
    <div class="col-xs-12 col-sm-4 col-md-3 pt-4">
        <a href="/pages/Financial%20Analytics" class="cardThumb">
            <div class="imgBox">
                <img class="img" src="/Content/Images/fa.jpg" alt="Alternate Text" />
            </div>
            <div class="textBox">
                <h4>Financial Analytics</h4>
                <p>Financial Analytics</p>
            </div>
        </a>
    </div>
    <div class="col-xs-12 col-sm-4 col-md-3 pt-4">
        <a href="/pages/Resource%20Analytics" class="cardThumb">
            <div class="imgBox">
                <img class="img" src="/Content/Images/ra.jpg" alt="Alternate Text" />
            </div>
            <div class="textBox">
                <h4>Resource Analytics</h4>
                <p>Resource Analytics</p>
            </div>
        </a>
    </div>

    <div class="contentCenter" style="width:100%;">
    <div class="col-xs-12 col-sm-4 col-md-3 pt-4">
        <a href="/pages/Project%20Analytics" class="cardThumb">
            <div class="imgBox">
                <img class="img" src="/Content/Images/pa.jpg" alt="Alternate Text" />
            </div>
            <div class="textBox">
                <h4>Project Analytics</h4>
                <p>Project Analytics</p>
            </div>
        </a>
    </div>
    <div class="col-xs-12 col-sm-4 col-md-3 pt-4">
        <a href="/pages/Recruitment%20Analytics" class="cardThumb">
            <div class="imgBox">
                <img class="img" src="/Content/Images/rv.jpg" alt="Alternate Text" />
            </div>
            <div class="textBox">
                <h4>Recruitment Analytics</h4>
                <p>Recruitment Analytics</p>
            </div>
        </a>
    </div>
    <div class="col-xs-12 col-sm-4 col-md-3 pt-4">
        <a href="/pages/Utilization%20Analytics" class="cardThumb">
            <div class="imgBox">
                <img class="img" src="/Content/Images/ea.jpg" alt="Alternate Text" />
            </div>
            <div class="textBox">
                <h4>Utilization Analytics</h4>
                <p>Utilization Analytics</p>
            </div>
        </a>
    </div>
    <div class="col-xs-12 col-sm-4 col-md-3 pt-4">
        <a href="/Pages/Bench%20Dashboard" class="cardThumb">
            <div class="imgBox">
                <img class="img" src="/Content/Images/BD.jpg" alt="Alternate Text" />
            </div>
            <div class="textBox">
                <h4>Bench Analytics </h4>
                <p>Bench Analytics</p>
            </div>
        </a>
    </div>
        </div>
</div>