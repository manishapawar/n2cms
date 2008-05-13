<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TabsInterface.ascx.cs" Inherits="N2.Templates.UI.TabPanel.TabsInterface" %>
<div>
	<%= CurrentItem.Text %>
</div>

<script type="text/javascript" src="/TabPanel/TabPanel.js"></script>
<script type="text/javascript">

jQuery.extend(jQuery.expr[':'], {
	until: function(el,i,args){
		if(i==0) jQuery.until_stop = false;
		if(jQuery.until_stop) return false;
		
		if($(el).is(args[3])) {
			jQuery.until_stop = true;
			return false;
		}

		return true;
	}
});

$(document).ready(function(){
	$("h2", "#main").each(function(i){
		$(this).hide()
			.nextAll(":until(h2)")
			.wrapAll("<div class='tabPanel' title='" + this.innerHTML + "'></div>");
	});
	$(".tabPanel").n2tabs();
});

</script>