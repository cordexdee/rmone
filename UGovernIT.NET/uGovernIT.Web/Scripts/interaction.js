/*
 * --------------------------------------------------------
 *   JavaScript Libary to Access SharePoint Lists
 *   
 *  Author: DuWei (Erucy), MVP of SharePoint Services
 *  Version: 1.0
 *  LastMod: 2009-1-26
 * --------------------------------------------------------
 */

/*
 * ajax post to sharepoint, for internal use, sync mode
 * 	wsUrl: web service file url
 * 	actionHeader: "SOAPAction" http header
 * 	wsBody: post content
 * 	dealFunc: ajax process function
 */
function innerPost(wsUrl, actionHeader, wsBody, dealFunc){
	var content = new StringBuffer();
	content.append("<?xml version='1.0' encoding='utf-8'?>");
	content.append("<soap:Envelope xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:soap='http://schemas.xmlsoap.org/soap/envelope/'>");
	content.append("<soap:Body>");
	content.append(wsBody);
	content.append("</soap:Body>");
	content.append("</soap:Envelope>");
	$.ajax({
		async:true,
		type:"POST",
		url:wsUrl,
		contentType:"text/xml; charset=utf-8",
		processData:false,
		data:content.toString(),
		dataType:"xml",
		beforeSend:function(xhr){
			xhr.setRequestHeader('SOAPAction', actionHeader);
		},
		success:dealFunc
	});
}

/*
 * get user profile data
 * return: user profile data in object type
 * 	result[propName] to get value
 */
function getUserProfile(){
	var result = {};
	innerPost(wsBaseUrl + 'userprofileservice.asmx', 
		'http://microsoft.com/webservices/SharePointPortalServer/UserProfileService/GetUserProfileByName',
		'<GetUserProfileByName xmlns="http://microsoft.com/webservices/SharePointPortalServer/UserProfileService"><AccountName></AccountName></GetUserProfileByName>',
		function(data){
			$('PropertyData', data).each(function(){
				var name = $(this).find('Name').text();
				var value = $(this).find('Value').text();
				result[name] = value;
			});
		});
	return result;
}

/*
 * build query content, for internal use
 * 	listName: name of the list
 * 	query: caml type query
 * 	viewFields: array of the fields name (internal name)
 * 	rowLimit: how many items to query
 * 	pagingInfo: which page to query
 * return: content for web service
 */
function buildQueryContent(listName, query, viewFields, rowLimit, pagingInfo){
	var result = new StringBuffer();
	result.append('<GetListItems xmlns="http://schemas.microsoft.com/sharepoint/soap/">');
	result.append('<listName>' + listName + '</listName>');
	if(query != null && query != ''){
		result.append('<query><Query xmlns="">');
		result.append(query);
		result.append('</Query></query>');
	}
	if(viewFields != null && viewFields.length > 0){
		result.append('<viewFields><ViewFields xmlns="">');
		$.each(viewFields, function(idx, field){
			result.append('<FieldRef Name="' + field + '"/>');
		});
		result.append('</ViewFields></viewFields>');
	}
	if(rowLimit != undefined && rowLimit != null && rowLimit > 0)
		result.append('<rowLimit>' + rowLimit + '</rowLimit>');
	else
	    result.append('<rowLimit>100000</rowLimit>');
	result.append('<queryOptions><QueryOptions xmlns=""><IncludeMandatoryColumns>FALSE</IncludeMandatoryColumns>');
	if(pagingInfo != undefined && pagingInfo != null && pagingInfo != '')
		result.append('<Paging ListItemCollectionPositionNext="' + pagingInfo.replace(/&/g, '&amp;') + '" />');
	result.append('</QueryOptions></queryOptions>');
	result.append('</GetListItems>');
	return result.toString();
}

/*
 * get items count in a list
 * 	listName: name of the list
 * 	query: caml type query
 * return: count of the items
 */
function queryItemCount(listName, query){
	var content = buildQueryContent(listName, query, null);
	var result = -1;
	innerPost(wsBaseUrl + 'lists.asmx', 'http://schemas.microsoft.com/sharepoint/soap/GetListItems', content, function(data){
		result = parseInt($('rs\\:data', data).attr('ItemCount'));
	});
	return result;
}

/*
 * convert item to object, for internal use
 * 	itemData: xml type data
 * 	viewFields: fields to view (internal name)
 * return: object type item
 * 	result[fieldName] to get value
*/
function generateItem(itemData, viewFields){
	var result = {};
	$.each(viewFields, function(idx, field){
		var value = $(itemData).attr('ows_' + field);
		if(value == undefined) value = null;
		result[field] = value;
	});
	return result;
}

