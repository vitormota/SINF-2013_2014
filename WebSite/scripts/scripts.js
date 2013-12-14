$(document).ready(function (){
	if($('#encomendaLink').length > 0)
		setEncomendasLinks();
	if($('#vendedoresLink').length > 0)
		setVendedoresLink();
	if($('#clientesLink').length > 0)
		setClientesLink();
});

function setEncomendasLinks(){
	$('#encomendaLink tr').each(function()
	{
		$(this).click(function(){
			console.log("passei aqui");
			window.location = "dashboard.php?menu=encDet&encID="+$(".hidden",this).html();
		});
		
	});
}

function setVendedoresLink(){
	$('#vendedoresLink tr').each(function()
	{
		$(this).click(function(){
			console.log("passei aqui");
			window.location = "dashboard.php?menu=vendDet&userID="+$(".hidden",this).html();
		});
		
	});
}

function setClientesLink(){
	$('#clientesLink tr').each(function()
	{
		$(this).click(function(){
			console.log("passei aqui");
			window.location = "dashboard.php?menu=cliDet&userID="+$(".hidden",this).html();
		});
		
	});
}