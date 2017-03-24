function SelectDialog(name,sure,close){
        this.name=name;
        this.title="";
 
        this.url=null;
      
        this.dialog=null;
        
        this.getHTML=function(){
            this.getUrl();
            var arrHTML=[];
            arrHTML.push('<div id="_dialogDivDetail" style=" padding:0px; margin:0px;  overflow:hidden;display:none;">');
            arrHTML.push('<iframe id="ifrShowDialog"   style=" overflow:hidden; height:100%; width:100%; margin:0px; padding:0px;" src="'+this.url+'" frameborder="0" scrolling="auto" marginheight="0" marginwidth="0"></iframe>');
            arrHTML.push("</div>");
            this.html=arrHTML.join("");
        };
         this.getBasePath=function(){
        
              //var location = (window.location + '').split('/');
              //var basePath1 = location[0] + '//' + location[2] + '/' + location[3];
              var url=window.location + '';
              url=url.substr(0,url.lastIndexOf('/'));
              url=url.substr(0,url.lastIndexOf('/'));
              url=url.substr(0,url.lastIndexOf('/'));
            
             
              return url;
         };
         this.getUrl=function(){
            switch(this.name){
                case "会计科目":{
                    this.width=305;//255,370
                    this.height=440;
                    this.title=this.name+"选择";
                    this.url="/Show/ShowAccountTitleTree.aspx";
                    break;
                }
                case "会计科目管理费用":{
                    this.width=305;//255,370
                    this.height=440;
                    this.title="管理费用"+"选择";//this.name+"选择";
                    this.url="/Show/ShowAccountTitleTree.aspx?Code=6602";
                    break;
                }
                case "组织":{
                    this.width=305;//255,370
                    this.height=440;
                    this.title=this.name+"选择";
                    this.url="/Show/ShowOrgInfo.aspx";
                    break;
                }
                  case "人员":{
                    this.width=700;//255,370
                    this.height=370;
                    this.title=this.name+"选择";
                    this.url="/Show/ShowPerson.aspx?IsSingle=true";
                    break;
                }
                case "供应商":{
                    this.width=710;//255,370
                    this.height=470;
                    this.title=this.name+"选择";
                    this.url="/Show/ShowSupply.aspx?IsSingle=true";
                    break;
                }
                case "客户":{
                    this.width=700;//255,370
                    this.height=370;
                    this.title=this.name+"选择";
                    this.url="/Show/ShowCustom.aspx?IsSingle=true";
                    break;
                }
                default:{
                    this.title=null;
                    this.url=null;
                }
            }
            if(this.url){
                this.url=this.getBasePath()+this.url;
            }
         };
         this.intialDialog= function (){
           this.divDialog=$( "#_dialogDivDetail" );
            if(this.divDialog){
                this.divDialog.remove();
            }
           this.getHTML();
           this.divDialog=$(this.html);
           this.divDialog.attr("title",this.title);
           var self=this;
           this.divDialog[0].sure=function(data){
                if( sure){
                    sure(data);
                 }
                self.dialog.dialog("close");
            };
            this.divDialog[0].close=function(data){
      
                if( close){
                  close(data);
                }
                self.dialog.dialog("close");
            };
            this.divDialog.width(this.width);
            this.divDialog.height(this.height);
            this.dialog=this.divDialog.dialog({
            autoOpen: false,
            height: this.height,
            width:this.width,
            modal: true,
            draggable: true,  
            resizable: true,  
            closed:true,  
            show: 'Transfer',  
            hide: 'Transfer'
             }); 
            this.divDialog.parent().appendTo('form:first');
        };
        //this.intialDialog(255,370);
        this.intialDialog(this.width,this.height);
        this.open=function(){ 
            this.dialog.dialog("open");
        };
    }