/*
 * get items according to query
 * 	listName: name of the list
 * 	query: caml type query
 * 	viewFields: fields to view (internal name)
 * 	rowLimit: how many items to return
 * 	pagingInfo: which page to query, use the 'nextPagingInfo' of the previous page result
 * return: 	{ count: return item count
 * 			  nextPagingInfo: paging info to query next page, undefined for the last page
 * 			  items:array of object type items
 * 			}
 */
function queryItems(listName, query, viewFields, rowLimit, pagingInfo, callback) {
    var content = buildQueryContent(listName, query, viewFields, rowLimit, pagingInfo);
    var result = { count: -1, nextPagingInfo: '', items: new Array() };
    innerPost(wsBaseUrl + 'lists.asmx', 'http://schemas.microsoft.com/sharepoint/soap/GetListItems', content, function (data) {


        result.count = $('rs\\:data', data).attr('ItemCount');
        if (result.count != -1 && result.count != undefined) {
            //if ($.browser.msie || $.browser.mozilla) {

            result.nextPagingInfo = $('rs\\:data', data).attr('ListItemCollectionPositionNext');
            $('z\\:row', data).each(function (idx, itemData) {
                result.items.push(generateItem(itemData, viewFields));
            });
        }
        else {
            result.count = $('data', data).attr('ItemCount');
            result.nextPagingInfo = $('data', data).attr('ListItemCollectionPositionNext');
            $('row', data).each(function (idx, itemData) {
                result.items.push(generateItem(itemData, viewFields));
            });
        }
        if (callback && typeof (callback) === "function") {
            callback(result);
        }       
    });
}




//function getListCollection() {

//        //listName = "Team Site Owners";
//        var content = new StringBuffer();
//        content.append('<GetListCollection xmlns="http://schemas.microsoft.com/sharepoint/soap/" />');
//        //content.append('<groupName>' + groupName + '</groupName>');
//        //content.append('</GetUserCollectionFromGroup>');
        
//        var result = null;
//        var wsBaseUrl = 'http://kellggncpu0122/_vti_bin/';
//        innerPost(wsBaseUrl + 'lists.asmx', 'http://schemas.microsoft.com/sharepoint/soap/GetListCollection', content.toString(), function (data) {
//            result = data;
//        });
//        return result;
//    }



function queryItemsByGroup(groupName) {

    //listName = "Team Site Owners";
    var content = new StringBuffer();
    content.append('<GetUserCollectionFromGroup xmlns="http://schemas.microsoft.com/sharepoint/soap/directory/">');
    content.append('<groupName>' + groupName + '</groupName>');
    content.append('</GetUserCollectionFromGroup>');
    //var result = { count: -1, nextPagingInfo: '', items: new Array() };
    var result = null;
    innerPost(wsBaseUrl + 'UserGroup.asmx', 'http://schemas.microsoft.com/sharepoint/soap/directory/GetUserCollectionFromGroup', content.toString(), function (data) {
        result = data;
    });
    return result;
}

/*
 * get a item by id
 * 	listName: name of the list
 * 	id: item id
 * 	viewFields: fields to view (internal name)
 */
function getItemById(listName, id, viewFields){
	var query = '<Where><Eq><FieldRef Name="ID" /><Value Type="Text">' + id + '</Value></Eq></Where>';
	var result = queryItems(listName, query, viewFields);
	if(result.items.length == 0) return null;
	else return result.items[0];
}

/*
 * build modify use (new & update) web service content, for internal use
 * 	listName: name of the list
 * 	id: item id to update, 0 for new item
 * 	data: update info
 * 		data[fieldName] = newValue
 * return: web service content to post
 */
function buildModifyContent(listName, id, data){
	var result = new StringBuffer();
	result.append('<UpdateListItems xmlns="http://schemas.microsoft.com/sharepoint/soap/">');
	result.append('<listName>' + listName + '</listName>');
	result.append('<updates><Batch OnError="Continue" xmlns="">');
	if (id == 0) {
		result.append('<Method ID="1" Cmd="New">');
	}
	else {
		result.append('<Method ID="1" Cmd="Update">');
		result.append('<Field Name="ID">' + id + '</Field>');
	}
	$.each(data, function(field, value){
		result.append('<Field Name="');
		result.append(field);
		result.append('"><![CDATA[');
		result.append(value);
		result.append(']]></Field>');
	});
	result.append('</Method></Batch></updates></UpdateListItems>');
	return result.toString();
}

/*
 * build modify use web service content, for internal use
 * 	listName: name of the list
 * 	ids: items id to update
 * 	data: update info
 * 		data[fieldName] = newValue
 * return: web service content to post
 */
