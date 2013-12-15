<div id="midframe">
	<?php 
		try {
			if(isset($menu))
				switch($menu)
				{
					//TODO: acertar os endereços
					case 'enc':
						//$end = 'http://localhost/sinf/api/orders.php';
						$end = 'http://localhost:49174/api/orders?clienteId='.$_SESSION['user_id'];
						//echo($end);
						$request = callAPI($end);
						include ('includes/clients/listaEncomendas.php');
						break;
					case 'vend':
					//TODO: fazer vendedores/empregados
						//$end = 'http://localhost/sinf/api/vendedores.php';
						$end = 'http://localhost:49174/api/vendedores/';
						$request = callAPI($end);
						include ('includes/clients/listaVendedores.php');
						break;
					case 'cli':
						//$end = 'http://localhost/sinf/api/vendedores.php';
						$end = 'http://localhost:49174/api/clientes/';
						$request = callAPI($end);
						include ('includes/clients/listaClientes.php');
						echo('<p>EM CONTRUÇÃO</p>');
						break;
					case 'perf':
						//$end = 'http://localhost/sinf/api/pfile.php';
						$end = 'http://localhost:49174/api/utilizadores/get?id=clientes&userid='.$_SESSION['user_id'];
						$request = callAPI($end);
						include ('includes/clients/profileUtilizador.php');
						break;
					case 'encDet':
						//$end = 'http://localhost/sinf/api/vendedores.php';
						$end = 'http://localhost:49174/api/orders/';
						$request = callAPI($end);
						include ('includes/clients/encomendaDetalhada.php');
						break;
					case 'vendDet':
						$end = 'http://localhost:49174/api/utilizadores/get?id=vendedores&userid='.$_GET['userID'];
						$request = callAPI($end);
						include ('includes/clients/profileUtilizador.php');
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