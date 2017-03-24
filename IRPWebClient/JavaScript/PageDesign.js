var selected_cell = null;
var delete_image_path = "";
var cancel_image_path = "";
var properties_image_path = "";
var btnSelectUserControl = "btnSelectUserControl";

function GetTableLayout(table)
{
	if(table.tagName!="TABLE") return "";	
	var html = "";	
	
	for(var i=0; i<3; i++)
	{		
		if (i==2||i==0)
		{
			html += "_";
			//alert(html);
		}	
		var tr = table.rows(i);		
		for(var j=0; j<tr.cells.length; j++)
		{
			if (i==1)
			{				
				html += "_";
				//alert(html);
			}
			var td = tr.cells(j);			
			
			for(var k=0; k<td.children.length; k++)
			{
				var div = td.children(k);
				
				if(div.type+'' == "Table")
				{
					GetTableLayout(div.children(0));
				}
				else
				{
					//if(div.selected+''=="true")
					
					if (html.charAt(html.length - 1) != '_')
					{									
						html += "/" + td.children(k).innerText;							
						//alert(html);
					}
					else
					{
					html += td.children(k).innerText;
					}
			//		}
				//	else
				//	{
				//		html += '<usercontrol guid="' + div.guid + '" name="' + td.children(k).innerText + '"></usercontrol>';
				//	}
				}
			}			
		}		
	}	
	html = html.substring(1,html.length);
	//alert(html);
	
	return html;
}

function SaveTableChanges()//use GetTableLayout()函数。
{
	//var MainTable = document.getElementById("MainTable");
	/*for (var i=0;i<TopPane.getElementsByTagName("div").length;i++)
	{
	  var html=TopPane.getElementsByTagName("div").item(i).getElementsByTagName("a").item(0).InnerText;
	}*/
	
	var layoutinfo = document.getElementById("layoutinfo");
	
	var html = GetTableLayout(document.all.MainTable);
	
	
	layoutinfo.value = html;
		
}

function NewRow(table) //use SetTdhtml()函数
{
	var length = table.rows.length;
	var max_td = 0;
			
	for(var i=0; i<length; i++)
	{
		var tr = table.rows(i);
		
		if(tr.cells.length > max_td)
		{
			max_td = tr.cells.length;
		}
	}
	
	var tr = table.insertRow(length);
	
	for(var j=0; j<max_td; j++)
	{
		var td = tr.insertCell(tr.cells.length);
		
		td.setAttribute("DragDrop", "true");
		/*
		if(table!=document.all.MainTable)
		{
			td.onmousedown = new Function("UserControlSelectedOnMouseDown(this)");
		}
		*/
		SetTDHtml(td, table==document.all.MainTable);
	}
}

function NewColumn(table) //use settdhtml()函数
{
	var length = table.rows.length;
		
	for(var i=0; i<length; i++)
	{
		var tr = table.rows(i);
		
		var td = tr.insertCell(tr.cells.length);
		
		td.setAttribute("DragDrop", "true");
		/*
		if(table!=document.all.MainTable)
		{
			td.onmousedown = new Function("UserControlSelectedOnMouseDown(this)");
		}
		*/
		SetTDHtml(td, table==document.all.MainTable);
	}
}

function SetTDHtml(td, ismaintable)
{
	if(ismaintable)
	{
		td.innerHTML = "<div class='Toolbar'><a onclick='return ShowCellProperties(this.parentElement.parentElement)' href='#'><img style='border:0px' src='" + properties_image_path + "' alt='Set cell properties' /></a> <a onclick='return CollapseColumn(this.parentElement.parentElement)' href='#'><img style='border:0px' src='" + delete_image_path + "' alt='Remove this cell' /></a></div>";
	}
	else
	{
		td.innerHTML = "<div class='Toolbar' onmousedown='UserControlSelectedOnMouseDown(this)'><a onclick='return ShowCellProperties(this.parentElement.parentElement)' href='#'><img style='border:0px' src='" + properties_image_path + "' alt='Set cell properties' /></a> <a onclick='return CollapseColumn(this.parentElement.parentElement)' href='#'><img style='border:0px' src='" + delete_image_path + "' alt='Remove this cell' /></a></div>";
	}
}

