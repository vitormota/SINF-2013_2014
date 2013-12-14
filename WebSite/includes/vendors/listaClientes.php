<h1>Listagem de Clientes</h1>
<table class="z-table">
	<thead>
		<th>Nome</th><th>Contribuinte</th>
	</thead>
	<?PHP 
	try{
		if($resquest)
		{		
			//print_r($resquest);
			foreach($resquest as $line){
				//TODO trocar os nomes das variaveis para as que são devolvidas pela api da primavera
				echo('<tr>');
				echo('<td>'.$line['NomeCliente'].'</td>');
				echo('<td>'.$line['NumContribuinte'].'</td>');
				echo('</tr>');
			}
		}
	} 
	catch(Exception $e){
		echo("<p> Warning: server return bad data. Please try again or contact admin!<p>");
	}
	?>
</table>