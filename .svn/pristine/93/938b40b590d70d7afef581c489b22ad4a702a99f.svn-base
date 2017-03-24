/**
 * 
 * @authors Your Name (you@example.org)
 * @date    2016-03-07 09:09:58
 * @version $Id$
 */

;(function($){
	$.extend({
		loadUser:function(obj){
			$.ajax({
				url:"GetSession.ashx",
				success:function(data){
					obj.search(data);
				}
			});
		}
	});
})(jQuery);