function DeleteCell(tr, td)
{
	if(tr.tagName!="TR") return;
	
	var table = tr;
	while(table.tagName!="TABLE")
	{
		table = table.parentElement;
	}

	if(table.id=="MainTable" && table.rows.length==1 && table.rows(0).cells.length==1)
	{
		return false;
	}
	
	if(td==selected_cell)
	{
		selected_cell = null;
		document.all.LinkSaveCellProperties.disabled = true;
	}
	
	tr.deleteCell(td.cellIndex);
	
	return true;
}

function VerifySpan(table)
{
	var max_td = 0;
	var max_span = 0;

	var length = table.rows.length;
		
	for(var i=0; i<length; i++)
	{
		var tr = table.rows(i);
		
		if(tr.cells.length > max_td)
		{
			max_td = tr.cells.length;
		}
	}
	
	for(var i=0; i<table.rows(0).cells.length; i++)
	{
		var td = table.rows(0).cells(i);
		
		max_span += td.colSpan;
	}
	
	if(max_span > max_td)
	{
		for(var i=0; i<table.rows.length; i++)
		{
			var span = max_span;
			var tr = table.rows(i);
			
			for(var j=0; j<table.rows(i).cells.length; j++)
			{
				var td = table.rows(i).cells(j);
				
				if(td.colSpan>1)
				{
					var minus = span - max_td;
					
					if(td.colSpan - 1 < minus)
					{
						minus = td.colSpan - 1;
					}
					
					span -= minus;
					td.colSpan -= minus;
				}
			}
		}
	}
}

function CollapseColumn(td) //调用了n多函数 一级 当删除表格使用
{
	if(td.tagName!="TD") return;
	
	var num = td.colSpan;
	
	var table = td;
	while(table.tagName!="TABLE")
	{
		table = table.parentElement;
	}
	
	if(table.id!="MainTable" && table.rows.length==1 && table.rows(0).cells.length==1)
	{
		var div = table.parentElement;
		var td = table.parentElement.parentElement;
		
		td.removeChild(div);
	}
	else
	{
		var previous = td.previousSibling;
		var next = td.nextSibling;
		var tr = td.parentElement;
		
		var span = td.colSpan;
		
		var ok = DeleteCell(tr, td);
		
		if(previous != null)
		{
			previous.colSpan = previous.colSpan + span;
			
			previous.removeAttribute("width", 0);
			
			//SetTDHtml(previous);
		}
		else if(next != null)
		{
			next.colSpan = next.colSpan + span;
			
			next.removeAttribute("width", 0);
			
			//SetTDHtml(next);
		}
		else
		{
			if(ok) table.deleteRow(tr.rowIndex);
		}
		
		if(ok) VerifySpan(table);
		
		if(selected_cell != null)
		{
			ShowCellProperties(selected_cell);
		}
	}

	SaveTableChanges();
	
	return false;
}

function AdjustWidth(td)//一级 但没被调用
{
	if(td.tagName!="TD") return;
	
	var tr = td.parentElement;
	var span = 0;

	var table = td;
	while(table.tagName!="TABLE")
	{
		table = table.parentElement;
	}
	
	for(var i=0; i<=td.cellIndex; i++)
	{
		span += tr.cells(i).colSpan;
	}
	
	for(var i=0; i<table.rows.length; i++)
	{
		tr = table.rows(i);
		var tmp_span = 0;
		
		for(var j=0; j<tr.cells.length; j++)
		{
			tmp_span += tr.cells(j).colSpan;
			
			if(tmp_span == span) tr.cells(j).width = td.width;
		}
	}

	SaveTableChanges();
}

// Cell Properties
function ShowCellProperties(td) //一级 当编辑表格时
{
	if(selected_cell!=null) selected_cell.className = "";//removeAttribute("bgColor");
	
	selected_cell = td;
	
	document.all.LinkSaveCellProperties.disabled = false;
	
	document.all.TextCellWidth.disabled = false;
	document.all.TextCellWidth.value = td.width;
	
	document.all.TextCellWidth.focus();
	
	if(td.colSpan>1)
	{
		document.all.TextCellStyle.focus();
		document.all.TextCellWidth.value = "The Width Can't be modified.";
		document.all.TextCellWidth.disabled = true;
	}
	
	document.all.TextCellStyle.value = td.style.cssText;
	
	td.className = "CellSelected"; // .bgColor = "#cccccc";
}

