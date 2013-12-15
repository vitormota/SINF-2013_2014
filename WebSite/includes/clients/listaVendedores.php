<h1>Listagem de Vendedores</h1>
<table id="vendedoresLink" class="z-table">
	<thead>
		<th>Nome</th><th>Email</th><th>Contacto</th>
	</thead>
	<?PHP 
	try{
		if($request)
		{		
			foreach($request as $line){
				//TODO trocar os nomes das variaveis para as que são devolvidas pela api da primavera
				echo('<tr>');
				echo('<td class="hidden">'.$line['Cod'].'</td>');
				echo('<td>'.$line['Nome'].'</td>');
				echo('<td>'.$line['Email'].'</td>');
				echo('<td>'.$line['Telefone'].'</td>');
				echo('</tr>');
			}
		}
	} 
	catch(Exception $e){
		echo("<p> Warning: server return bad data. Please try again or contact admin!<p>");
	}
	?>
</table>