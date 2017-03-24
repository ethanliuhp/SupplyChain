/**
 * 
 * @authors Your Name (you@example.org)
 * @date    2016-01-29 14:10:14
 * @version $Id$
 */

 !function($){
 	// bootstrap日期之间初始化
 	$.fn.datetimepicker.defaults = {
        pickDate: true, //en/disables the date picker
        pickTime: true, //en/disables the time picker
        useMinutes: true, //en/disables the minutes picker
        useSeconds: true, //en/disables the seconds picker
        useCurrent: true, //when true, picker will set the value to the current date/time
        minuteStepping:1, //set the minute stepping
        minDate:'1900-01-01', //set a minimum date
        maxDate: '2060-12-31', //set a maximum date (defaults to today +100 years)
        showToday: true, //shows the today indicator
        language:'zh-CN', //sets language locale
        defaultDate:'', //sets a default date, accepts js dates, strings and moment objects
        disabledDates:[], //an array of dates that cannot be selected
        enabledDates:[], //an array of dates that can be selected
        icons : {
        	time: 'glyphicon glyphicon-time',
        	date: 'glyphicon glyphicon-calendar',
        	up: 'glyphicon glyphicon-chevron-up',
        	down: 'glyphicon glyphicon-chevron-down'
        },
        format: 'yyyy-mm-dd',
        useStrict: false, //use “strict” when validating dates
        sideBySide: false, //show the date and time picker side by side
        daysOfWeekDisabled:[], //for example use daysOfWeekDisabled: [0,6] to disable weekends
        autoclose: true,
        minView: "month",
        maxView: "decade",
        todayBtn: true,
        pickerPosition: "bottom-left"
    };

    // 方法扩展
	$.extend({
		formatColumns:function(obj){
			var o = "";
			for(var key in obj){
				o+=key+"|";
			}
			if(o != ""){
				o = o.substr(0,o.length-1);
			}
			return o;
		}
	});
}(jQuery)
