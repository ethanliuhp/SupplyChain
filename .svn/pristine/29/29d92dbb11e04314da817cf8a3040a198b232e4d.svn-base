Type.registerNamespace("com.think3.PLM.Custom");

com.think3.PLM.Custom.PLMCustomEntity = function(objectID,customName) {
    this._objectID = objectID;
    this._customName = customName;
}


com.think3.PLM.Custom.PLMCustomEntity.prototype = {
    
    getObjectID: function() {
        return this._objectID;
    },
    
    getCustomName: function() {
        return this._customName;
    }
    
}
com.think3.PLM.Custom.PLMCustomEntity.registerClass('com.think3.PLM.Custom.PLMCustomEntity', null, Sys.IDisposable);
if (typeof(Sys) !== 'undefined') Sys.Application.notifyScriptLoaded();



function GetEntities(instanceString)
{
   var iStr=instanceString.toString();
   var pts=iStr.split(',');
   if(pts.length==2)
   {
      var objIDs=pts[0].split(";");
      var customNames=pts[1].split(";");
      var objCount=objIDs.length;
      var entities=new Array();
      for(var i=0;i<objCount;i++)
      {
         var entity=new com.think3.PLM.Custom.PLMCustomEntity(objIDs[i],customNames[i]);
         entities[i]=entity;
      }
      return entities;
   }
   return null;
}