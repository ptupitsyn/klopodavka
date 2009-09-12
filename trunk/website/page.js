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
	'	<p><a href="http://www.pochta.ru" target="_blank"><img src="http://counters1.pochta.ru/klopodavka.nm.ru/" border="0" alt="почта"></a></P>'+
	'	<BR>'+
	'	<P></P>'+
	'	<BR>'+
	'	</TD>'+
	'	<TD align="left" valign="top" width="100%">'+
	'	<table cellpadding="30" cellspacing="0" border="1" width="100%" height="100%"><TR><TD width="100%" height="100%" background="pic/bg2.jpg">'
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
	'<a href="mailto:nAXAH1@gmail.com" title="E-Mail me">nAXAH1@gmail.com</a>'+
	'<BR><a href="http://ru.wikipedia.org/wiki/Клоподавка" title="Статья в Wiki"><img src="http://ru.wikipedia.org/favicon.ico"> &nbsp;ru.wikipedia.org/wiki/Клоподавка</a>'+
	'<BR><a href="http://code.google.com/p/klopodavka/" title="Google Code"><img src="http://google.com/favicon.ico"> &nbsp;code.google.com/p/klopodavka/</a>'+
	'</center>'+
	'</TD>'+
	'</TR>'+
	'</table>'
	);
}
