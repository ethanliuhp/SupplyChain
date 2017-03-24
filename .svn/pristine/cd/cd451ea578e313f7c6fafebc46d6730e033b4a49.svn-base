function upFile(arrArg){
        this.name=arrArg.name;
        this.title="";
 
        this.url=null;
      
        this.dialog=null;
        
        this.getHTML=function(){
            this.getUrl();
            var arrHTML=[];
            arrHTML.push('<div id="_dialogDivUpfile" style=" padding:0px; margin:0px;  overflow:hidden;display:none;">');
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
            switch(this.name.toLowerCase()){
                case "excel":{
                    this.width=400;//255,370
                    this.height=200;
                    this.title="请选择["+this.name +"]文件";
                    this.url="/UpFile/UpFile.aspx";
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
        
           this.divDialog=$( "#_dialogDivUpfile" );
            if(this.divDialog){
                this.divDialog.remove();
            }
           this.getHTML();
           this.divDialog=$(this.html);
           this.divDialog.attr("title",this.title);
           var self=this;
           this.divDialog[0].sure=function(data){
           //data={WebFilePath:"",FilePath:""}网页路径和物理路径
                if( arrArg.sure){
                    arrArg.sure(data);
                 }
                 
                self.dialog.dialog("close");
               //$("#_dialogDivUpfile").remove();
            };
            this.divDialog[0].close=function(data){
                if( arrArg.close){
                  arrArg.close(data);
                }
                 
                self.dialog.dialog("close");
               // $("#_dialogDivUpfile").remove();
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
            //this.divDialog.parent().appendTo('form:first');
        };
        //this.intialDialog(255,370);
        this.intialDialog(this.width,this.height);
        this.open=function(){ 
            this.dialog.dialog("open");
        };
    }