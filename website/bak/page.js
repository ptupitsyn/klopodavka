// Page structure generation script for KLOPODAVKA site
// Coded by nAXAH
// nAXAH@nm.ru

function header()
{
	document.write('<table width="95%" cellspacing="10"><!--  MAIN TABLE -->'+
	'<TR align="center" valign="middle" height="130">'+
	'	<TD width="150"><img src="pic/klp.jpg" width="150" height="130"></TD>'+
	'	<TD align="center" valign="middle" width="100%"><img src="pic/klopodavka.jpg"></TD>'+
	'</TR>'+
	'<TR>'+
	'	<TD width="150" align="center" valign="top">'+
	'	<!-- Menu output -->'+
	'	<script language="JavaScript1.2">generate_mainitems()</script>'+
	'	<!-- Here comes all info under menu -->'+
	'	<p><iframe height="100px" width="140px" src="counter.dhtml" frameborder="0"></iframe></P>'+
	'	<BR>'+
	'	<P></P>'+
	'	<BR>'+
	'	</TD>'+
	'	<TD align="left" valign="top" width="100%">'+
	'	<table cellpadding="30" cellspacing="0" border="1" width="100%" height="100%"><TR><TD width="100%" height="100%"  background="pic/bg.jpg">'
	);
	document.close();
}

function footer()
{
	document.write(
	'	</TD></TR></table>'+
	'	</TD>'+
	'</TR>'+
	'<TR>'+
	'<TD colspan="2" bordercolor="#000000">'+
	'<!-- FINAL PART -->'+
	'<center>'+
	'<FONT size="1px" color="#CCCCCC">[site is tested for compatibility in IE5, Opera6, Mozilla1]</FONT><BR><P></P>'+
	':: nAXAHsoft 2005 ::<BR>'+
	'<a href="mailto:nAXAH@nm.ru" title="E-Mail nAXAH">nAXAH@nm.ru</a>'+
	'</center>'+
	'</TD>'+
	'</TR>'+
	'</table>'
	);
}