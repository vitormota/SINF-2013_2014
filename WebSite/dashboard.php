<?php
	//require_once('fphp/errorhandler.php');
	session_start();

	if(!isset($_SESSION['user_id']))
	{
		header("location: index.php");
	}
	else
	{
		if(isset($_GET['menu']))
			$menu = $_GET['menu'];
			
		require_once('fphp/resquests.php');
	}
?>
<!DOCTYPE html>
<html>
	<head>
		<title>Painel de Utilizador</title>
		<meta charset="UTF-8">
		<script type="text/javascript" src="./scripts/jquery-1.10.2.js"></script>
		<script type="text/javascript" src="./scripts/scripts.js"></script>
		<link rel="stylesheet" href="./css/dashboard.css">
	</head>
	<body>
	<div id="mainframe">
		<?php 
			//TODO finalizar os outros menus
			switch($_SESSION['type_id']){
				case '1':
					include ('includes/admin/header.php');
					include ('includes/admin/body.php');
					break;
				case '2':
					include ('includes/vendors/header.php');
					include ('includes/vendors/body.php');
					break;
				case '3':
					include ('includes/clients/header.php');
					include ('includes/clients/body.php');
					break;
				default:
					echo ("<h1>Utilizador inválida, contacte um administrador!</h1>");
					break;
			}
		?>
	</div>

	</body>
<html>
	