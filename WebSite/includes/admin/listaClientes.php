<h1>Listagem de Clientes</h1>
<table id="clientesLink" class="z-table">
	<thead>
		<th>Nome</th><th>Contacto</th>
	</thead>
	<?PHP 
	try{
		if($request)
		{		
			//print_r($request);
			foreach($request as $line){
				//TODO trocar os nomes das variaveis para as que são devolvidas pela api da primavera
				echo('<tr>');
				echo('<td class="hidden">'.$line['Cod'].'</td>');
				echo('<td>'.$line['Nome'].'</td>');
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