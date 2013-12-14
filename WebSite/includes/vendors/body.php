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
						$resquest = callAPI($end);
						include ('includes/vendors/listaEncomendas.php');
						echo('<p>EM CONTRUÇÃO</p>');
						break;
					case 'cli':
						//$end = 'http://localhost/sinf/api/vendedores.php';
						$end = 'http://localhost:49174/api/clientes/';
						$resquest = callAPI($end);
						include ('includes/vendors/listaClientes.php');
						echo('<p>EM CONTRUÇÃO</p>');
						break;
					case 'perf':
						$end = 'http://localhost/sinf/api/pfile.php';
						//$end = 'http://localhost:49174/api/perfil/';
						$resquest = callAPI($end);
						include ('includes/vendors/profile.php');
						echo('<p>EM CONTRUÇÃO</p>');
						break;
					default:
						echo('<h1>Opção Inválida</h1>');
						break;
				}
		}
		catch(Exception $e){
			$resquest = FALSE;
			echo("<p> Warning: Exception catch server probably offline!</p>");
		}	 
	?>
</div>
