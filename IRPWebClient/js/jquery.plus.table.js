/**
 * 
 * @authors Dancy Sun (you@example.org)
 * @date    2016-03-02 09:00:40
 * @version $Id$
 */

;(function($){
	$.fn.extend({
		table : function(params){
			var tableData = [];										// 表数据
			var $index = 0;											// 记录表数据的序号
			var table = this;										// 当前table标签的jquery对象
			var hidTag = $(params.output);							// 最终数据输出的隐藏于控件
			var frontColumns = 0;									// 列表前有几列
			var $delRow;											// 待删除的数据行
			if(params.isRowNumber)frontColumns++;					// 是否自动生成序号
			if(params.isOperate)frontColumns++;						// 是否需要操作栏
			
			var tbody = table.find("tbody");						// 获取表主体内容
			if(tbody.length == 0){	
				tbody = $("<tbody>");
				table.append(tbody);
			}

			var tfoot = table.find("tfoot");						// 获取表尾
			if(tfoot.length == 0){	
				tfoot = $("<tfoot>");
				tfoot.append('<tr class="row-foot"><td colspan="'+params.columns.length+'">+</td><tr>');
				table.append(tfoot);
			}

			$.ajax({												// 获取远程数据
				url:params.url,
				data:params.data,
				type:"post",
				dataType:"json",
				success:function(data){
					// 处理返回值，格式化后保存在tableData中
					for (var i = 0; i < data.length; i++) {
						// var temp = {$index:$index};
						// for(var j in params.columns){
						// 	temp[params.columns[j]] = data[i][params.columns[j]];
						// }
						// tableData.push(temp);
						data[i].$index = $index;
						tableData.push(data[i]);
						$index++;
					};
					// 操作DOM显示数据
					reloadView();
					// 保存对象字符串
					saveData();
				}
			});

			table.on("click",".row-foot",function(){				// 新添加一行
				// 默认带出上一行的数据
				var newData = {$index:$index++};
				if (tableData.length > 0) {
					newData = $.extend({},tableData[tableData.length-1],newData);
					newData[params.PK] = "";
				}
				tableData.push(newData);
				var tr = $("<tr>");
				tr.addClass("row-data");
				tr.attr("data-index",newData.$index);
				var firstTd;
				for(var i = 0;i < params.columns.length;i++){
					var newTd = $("<td>");
					newTd.attr("name",params.columns[i])
					newTd.text(newData[params.columns[i]]);
					tr.append(newTd);
					if(i == 0)firstTd = newTd;
				}
				tbody.append(tr);
				editData(firstTd);
			})
			.on("dblclick",".row-data td",function(){				// 单元格双击
				editData($(this));
			})
			.on("contextmenu",".row-data td",function(e){
				$delRow = $(this).closest("tr");
				delLayer(e);
				$(document).on("click",function(){
					closeDelLayer();
				});
			});

			function editData(td){									// 编辑单元格，根据配置类型加载指定的标签
				var tdValue = td.text();
				var tdName = td.attr("name");
				var type = params.editRows[tdName];
				if(!type)return;
				var tag = getTagByType(type,tdValue);
				td.html("");
				td.append(tag);
				tag.focus();
				tag.blur(tagBlur);
				tag.select();
				tag.keydown(tagTab);
			}
			
			function reloadView () {								// 重新加载数据
				tbody.html("");										// 清空原数据
				for(var item in tableData){							// 重新加载
					var tr = $("<tr>");
					tr.addClass("row-data");
					tr.attr("data-index",tableData[item].$index);
					$.each(params.columns,function(){
						tr.append('<td name="'+this+'">'+tableData[item][this]+'</td>');
					});
					tbody.append(tr);
				}
			}

			function tagBlur(){									// 控件离焦后触发
				var tag = $(this);
				setTimeout(function(){
					var $index = tag.closest("tr").data("index");
					var $name = tag.closest("td").attr("name");
					var data;
					$.each(tableData,function(){
						if(this.$index == $index){
							data = this;
							return;
						}
					})
					data[$name] = tag.val();
					tag.closest("td").text(tag.val());
					tag.remove();
					// 保存对象字符串
					saveData();
				},100);
			}

			function tagTab(e){										// 输入tab之后跳转到下一个输入框
				if(e.which == 9){
					var tag = $(this);
					var next = tag.closest("td").next();
					if(next.length > 0){
						next.dblclick();
						e.preventDefault();
					}	
				}
			}

			function saveData(){								// 保存对象字符串
				hidTag.val($.parseString(tableData));
			}

			function delLayer(e){								// 弹出删除选项
				var div = $("<div>");
				div.addClass("table-item-delete");
				var p = $("<span title='删除'>");
				p.addClass("table-item-delete")
				p.html("&times;");
				div.append(p);
				div.css({top:e.pageY + 2,left:e.pageX + 2})
				$("body").append(div);
				e.preventDefault();
			}

			function delItem(e){									// 删除数据项
				var $index = $delRow.data("index");
				var numIndex = 0;
				$.each(tableData,function(n,obj){
					if($index == obj.$index){
						numIndex = n;
						return false;
					}
				});
				tableData.splice(numIndex,1);
				$delRow.remove();
				saveData();
				closeDelLayer();
				e.stopPropagation();
			}

			function closeDelLayer(){							// 关闭删除层
				$(".table-item-delete").remove();
				$(document).off("click");
			}

			function getTagByType(type,val){					// 编辑表格创建指定的标签
				var result;
				if (typeof type == "string") {
					switch(type){
						case "text":
						result = $('<input type="text">');
						break;
						case "number":
						result = $('<input type="number" value="0">');
						if(!val)val = 0;
						break;
						case "date":
						result = $('<input type="text" class="Wdate" onfocus="new WdatePicker(this,\'%Y-%M-%D\',true)">')
						if(!val){
							var curTime = new Date();
							var year = curTime.getFullYear();
							var month = curTime.getMonth() + 1;
							if(month.toString().length == 1)month = "0"+month;
							var day = curTime.getDate();
							if(day.toString().length == 1)day = "0"+day;
							val = year + "-" + month + "-" + day;
						}
						break;
						default:
						break;
					}
					result.val(val);
				}
				else if(typeof type == "object"){
					switch(type[0]){
						case "dropdown":
						result = $('<select>');
						$.each(type[1],function(){
							var option = $('<option value="'+this+'">'+this+'</option>');
							if(this == val)option.attr("selected","selected");
							result.append(option);
						});
						break;
						default:
						break;
					}
				}
				return result;
			}

			$("body")											// 绑定body单击事件，隐藏删除框
			.on("click",".table-item-delete",function(e){
				delItem(e);
			});
		}

	});

	// 全局方法扩展
	$.extend({						
		parseString:function(o){								// 
			if(JSON){		
				return JSON.stringify(o);
			}
			else{
				if (o == undefined) {
                    return "";
                }
                var r = [];
                if (typeof o == "string") return "\"" + o.replace(/([\"\\])/g, "\\$1").replace(/(\n)/g, "\\n").replace(/(\r)/g, "\\r").replace(/(\t)/g, "\\t") + "\"";
                if (typeof o == "object") {
                    if (!o.sort) {
                        for (var i in o)
                            r.push("\"" + i + "\":" + objConvertStr(o[i]));
                        if (!!document.all && !/^\n?function\s*toString\(\)\s*\{\n?\s*\[native code\]\n?\s*\}\n?\s*$/.test(o.toString)) {
                            r.push("toString:" + o.toString.toString());
                        }
                        r = "{" + r.join() + "}"
                    } else {
                        for (var i = 0; i < o.length; i++)
                            r.push(objConvertStr(o[i]))
                        r = "[" + r.join() + "]";
                    }
                    return r;
                }
                return o.toString().replace(/\"\:/g, '":""');
            }
		}
	});
})(jQuery);