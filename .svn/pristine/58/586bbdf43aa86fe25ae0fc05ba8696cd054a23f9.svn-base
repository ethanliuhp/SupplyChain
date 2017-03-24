/**
 * 
 * @authors Dancy sunxsh (you@example.org)
 * @date    2016-03-04 08:35:44
 * @version $Id$
 */

(function($){
	// jquery全局方法扩展
	$.extend({
		getUrlParam : function(name){  					// 获取url参数
	        var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)");  
	        var r = window.location.search.substr(1).match(reg);  
	        if (r!=null) return unescape(r[2]); return null;  
    	},
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

	// 扩展datagrid方法
	$.extend($.fn.datagrid.methods, {
        editCell: function (jq, param) {
            return jq.each(function () {
                var opts = $(this).datagrid('options');
                var fields = $(this).datagrid('getColumnFields', true).concat($(this).datagrid('getColumnFields'));
                for (var i = 0; i < fields.length; i++) {
                    var col = $(this).datagrid('getColumnOption', fields[i]);
                    col.editor1 = col.editor;
                    if (fields[i] != param.field) {
                        col.editor = null;
                    }
                }
                $(this).datagrid('beginEdit', param.index);
                for (var i = 0; i < fields.length; i++) {
                    var col = $(this).datagrid('getColumnOption', fields[i]);
                    col.editor = col.editor1;
                }
            });
        }
    });
})(jQuery);  
