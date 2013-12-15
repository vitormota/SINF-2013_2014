$(document).ready(function (){
	if($('#encomendaLink').length > 0)
		setEncomendasLinks();
	if($('#vendedoresLink').length > 0)
		setVendedoresLink();
	if($('#clientesLink').length > 0)
		setClientesLink();
	$('#filters #idate').change(function(){filtros(); });
	$('#filters #fdate').change(function(){filtros(); });
	$('#filters #estadoFacturacao').change(function(){filtros();});
	$('#filters #estadoExpedicao').change(function(){filtros();});
});

function filtros(){

	var selectedField = $('#filters #idate').val();
	$('#encomendaLink tr').each(function(i) 
		{
			$(this).show();
		});

	if (selectedField != "")
	{
		$('#encomendaLink tbody tr').each(function(i) 
		{
			if(new Date($("#dateEnc",this).html()) <= new Date(selectedField) )
				$(this).hide();
		});
	}

	var selectedField2 = $('#filters #fdate').val();

	if (selectedField2 != "")
	{
		$('#encomendaLink tbody tr').each(function(i) 
		{
			if(new Date($("#dateEnc",this).html()) > new Date(selectedField2) )
				$(this).hide();
		});
	}

	var selectedField3 = $('#filters #estadoFacturacao :selected').val();
	console.log(selectedField3);

	if (selectedField3 != "")
	{
		$('#encomendaLink tbody tr').each(function(i) 
		{
			if($("#estadoFact",this).html() !== selectedField3 )
				$(this).hide();
		});
	}

	var selectedField4 = $('#filters #estadoExpedicao :selected').val();
	console.log(selectedField4);

	if (selectedField4 != "")
	{
		$('#encomendaLink tbody tr').each(function(i) 
		{
			if($("#estadoExp",this).html() !== selectedField4 )
				$(this).hide();
		});
	}
}


function setEncomendasLinks(){
	$('#encomendaLink tbody tr').each(function()
	{
		$(this).click(function(){
			window.location = "dashboard.php?menu=encDet&encID="+$(".hidden",this).html();
		});
		
	});
}

function setVendedoresLink(){
	$('#vendedoresLink tbody tr').each(function()
	{
		$(this).click(function(){
			window.location = "dashboard.php?menu=vendDet&userID="+$(".hidden",this).html();
		});
		
	});
}

function setClientesLink(){
	$('#clientesLink tbody tr').each(function()
	{
		$(this).click(function(){
			window.location = "dashboard.php?menu=cliDet&userID="+$(".hidden",this).html();
		});
		
	});
}