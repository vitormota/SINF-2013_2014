<?php
    session_start();
	if(isset($_SESSION['user_id']))
		unset($_SESSION['user_id']);
	if(isset($_SESSION['type_id']))
		unset($_SESSION['type_id']);
    session_destroy(); // force clear!
	header("location: ../index.php");
?>