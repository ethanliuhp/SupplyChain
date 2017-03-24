 $(window).ready(function(){
           $(window).resize(function(){
                var w=$(window).width()-10;
                var h=$(window).height();
                var main=$("#divMain");
                var tool=$("#divTool");
                var content=$("#divContent");
                var masterList=$("#divMasterList");
                var detailList=$("#divDetailList");
                if(main.length>0){
                    main.width(w);
                    main.height(h);
                   content.width(w-10);
                   content.height(h-tool.height());
                   masterList.height(parseInt( (h-tool.height())*0.3));
                    masterList.width(w-10);
                    detailList.height(parseInt( (h-tool.height())*0.7));
                    detailList.width(w-10);
                   //设置主表列表大小
                   
              
                  var masterListFieldset= $("#divMasterList fieldset");
                  var masterListFrame=$("#divMasterList fieldset iframe");
                  masterListFieldset.height(masterList.height());
                   masterListFieldset.width(w-12);
                  masterListFrame.height(masterList.height()-30);
                   masterListFrame.width(w-14);
                   //设置明细列表大小
                   var detailListFieldset= $("#divDetailList fieldset");
                  var detailListFrame=$("#divDetailList fieldset iframe");
                  detailListFieldset.height(detailList.height()-15);
                   detailListFieldset.width(w-12);
                  detailListFrame.height(detailList.height()-50);
                detailListFrame.width(w-14);
                }
           });
           $(document).resize();
           $("#divList").show();
           $("#divListTool").show();
           $("#btnBack").click(function(){
                $("#divList").show();
                $("#divDetailInfo").hide();
                $("#divListTool").show();
                $("#divDetailInfoTool").hide();
           });
           $("#btnDetailInfo").click(function(){
                $("#divList").hide();
                $("#divDetailInfo").show();
                $("#divListTool").hide();
                $("#divDetailInfoTool").show();
           });
        });