<h1>Encomendas Associadas</h1>
<table id="encomendaLink" class="z-table">
	<thead>
		<th>Cliente</th><th>Total Mercadoria</th><th>Estado Facturação</th><th>Estado Expedição</th><th>Data Encomenda</th><th>Última Modificação</th>
	</thead>
	<tbody>
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
				echo('<td id="estadoFact">'.$line['estadoFact'].'</td>');
				echo('<td id="estadoExp">'.$line['expedido'].'</td>');
				$date = explode("T", $line['date']);
				echo('<td id="dateEnc">'.$date[0].'</td>');
				echo('<td>'.$line['lastUpdated'].' dias</td>');
				echo('</tr>');
			}
		}
	} 
	catch(Exception $e){
		echo("<p> Warning: server return bad data. Please try again or contact admin!<p>");
	}
	?>
</tbody>
</table>

<?php include_once("filter_enc.php") ?>