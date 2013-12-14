<div id="midframe">
	<?php 
		try {
			if(isset($menu))
				switch($menu)
				{
					//TODO: acertar os endereços
					case 'enc':
						//$end = 'http://localhost/sinf/api/orders.php';
						$end = 'http://localhost:49174/api/orders/';
						$request = callAPI($end);
						include ('includes/admin/listaEncomendas.php');
						break;
					case 'vend':
					//TODO: fazer vendedores/empregados
						//$end = 'http://localhost/sinf/api/vendedores.php';
						$end = 'http://localhost:49174/api/utilizadores/get/vendedores/';
						$request = callAPI($end);
						include ('includes/admin/listaVendedores.php');
						break;
					case 'cli':
						//$end = 'http://localhost/sinf/api/vendedores.php';
						$end = 'http://localhost:49174/api/utilizadores/get/clientes/';
						$request = callAPI($end);
						include ('includes/admin/listaClientes.php');
						break;
					case 'encDet':
						//$end = 'http://localhost/sinf/api/vendedores.php';
						$end = 'http://localhost:49174/api/orders/';
						$request = callAPI($end);
						include ('includes/admin/encomendaDetalhada.php');
						break;
					case 'vendDet':
						$end = 'http://localhost:49174/api/utilizadores/get?id=vendedores&userid='.$_GET['userID'];
						$request = callAPI($end);
						include ('includes/admin/profileUtilizador.php');
						break;
					case 'cliDet':
						$end = 'http://localhost:49174/api/utilizadores/get?id=clientes&userid='.$_GET['userID'];
						$request = callAPI($end);
						include ('includes/admin/profileUtilizador.php');
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