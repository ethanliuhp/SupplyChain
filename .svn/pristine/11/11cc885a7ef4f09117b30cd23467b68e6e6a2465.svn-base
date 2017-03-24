// JScript 文件
function DoPurview(url,path)
{
		var spans = document.getElementsByTagName('span');
		var count = spans.length;		
		var objsHtml = '';
		for (var i=0;i<count;i++)
		{
			var span = spans[i];	
			
			var pId = span.getAttribute('purviewId');			
			if ( pId != 'ToBeMulitiPurview') continue;		
			
			var chk = span.getElementsByTagName('input')[0];			
			if (chk.getAttribute('type') != 'checkbox') continue;
			
			if (!chk.checked) continue;
			
			var objId = span.getAttribute('objectId');
			var objType = span.getAttribute('objectType');			
			
			if (objsHtml != '') objsHtml += '|';
			objsHtml += (objId +'/'+ objType);
		}		
		
		document.cookie = 'SelectPurviewObject=' + objsHtml + ';path=' + path;			
		window.showModalDialog(url,window,'dialogWidth: 733px;dialogHeight: 550px;status:no;center: yes;resizable:on;');

		
}

function ShowDialogSizeByParam(url,path,width,height)
{
		var spans = document.getElementsByTagName('span');
		var count = spans.length;		
		var objsHtml = '';
		for (var i=0;i<count;i++)
		{
			var span = spans[i];	
			
			var pId = span.getAttribute('purviewId');			
			if ( pId != 'ToBeMulitiPurview') continue;		
			
			var chk = span.getElementsByTagName('input')[0];			
			if (chk.getAttribute('type') != 'checkbox') continue;
			
			if (!chk.checked) continue;
			
			var objId = span.getAttribute('objectId');
			var objType = span.getAttribute('objectType');			
			
			if (objsHtml != '') objsHtml += '|';
			objsHtml += (objId +'/'+ objType);
		}		
		
		document.cookie = 'SelectPurviewObject=' + objsHtml + ';path=' + path;			
		window.showModalDialog(url,window,'dialogWidth: '+width+'px;dialogHeight: '+height+'px;status:no;center: yes;resizable:on;');

		
}

function DoAddToBaseLine(url,path)
{
		var spans = document.getElementsByTagName('span');
		var count = spans.length;		
		var objsHtml = '';
		for (var i=0;i<count;i++)
		{
			var span = spans[i];	
			
			var pId = span.getAttribute('purviewId');			
			if ( pId != 'ToBeMulitiPurview') continue;		
			
			var chk = span.getElementsByTagName('input')[0];			
			if (chk.getAttribute('type') != 'checkbox') continue;
			
			if (!chk.checked) continue;
			
			var objId = span.getAttribute('objectId');
			var objType = span.getAttribute('objectType');			
			
			if (objsHtml != '') objsHtml += '|';
			objsHtml += (objId +'/'+ objType);
		}		
		
		document.cookie = 'SelectPurviewObject=' + objsHtml + ';path=' + path;			
		window.showModalDialog(url,window,'dialogWidth: 500px;dialogHeight: 300px;status:no;center: yes;resizable:on;');

		
}
