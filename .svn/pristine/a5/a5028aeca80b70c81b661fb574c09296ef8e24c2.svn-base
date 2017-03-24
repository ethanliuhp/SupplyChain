function dataGrid(arg){
    var self=this;
    this._funBackData=arg.funBackData;

    this.editIndex =null;
    this.id=arg.id;
    this.title=arg.title;
    this.table=$("<table style=' width:100%;padding:0px; margin:0px; height:400px;'></table>");
    $("#"+this.id).append(this.table);
    this.table.addClass("easyui-datagrid");

   if( arg.width){this.table.width(arg.width);}
   if( arg.height){ this.table.height(arg.height);}
  
   this.loadData=function(data){    
       this.table.datagrid("loadData",data);
   }
   this.getSelectRow=function(){
      var row=this.table.datagrid('getSelected');
      return row;
   };
   //获取当前编辑状态的列表的
   this.getEditor=function(columnName){
        return this.table.datagrid("getEditor", {index:this.editIndex,field:columnName});
   };
   //获取列表行
   this.getRows=function(){
        return this.table.datagrid("getRows");
   };
   this.onRowClick=function(rowIndex, rowData){
       //alert("onRowClick");
       self.beginEditRow(rowIndex);
   };
   this.onDblClickCell=function(rowIndex, field, value){
      
         //alert("onDblClickCell");
   };
   this.onClickCell=function(rowIndex, field, value){
   
           // alert("onClickCell");
   
   };
   //将相应行设置为编辑状态
   this.beginEditRow=function(index){
   
        if(this.editIndex!=index){
            if(this.endEditRow()){
                this.table.datagrid("selectRow",index).datagrid("beginEdit",index);
                this.editIndex=index;
            }
            else{
                this.table.datagrid("selectRow",this.editIndex);
            }
       }
   };
   //编辑结束
   this.endEditRow=function(){
        var result=false;
        if(this.editIndex==null) {
            result= true;
        }
        else{
            if( this.validateRow(this.editIndex)){
                var oRowData=null;
                var target=null;
                var fieldName=null;
                var sValue=null;
                for(var iColumn=0;iColumn<this.option.columns[0].length;iColumn++){
                    if(this.option.columns[0][iColumn].setValue){
                       fieldName=this.option.columns[0][iColumn].field;
                       if(fieldName){
                           oRowData=this.getRows()[this.editIndex];
                           target=this.getEditor( fieldName).target;
                           sValue=this.option.columns[0][iColumn].setValue(this,target,oRowData);
                          // debugger;
                          // oRowData[fieldName]=sValue;
                       } 
                    }
                }
                this.table.datagrid("endEdit",this.editIndex);
                this.editIndex=null;
                result= true;
            }
            else{   
                result=false;
            }
        }
        return result;
   };
   this.validateRow=function(index){
        return this.table.datagrid("validateRow",this.editIndex);
   };
   this.option={
	      columns:arg.columns,
	      title:arg.title||'列表',
	      iconCls:arg.iconCls||'icon-edit',
	      singleSelect:arg.singleSelect==undefined? true:arg.singleSelect,
	      rownumbers:arg.rownumbers==undefined?true:arg.rownumbers,
	      onClickRow:arg.onClickRow||this.onRowClick,
	      //onDblClickCell: arg.onDblClickCell||this.onDblClickCell,
	      onClickCell:arg.onClickCell||this.onClickCell
	    };
   this.intial=function(){
 
     var gid=  this.table.datagrid(
	          this.option
       );
   };
   this._operateRow=function(sFlag,sID){
   debugger;
   };
   this.addOperateColumn=function(){
     
   };
   this.intial();
   return this;
}
