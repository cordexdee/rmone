/*
@license

dhtmlxGantt v.6.2.3 Professional Evaluation
This software is covered by DHTMLX Evaluation License. Contact sales@dhtmlx.com to get Commercial or Enterprise license. Usage without proper license is prohibited.

(c) XB Software Ltd.

*/
Gantt.plugin(function(e){!function(e,t){if("object"==typeof exports&&"object"==typeof module)module.exports=t();else if("function"==typeof define&&define.amd)define([],t);else{var r=t();for(var a in r)("object"==typeof exports?exports:e)[a]=r[a]}}(window,function(){return function(e){var t={};function r(a){if(t[a])return t[a].exports;var n=t[a]={i:a,l:!1,exports:{}};return e[a].call(n.exports,n,n.exports,r),n.l=!0,n.exports}return r.m=e,r.c=t,r.d=function(e,t,a){r.o(e,t)||Object.defineProperty(e,t,{enumerable:!0,get:a})},r.r=function(e){"undefined"!=typeof Symbol&&Symbol.toStringTag&&Object.defineProperty(e,Symbol.toStringTag,{value:"Module"}),Object.defineProperty(e,"__esModule",{value:!0})},r.t=function(e,t){if(1&t&&(e=r(e)),8&t)return e;if(4&t&&"object"==typeof e&&e&&e.__esModule)return e;var a=Object.create(null);if(r.r(a),Object.defineProperty(a,"default",{enumerable:!0,value:e}),2&t&&"string"!=typeof e)for(var n in e)r.d(a,n,function(t){return e[t]}.bind(null,n));return a},r.n=function(e){var t=e&&e.__esModule?function(){return e.default}:function(){return e};return r.d(t,"a",t),t},r.o=function(e,t){return Object.prototype.hasOwnProperty.call(e,t)},r.p="/codebase/",r(r.s=231)}({231:function(t,r){!function(){function t(t){if(!e.config.show_markers)return!1;if(!t.start_date)return!1;var r=e.getState();if(!(+t.start_date>+r.max_date||(!t.end_date||+t.end_date<+r.min_date)&&+t.start_date<+r.min_date)){var a=document.createElement("div");a.setAttribute("data-marker-id",t.id);var n="gantt_marker";e.templates.marker_class&&(n+=" "+e.templates.marker_class(t)),t.css&&(n+=" "+t.css),t.title&&(a.title=t.title),a.className=n;var i=e.posFromDate(t.start_date);if(a.style.left=i+"px",a.style.height=Math.max(e.getRowTop(e.getVisibleTaskCount()),0)+"px",t.end_date){var o=e.posFromDate(t.end_date);a.style.width=Math.max(o-i,0)+"px"}return t.text&&(a.innerHTML="<div class='gantt_marker_content' >"+t.text+"</div>"),a}}function r(){if(e.$task_data){var t=document.createElement("div");t.className="gantt_marker_area",e.$task_data.appendChild(t),e.$marker_area=t}}e._markers||(e._markers=e.createDatastore({name:"marker",initItem:function(t){return t.id=t.id||e.uid(),t}})),e.config.show_markers=!0,e.attachEvent("onBeforeGanttRender",function(){e.$marker_area||r()}),e.attachEvent("onDataRender",function(){e.$marker_area||(r(),e.renderMarkers())}),e.attachEvent("onGanttReady",function(){r(),e.$services.getService("layers").createDataRender({name:"marker",defaultContainer:function(){return e.$marker_area}}).addLayer(t)}),e.getMarker=function(e){return this._markers?this._markers.getItem(e):null},e.addMarker=function(e){return this._markers.addItem(e)},e.deleteMarker=function(e){return!!this._markers.exists(e)&&(this._markers.removeItem(e),!0)},e.updateMarker=function(e){this._markers.refresh(e)},e._getMarkers=function(){return this._markers.getItems()},e.renderMarkers=function(){this._markers.refresh()}}()}})})});