function buildUpdatesContent(listName, ids, data){
	var result = new StringBuffer();
	result.append('<UpdateListItems xmlns="http://schemas.microsoft.com/sharepoint/soap/">');
	result.append('<listName>' + listName + '</listName>');
	result.append('<updates><Batch OnError="Continue" xmlns="">');
	$.each(ids, function(idx, id){
		result.append('<Method ID="' + (idx + 1).toString() + '" Cmd="Update">');
		result.append('<Field Name="ID">' + id + '</Field>');		
		$.each(data, function(field, value){
			result.append('<Field Name="');
			result.append(field);
			result.append('"><![CDATA[');
			result.append(value);
			result.append(']]></Field>');
		});
		result.append('</Method>');
	});
	result.append('</Batch></updates></UpdateListItems>');
	return result.toString();
}

/*
 * build delete use web service content, for internal use
 * 	listName: name of the list
 * 	ids: item id to delete
 * return: web service content to post
 */
function buildDeleteContent(listName, id){
	var result = new StringBuffer();
	result.append('<UpdateListItems xmlns="http://schemas.microsoft.com/sharepoint/soap/">');
	result.append('<listName>' + listName + '</listName>');
	result.append('<updates><Batch OnError="Continue" xmlns="">');
	result.append('<Method ID="1" Cmd="Delete">');
	result.append('<Field Name="ID">' + id + '</Field>');
	result.append('</Method>');
	result.append('</Batch></updates></UpdateListItems>');
	return result.toString();
}

/*
 * update item
 * 	listName: name of the list
 * 	id: id of the item to update
 * 	data: data to modified
 * 		data[fieldName] = value, use internal name
 * return: operation result
 * 		{ success: (bool) whether the operation is success
 * 		  errorCode: error code, 0x00000000 for no error
 * 		  errorText: error text, null for no error
 * 		  id: item id, -1 for error
 * 		}
 */
function updateItem(listName, id, data){
	var content = buildModifyContent(listName, id, data);
	var result = {success:false, errorCode:'', errorText:'internal error', id:-1};
	innerPost(wsBaseUrl + 'lists.asmx', 'http://schemas.microsoft.com/sharepoint/soap/UpdateListItems', content, function(data){
		if ($('ErrorText', data).length > 0) {
			result.success = false;
			result.errorCode = $('ErrorCode', data).text();
			result.errorText = $('ErrorText', data).text();
			result.id = -1;
		} else {
			result.success = true;
			result.errorCode = '0x00000000';
			result.errorText = null;
			result.id = $('z\\:row', data).attr('ows_ID');
		}
	});
	return result;
}

/*
 * update items
 * 	listName: name of the list
 * 	ids: id of the items to update
 * 	data: data to modified
 * 		data[fieldName] = value, use internal name
 * return: operation result
 * 		{ success: (bool) whether the operation is success
 * 		  errorCode: error code, 0x00000000 for no error
 * 		  errorText: error text, null for no error
 * 		}
 */
function updateItems(listName, ids, data){
	var content = buildUpdatesContent(listName, ids, data);
	var result = {success:false, errorCode:'', errorText:'internal error'};
	innerPost(wsBaseUrl + 'lists.asmx', 'http://schemas.microsoft.com/sharepoint/soap/UpdateListItems', content, function(data){
		if ($('ErrorText', data).length > 0) {
			result.success = false;
			result.errorCode = $('ErrorCode', data).text();
			result.errorText = $('ErrorText', data).text();
		} else {
			result.success = true;
			result.errorCode = '0x00000000';
			result.errorText = null;
		}
	});
	return result;
}

/*
 * add a new item
 * 	listName: name of the list
 * 	data: data of the new item
 * 		data[fieldName] = value, use internal name
 * return: operation result
 * 		{ success: (bool) whether the operation is success
 * 		  errorCode: error code, 0x00000000 for no error
 * 		  errorText: error text, null for no error
 * 		  id: new item id, -1 for error
 * 		}
 */
function addItem(listName, data){
	return updateItem(listName, 0, data);
}

/*
 * delete item
 * 	listName: name of the list
 * 	id: id of the item to delete
 * return: operation result
 * 		{ success: (bool) whether the operation is success
 * 		  errorCode: error code, 0x00000000 for no error
 * 		  errorText: error text, null for no error
 * 		}
 */
function deleteItem(listName, id){
	var content = buildDeleteContent(listName, id);
	var result = {success:false, errorCode:'', errorText:'internal error'};
	innerPost(wsBaseUrl + 'lists.asmx', 'http://schemas.microsoft.com/sharepoint/soap/UpdateListItems', content, function(data){
		if ($('ErrorText', data).length > 0) {
			result.success = false;
			result.errorCode = $('ErrorCode', data).text();
			result.errorText = $('ErrorText', data).text();
		} else {
			result.success = true;
			result.errorCode = '0x00000000';
			result.errorText = null;
		}
	});
	return result;
}