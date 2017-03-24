//
// Jquery.jwindow 0.01.1
// 窗口插件
// 需要 jquery.interface.js 与 jquery.js 的支持才可以用
// By Wathon Team.
// WebSite: http://www.wathon.com
// ChangeLog: http://www.wathon.com/opensource/js/jwindow/20070902
//

/*************** Example *******************
[HTML]
<button id="button1">显示窗口</div>
<div class="window " id="panelWindow">
<div class="title">Title<span class="buttons"><span class="close"></span></span></div>
<div class="content">
Message Content
</div>
<div class="status"><span class="resize"></span></div>
</div>
</div>


[CSS]
.window { 
padding:0px;
border:1px solid #75A0CA;
background:#FFF;
position:absolute;
z-index:20;	
width: 400px;
top: 17%;
left: 30%;
display:none;
}

* html .window {
position: absolute;
top: expression((document.documentElement.scrollTop || document.body.scrollTop) + Math.round(17 * (document.documentElement.offsetHeight || document.body.clientHeight) / 100) + 'px');
}

.window .title { background:#ACC6DF; padding:5px;}
.window .title .buttons { float:right;margin-top:-16px; _margin-top:-20px; _margin-right:3px;}
* + html .window .title .buttons {margin-top:-18px; margin-right:3px;}
.window .title .buttons .min { cursor:pointer; display:block; float:left; background:#F5714E url("../images/window_min.gif") top left no-repeat; border:1px solid #A7190F; height:10px; width:10px; padding:1px;margin-right:3px;_padding-bottom:0px;}
.window .title .buttons .close {cursor:pointer; display:block; float:left; background:#F5714E url("../images/window_close.gif") top left no-repeat; border:1px solid #A7190F; height:10px; width:10px; padding:1px;_padding-bottom:0px;}
.window .title .buttons .hover { background-color:#EF947D;}
.window .title .status .resize { color: }
.window .content { padding:8px;overflow:hidden;}

.transferer
{
border: 1px solid #EEE;
background-color: #F9F9F9;
opacity:0.7;
filter:alpha(opacity=30);
z-index:99999;
}


[Javascript]
$(document).ready(function(){
$("#button1").click(function(){
$("#panelWindow").jWindowOpen({
modal:true,
center:true,
drag:"#window .title",
close:"#window .title .close",
closeHoverClass:"hover",
transfererFrom:"#button1",
transfererClass:"transferer"
});
});	
})
*************** Example *******************/

jQuery.fn.jWindowOpen = function(s) {
    defaults = {
        modal: false,
        center: false,
        drag: "",
        close: "",
        closeHoverClass: "",
        transfererFrom: "",
        transfererClass: ""
    }

    $.extend(defaults, s);

    jWindow.init();

    //居中
    if (defaults.center) {
        var frm = (this);
        jWindow.moveCenter(frm);
        $(document).scroll(function() {
            jWindow.moveCenter(frm);
        });
        $(document).resize(function() {
            jWindow.moveCenter(frm);
        });
    }

    //模式窗口
    if (defaults.modal) {
        jWindow.showOverlayer();
    }

    //是否可移动
    if (defaults.drag) {
        jWindow.setMoveable(this, defaults.drag);
    }

    //关闭按钮
    if (defaults.close != "") {

        var frm = this;
        $(defaults.close).bind("click", function() {
            jWindow.close(frm);
        });

        if (defaults.closeHoverClass != "") {
            $(defaults.close).bind("mouseover", function() {
                $(this).addClass(defaults.closeHoverClass);
            });

            $(defaults.close).bind("mouseout", function() {
                $(this).removeClass(defaults.closeHoverClass);
            });
        }
    }

    //显示窗口
    //是否有状态
    if (defaults.transfererFrom != "" && defaults.transfererClass != "") {
        var frm = $(this);
        $(defaults.transfererFrom).TransferTo({ to: this, className: defaults.transfererClass, duration: 400, complete: function() {
            frm.show();
        }
        });

    } else {
        $(this).show();
    }
};

jQuery.fn.jWindowClose = function() {
    jWindow.close(this);
}

