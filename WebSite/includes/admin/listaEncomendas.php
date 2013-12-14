<h1>Encomendas Associadas</h1>
<table id="encomendaLink" class="z-table">
	<thead>
		<th>Cliente</th><th>Total Mercadoria</th><th>Estado Facturação</th><th>Estado Expedição</th><th>Última Modificação (dias)</th>
	</thead>
	<?PHP 
	try{
		if($request)
		{
			//print_r($request);
			foreach($request as $line){
				//TODO trocar os nomes das variaveis para as que são devolvidas pela api da primavera
				echo('<tr>');
				echo('<td class="hidden">'.$line['docNum'].'</td>');
				echo('<td>'.$line['CodClient'].'</td>');
				echo('<td>'.$line['totalMerc'].'</td>');
				echo('<td>'.$line['estadoFact'].'</td>');
				echo('<td>'.$line['expedido'].'</td>');
				echo('<td>'.$line['lastUpdated'].'</td>');
				echo('</tr>');
			}
		}
	} 
	catch(Exception $e){
		echo("<p> Warning: server return bad data. Please try again or contact admin!<p>");
	}
	?>
</table>