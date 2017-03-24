<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StockIn.aspx.cs" Inherits="StockIn_StockIn" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>入库单</title>
    <style type="text/css">
    form,body{ background:#fff; width:100%; height:100%; margin:0px; padding:0px;} 
    
*{ margin:0; padding:0;} 
.box{ margin:20px; } 
.container{background:#FFF; width:100%; clear:both; margin-top:1px; _margin-top:-1px;} 
.sub-con{height:100%; width:100%;border:1px solid #3CF; background:#FFF; display:none;} 
.cur-sub-con{ display:block;} 
.sub-con iframe{ text-align:center;width:100%; height:100%; border:solid 0px black; } 
.nav{ width:100%; height:28px; margin-left:10px;} 
.nav ul{width:100%; height:28px;}  
.nav ul li{ list-style:none; display:inline;width:auto; height:28px;line-height:28px; text-align:center; float:left;margin-left:-1px;   } 
.nav ul li a{ background:#fff;border:1px solid #3CF; text-decoration:none; color:#000; height:28px;display:block;} 
.nav ul li a:hover{ background:#CCEDFB} 
.nav ul li a.cur{ z-index:9999;border-bottom:1px solid #FFF; color:#F00;} 
    </style>
     <script type="text/javascript" src="../JavaScript/jquery/jquery-1.4.2.min.js"></script>
   
</head>
<body>
    <form id="form1" runat="server">
    <div class="box"> 
        <div class="nav"> 
            <ul> 
              
                <li></li>
              
            </ul> 
          </div> 
          <div class="container"> 
         
          </div>
    </div> 
    </form>
     <script type="text/javascript">
        $(document).ready(function(){
           var doc= $(window);
            doc.resize=function(){
                var diff=80;
                var w=doc.width();
                var h=doc.height();
                $(".box .container").height(h-diff);
                $(".box .container .sub-con").height(h-diff);
                $(".box .container .sub-con iframe").height(h-diff);
                
            };
           doc.resize();
           createTab("列表","StockInList.aspx");
           createTab("详细信息","StockInfo.aspx");
         
           selectTab("列表");
           
           $(".nav li a").click(function(){//鼠标点击也可以切换 
           
            $(".cur").removeClass("cur"); 
            $(this).addClass("cur"); 
            var id=this.id ;
             $(".cur-sub-con").removeClass("cur-sub-con"); 
            $("#"+id+"_Content").addClass("cur-sub-con"); 
         
            }); 
        });
        function createTab(id,sUrl){
            //添加tab头
            var sHeaderTemplate= '<li><a href="javascript:void(0)" id="'+id+'" >&nbsp;&nbsp;'+id+' &nbsp;&nbsp; </a></li>' ;
            var nav=$(".box .nav ul");
            nav.append(sHeaderTemplate);  
            var container=$(".box .container");
            var sContentTemplate='<div class="sub-con " id="'+id+'_Content'+'"><iframe frameborder="0" id="'+id+'_Iframe'+'" src="'+sUrl+'"></iframe></div> ';
            container.append(sContentTemplate);
        }
        function selectTab(id){
            //选中tab头
            var selectAStyle="cur";
            var a=   $(".box .nav ul li ."+selectAStyle);
            if(a && a.length>0){
                a.removeClass(selectAStyle);
            }
            a=$("#"+id);
            if(a && a.length>0){
                a.addClass(selectAStyle);
            }
            //选中tab内容
            var selectDivStyle="cur-sub-con";
            var div=$(".box .container ."+selectDivStyle);
            if(div && div.length>0){
                div.removeClass(selectDivStyle);
            }
            div=$("#"+id+"_Content");
            if(div && div.length>0){
                div.addClass(selectDivStyle);
            }
            
        }
    </script>
</body>
</html>

