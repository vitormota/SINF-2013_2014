<h1>Encomendas Associadas</h1>
<table class="z-table">
	<thead>
		<th>Cliente</th><th>Total Mercadoria</th><th>Estado Facturação</th><th>Estado Expedição</th><th>Data</th>
	</thead>
	<?PHP 
	try{
		if($resquest)
		{
			//print_r($resquest);
			foreach($resquest as $line){
				//TODO trocar os nomes das variaveis para as que são devolvidas pela api da primavera
				echo('<tr>');
				echo('<td>'.$line['CodClient'].'</td>');
				echo('<td>'.$line['totalMerc'].'</td>');
				echo('<td>'.$line['estadoFact'].'</td>');
				echo('<td>'.$line['expedido'].'</td>');
				echo('<td>'.$line['date'].'</td>');
				echo('</tr>');
			}
		}
	} 
	catch(Exception $e){
		echo("<p> Warning: server return bad data. Please try again or contact admin!<p>");
	}
	?>
</table>