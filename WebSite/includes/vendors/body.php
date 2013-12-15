<div id="midframe">
	<?php 
		try {
			if(isset($menu))
				switch($menu)
				{
					//TODO: acertar os endereços
					case 'enc':
						$end = 'http://localhost:49174/api/orders/';
						//echo($end);
						$request = callAPI($end);
						include ('includes/vendors/listaEncomendas.php');
						break;
					case 'cli':
						$end = 'http://localhost:49174/api/utilizadores/get/clientes/';
						$request = callAPI($end);
						include ('includes/vendors/listaClientes.php');
						break;
					case 'vendDet':
						$end = 'http://localhost:49174/api/utilizadores/get?id=vendedores&userid='.$_SESSION['user_id'];
						$request = callAPI($end);
						include ('includes/vendors/profileUtilizador.php');
						break;		
					case 'encDet':
						//$end = 'http://localhost/sinf/api/vendedores.php';
						$end = 'http://localhost:49174/api/orders/';
						$request = callAPI($end);
						include ('includes/vendors/encomendaDetalhada.php');
						break;				
					case 'cliDet':
						$end = 'http://localhost:49174/api/utilizadores/get?id=clientes&userid='.$_GET['userID'];
						$request = callAPI($end);
						include ('includes/vendors/profileUtilizador.php');
						break;
					default:
						echo('<h1>Opção Inválida</h1>');
						break;
				}
		}
		catch(Exception $e){
			$request = FALSE;
			echo("<p> Warning: Exception catch server probably offline!</p>");
		}	 
	?>
</div>
