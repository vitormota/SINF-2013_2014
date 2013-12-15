<?PHP 
	//require_once('fphp/errorhandler.php');
	session_start();
	if(isset($_SESSION['user_id']))
		header("location: dashboard.php");
	else{
		if(isset($_POST['username']) && isset($_POST['password'])){
			try {
			//TODO verificar se o caminho está certo!
				$resquest = @file_get_contents('http://localhost:49174/api/login?username='.$_POST["username"].'&password='.$_POST["password"]);
				//$resquest = @file_get_contents('http://gnomo.fe.up.pt/~ei11022/cadeiras/sinf/api/login.php?username='.$_POST["username"].'&password='.$_POST["password"]); //TODO eliminar testes
					//print_r($resquest);
					$resquest = json_decode($resquest, true);
					$type = $resquest['type'];
					//echo("tipo: ".$type);
					if(($type == "1") || ($type == "2"))
					{
						$_SESSION['user_id'] = $_POST['username'];	
						$_SESSION['type_id'] = $type;
						header("location: dashboard.php?menu=enc");
					}
					else 
						if($type == "3")
						{
						//TODO ver ao certo como é para fazer em caso de um cliente.
						$_SESSION['user_id'] = $_POST['username'];
						$_SESSION['guest_name'] = $resquest['name'];
						$_SESSION['type_id'] = $type;
						header("location: dashboard.php?menu=enc");
						
						}
						else{
							$msg = "Username or password invalid!";
							session_destroy();
						}
					
				}
			catch(Exception $e){
				$msg = "Server OFFLINE. Please try again later or contact admin!";
				echo("Warning: Exception catch server probably offline!");
			}
		}
	}
?>
<!DOCTYPE html>
<html>
	<head>
		<title>Autenticação</title>
		<meta charset="UTF-8">
		<link rel="stylesheet" href="./css/login.css">
	</head>
	<body>
		<div id="mainframe">
			<form class="login" action="index.php"  method="post">
    		<p>
      			<label for="login">Nome:</label>
      			<input type="text" name="username" id="login" placeholder="Nome da conta" autocomplete required>
    		</p>
    		<p>
      			<label for="password">Password:</label>
      			<input type="password" name="password" id="password" placeholder="Senha para autenticação" required>
    		</p>
			<p class="login-submit">
      			<button type="submit" class="login-button" >Entrar</button>
    		</p>
			<?PHP 
				if(isset($msg))
				echo('<p class="warning">'.$msg.'</p>');
			?>
  			</form>
		</div>
	</body>
<html>