function SaveCellProperties()//一级 当点击link,保存表格属性使用
{
	if(selected_cell!=null && selected_cell.tagName=="TD")
	{
		if(!document.all.TextCellWidth.disabled)
		{
			selected_cell.width = document.all.TextCellWidth.value;
		}
		
		selected_cell.style.cssText = document.all.TextCellStyle.value;

		SaveTableChanges();
	}
}

var mousex = null;
var mousey = null;
function UserControlSelectedOnMouseDown(element)//use --onmouseup he move 一级 设置TD或div为可拖动的元素
{
	var from = window.event.srcElement;
	
	if(from.tagName=="TD" || from.tagName=="DIV")
	{
		while(element!=null && (element.tagName!="DIV" || element.DragDrop+''!="true"))
		{
			element = element.parentElement;
		}
			
		if(element == null) return;

		document.body.onmousemove = UserControlSelectedOnMouseMove;
		document.body.onmouseup = UserControlSelectedOnMouseUp;
		
		element.style.position = "absolute";
		
		var x = window.event.x - window.event.offsetX - 6;
		var y = window.event.y - window.event.offsetY - 10;
		
		//alert("X:" + window.event.x + ",Y:" + window.event.y+ ";ClientX:" + window.event.clientX+",ClientY:" + window.event.clientY+ ";OffsetX:" + window.event.offsetX + ", OffsetY:" + window.event.offsetY + ";" + x + "," + y);
		
		if(from.tagName=="DIV")
		{
			from = from.parentElement;
			
			x = x - from.offsetLeft;
			y = y - from.offsetTop;
		}
		
		//alert(x+"," + y);
		element.style.left = x;
		element.style.top = y;
		
		dragobject = element;
		
		mousex = window.event.clientX;
		mousey = window.event.clientY;
	}
}

function UserControlSelectedOnMouseMove()
{
	if (dragobject)
	{
		if (window.event.clientX >= 0 && window.event.clientY >= 0)
		{
			var x = parseInt(dragobject.style.left);
			var y = parseInt(dragobject.style.top);
			
			//alert(x +"," + y);
			
			dragobject.style.left = x + window.event.clientX - mousex;
			dragobject.style.top = y + window.event.clientY - mousey;
			
			mousex = window.event.clientX;
			mousey = window.event.clientY;
		}
		
		window.event.returnValue = false;
		window.event.cancelBubble = true;
	}	
}

function UserControlSelectedOnMouseUp()
{
	if(dragobject)
	{
		var parent = dragobject.parentElement;
		parent.removeChild(dragobject);
		
		var element = document.elementFromPoint(window.event.clientX, window.event.clientY);
			
		while(element!=null && element.DragDrop!="true")
		{
			element = element.parentElement;
		}
						
		if(element!=null)
		{
				
			if(element.tagName=="TD")
			{
				element.appendChild(dragobject);	
			}
			else if(element.tagName=="DIV")
			{
				var td = element.parentElement;

				while(td!=null && td.DragDrop!="true")
				{
					td = td.parentElement;
				}
				
				if(td==null) return;
				
				td.insertBefore(dragobject, element);
			}
			else if(element.tagName=="TABLE")
			{
				element = element.parentElement;
				
				var td = element.parentElement;

				while(td!=null && td.DragDrop!="true")
				{
					td = td.parentElement;
				}
				
				if(td==null) return;
				
				td.insertBefore(dragobject, element);
			}
			
			dragobject.style.position = "";
			dragobject.style.left = 0;
			dragobject.style.top = 0;
						
			dragobject = null;
			
			SaveTableChanges();
		}
		else
		{
			document.body.removeChild(dragobject);
			
			dragobject = null;
		}
	}
}