var jWindow = {
    arrayPageSize: new Array(0, 0, 0, 0),
    arrayPageScroll: new Array(0, 0, 0, 0),

    //初始化，要使用必须先运行此方法
    //init()
    //Results none
    //
    init: function() {
        arrayPageScroll = this.getPageScroll();
        arrayPageSize = this.getPageSize();
    },

    //显示modal层
    //showOverlayer()
    //Results none
    //
    showOverlayer: function() {
        this.removeOverlayer();
        var layer = "<div id=\"_______overlayer\" style=\"position:absolute;z-index:9990;top:0px; left:0px;-moz-opacity:0.7; opacity:0.7;filter:alpha(opacity=30);background:#FFFFFF;\"></div>";
        $("body").append(layer);
        var overlayer = $("#_______overlayer");
        overlayer.css("top", "0px");
        overlayer.css("left", "0px");
        overlayer.css("width", self.innerHeight ? "100%" : arrayPageSize[0] + "px");
        overlayer.css("height", (self.innerHeight ? arrayPageSize[1] : arrayPageSize[1] + 30) + "px");
    },

    //关闭
    //close(窗口的jquery Dom)
    //Results none
    //
    close: function(frm) {
        frm.hide();
        frm.DraggableDestroy();
        this.removeOverlayer();
    },

    //清除Modal层
    //removeOverlayer()
    //Returns none
    //
    removeOverlayer: function() {
        var overlayer = $("#_______overlayer");
        if (overlayer != null) {
            overlayer.remove();
        }
    },

    //移动窗口到
    //moveCenter(窗口的jquery Dom)
    //Returns none
    //
    moveCenter: function(frm) {
        arrayPageScroll = this.getPageScroll();
        arrayPageSize = this.getPageSize();
        var frmLeft = (arrayPageSize[2] - frm.width()) / 2;
        var frmTop = arrayPageScroll[1] + ((arrayPageSize[3] - frm.height()) / 2);
        frm.css("left", frmLeft + "px");
        frm.css("top", frmTop + 'px');
    },

    //设置为可移动的
    //
    //setMoveable(窗口的jquery Dom,标题的对象名称)
    //Returns none
    //
    setMoveable: function(frm, title) {
        frm.Draggable({
            zIndex: 1000,
            handle: title,
            opacity: 0.7
        });
    },


    //取得页面大小属性
    //
    // getPageScroll()
    // Returns array with x,y page scroll values.
    // Core code from - quirksmode.org
    //
    getPageScroll: function() {

        var yScroll;

        if (self.pageYOffset) {
            yScroll = self.pageYOffset;
        } else if (document.documentElement && document.documentElement.scrollTop) {	 // Explorer 6 Strict
            yScroll = document.documentElement.scrollTop;
        } else if (document.body) {// all other Explorers
            yScroll = document.body.scrollTop;
        }

        arrayPageScroll = new Array('', yScroll)
        return arrayPageScroll;
    },



    //
    // getPageSize()
    // Returns array with page width, height and window width, height
    // Core code from - quirksmode.org
    // Edit for Firefox by pHaez
    //
    getPageSize: function() {

        var xScroll, yScroll;

        if (window.innerHeight && window.scrollMaxY) {
            xScroll = document.body.scrollWidth;
            yScroll = window.innerHeight + window.scrollMaxY;
        } else if (document.body.scrollHeight > document.body.offsetHeight) { // all but Explorer Mac
            xScroll = document.body.scrollWidth;
            yScroll = document.body.scrollHeight;
        } else { // Explorer Mac...would also work in Explorer 6 Strict, Mozilla and Safari
            xScroll = document.body.offsetWidth;
            yScroll = document.body.offsetHeight;
        }

        var windowWidth, windowHeight;
        if (self.innerHeight) {	// all except Explorer
            windowWidth = self.innerWidth;
            windowHeight = self.innerHeight;
        } else if (document.documentElement && document.documentElement.clientHeight) { // Explorer 6 Strict Mode
            windowWidth = document.documentElement.clientWidth;
            windowHeight = document.documentElement.clientHeight;
        } else if (document.body) { // other Explorers
            windowWidth = document.body.clientWidth;
            windowHeight = document.body.clientHeight;
        }

        // for small pages with total height less then height of the viewport
        if (yScroll < windowHeight) {
            pageHeight = windowHeight;
        } else {
            pageHeight = yScroll;
        }

        // for small pages with total width less then width of the viewport
        if (xScroll < windowWidth) {
            pageWidth = windowWidth;
        } else {
            pageWidth = xScroll;
        }


        arrayPageSize = new Array(pageWidth, pageHeight, windowWidth, windowHeight)
        return arrayPageSize;
    }
}	