// Drag & Drop
var dragobject = null;
function DragDropOnMouseDown(element)//use --up move 一级 设置简单拖动元素  table tr td usercontrol
{
	var div = document.createElement("DIV");

	document.body.onmousemove = DragDropOnMouseMove;
	document.body.onmouseup = DragDropOnMouseUp;

	document.body.insertBefore(div);

	div.innerHTML = element.innerHTML;
	
	div.style.cssText = "";
	div.className = "UserControlMoving";
	div.style.position = "absolute";
	div.style.left = window.event.clientX + document.body.scrollLeft;
	div.style.top = window.event.clientY + document.body.scrollTop;
	
	div.onselectstart = "return false";
	
	if(element.type!=null)
	{
		div.type = element.type;
	}
	else
	{
		div.type = "UserControl";
	}
	
	dragobject = div;
}

function DragRow()//use newnow()函数
{
	var element = document.elementFromPoint(window.event.clientX, window.event.clientY);
		
	while(element!=null && element.tagName!="TABLE")
	{
		element = element.parentElement;
	}
				
	if(element!=null && element.tagName=="TABLE" && element.DragDrop=="true")
	{
		NewRow(element);
		
		SaveTableChanges();
	}

	document.body.removeChild(dragobject);
		
	dragobject = null;
}

function DragColumn()//use newcolume()函数
{
	var element = document.elementFromPoint(window.event.clientX, window.event.clientY);
		
	while(element!=null && element.tagName!="TABLE")
	{
		element = element.parentElement;
	}
					
	if(element!=null && element.tagName=="TABLE" && element.DragDrop=="true")
	{
		NewColumn(element);
				
		SaveTableChanges();
	}

	document.body.removeChild(dragobject);
		
	dragobject = null;
}

function DragUserControl(type)
{
	var element = document.elementFromPoint(window.event.clientX, window.event.clientY);
		
	while(element!=null && element.DragDrop!="true")
	{
		element = element.parentElement;
	}
					
	if(element!=null)
	{
		if(element.tagName=="TD")
		{
			element.appendChild(dragobject);	
		}
		else if(element.tagName=="DIV")
		{
			var td = element.parentElement;

			while(td!=null && td.DragDrop!="true")
			{
				td = td.parentElement;
			}
			
			if(td==null) return;
			
			td.insertBefore(dragobject, element);
		}
		else if(element.tagName=="TABLE")
		{
			element = element.parentElement;
			
			var td = element.parentElement;

			while(td!=null && td.DragDrop!="true")
			{
				td = td.parentElement;
			}
			
			if(td==null) return;
			
			td.insertBefore(dragobject, element);
		}
		
		dragobject.style.cssText = "";
		dragobject.className = "UserControlSelected";
		
		dragobject.style.position = "";
		dragobject.style.left = 0;
		dragobject.style.top = 0;
		dragobject.DragDrop = true;
		dragobject.type = type;
		
		var html = "";
		
		if(type=="UserControl")
		{
			html += "<div  type='UserControl' DragDrop='true' >\n";
			html+= "<table width='100%' border='0px' cellpadding='0' cellspacing='0' class='Portal_Title_bg'>";
			html+= "<tbody>";
			html+= "<tr><td class='Portal_Title_Table' onmousedown='UserControlSelectedOnMouseDown(this)'>";
			html+= dragobject.innerText;
			html+= "</td><td  width='66'align='center' class='Portal_Title_r_dot'>";
			html+= "<a onclick='DeleteUserControl(this)'><img  src='" + cancel_image_path + "' width='16' height='15' style='border:0px' alt='Remove this webpart' /></a>&nbsp;";
			html+= "</td></tr></tbody></table>";
			html += "</div>\n";		
			
		}
		else if(type=="Table")
		{
			html = "<table DragDrop='true' width='100%' cellpadding='0' cellspacing='0' height='100%' border='1' style='font:10.5pt Verdana;'>";
			html += "<tr><td DragDrop='true'>";
			html += "<div class='Toolbar' onmousedown='UserControlSelectedOnMouseDown(this)'><a onclick='return ShowCellProperties(this.parentElement.parentElement)' href='#'><img style='border:0px' src='" + properties_image_path + "' alt='Set cell properties' /></a> <a onclick='return CollapseColumn(this.parentElement.parentElement)' href='#'><img style='border:0px' src='" + delete_image_path + "' alt='Remove this cell' /></a></div>";
			html += "</td></tr></table>";
		}
		
		dragobject.innerHTML = html;
		
		dragobject = null;
		
		SaveTableChanges();
	}
	else
	{
		document.body.removeChild(dragobject);
		
		dragobject = null;
	}
}

function DragDropOnMouseUp()
{
	if(dragobject)
	{
		switch(dragobject.type)
		{
			case "Table":
				DragUserControl("Table");
				break;
			
			case "Row":
				DragRow();
				break;
			
			case "Column":
				DragColumn();
				break;
			
			case "UserControl":
				DragUserControl("UserControl");
				break;
		}
	}
}

function DragDropOnMouseMove()
{
	if (dragobject)
	{
		if (window.event.clientX >= 0 && window.event.clientY >= 0)
		{
			dragobject.style.left = window.event.clientX + document.body.scrollLeft;
			dragobject.style.top = window.event.clientY + document.body.scrollTop;
		}
		window.event.returnValue = false;
		window.event.cancelBubble = true;
	}
}

function DeleteUserControl(element) //一级 当用户点选删除图标时用
{
	
	while(element!=null && element.tagName!='DIV')
	{
		element = element.parentElement;
	}
	
	if(element != null)
	{
		element.parentElement.removeChild(element);
		//CollapseColumn(element.parentElement);
		SaveTableChanges();
	}
	
}

function SelectUserControl(element)//一级 当用户点选修改时用
{
	var pos = "";
	
	while(element!=null && element.tagName!="DIV")
	{
		element = element.parentElement;
	}
	
	if(element!=null)
	{
		element.setAttribute("selected", "true");
		
		SaveTableChanges();
		/*
		pos = element.parentElement.parentElement.rowIndex + "$" + element.parentElement.cellIndex + "$";
		
		var index = -2;
		
		while(element!=null)
		{
			element = element.previousSibling;
			index++;
		}
		
		pos = pos + index;
				
		document.all.usercontrol_selected.value = pos;
		*/				
		__doPostBack(btnSelectUserControl,'');
	}
}

function SetStatus(element, disabled)
{
	if(element.children!=null && element.children.length > 0)
	{
		for(var i = 0; i<element.children.length; i++)
		{
			SetStatus(element.children(i));
		}
	}
	
	if(element.disabled != null)
	{
		element.disabled = !disabled;
	}
}

function ClickMe(element, disabled)
{
	element = element.parentElement.nextSibling;

	while(element!=null)
	{
		SetStatus(element, disabled);
		
		element = element.nextSibling;
	}
}

function FocusInput(element)
{
	element = element.parentElement.nextSibling;

	while(element!=null)
	{
		if(element.tagName=="INPUT" || element.tagName=="TEXTAREA")
		{
			element.focus();
			
			return;
		}
		
		element = element.nextSibling;
	}
}

function SelectMe(element)//一级 但没用到
{
	if(element.tagName=="INPUT")
	{
		if(element.type=="radio")
		{
			var radios = document.body.all.tags("INPUT");
			for(var i=0; i<radios.length; i++)
			{
				if(radios(i).type=="radio" && radios(i).name==element.name)
				{
					ClickMe(radios(i), !element.checked);
				}
			}
			
			ClickMe(element, element.checked);
			
			if(element.checked) FocusInput(element);
		}
		else if(element.type == "checkbox")
		{
			ClickMe(element, element.checked);
			
			if(element.checked) FocusInput(element);
		}
	}
}

function InitPage()//一级
{
	var fieldsets = document.body.all.tags("FIELDSET");
	
	for(var i=0; i<fieldsets.length; i++)
	{
		var element = fieldsets(i).children(0).children(0);
		if(element.tagName=="INPUT")
		{
			if(element.type=="radio" || element.type=="checkbox")
			{
				ClickMe(element, element.checked);
			}
		}
	}
	
	/*
	if(document.all.usercontrol_selected.value!="")
	{
		//var pos = document.all.usercontrol_selected.value;
		
		//var index = pos.split("$");
								
		//document.all.MainTable.rows[index[0]].cells[index[1]].children(Math.abs(index[2]) + 1).className = "UserControlEditing";
	}
	*/
}


