<?php
	$ToEmail = $_POST['vendorMail'];
	$EmailSubject = 'Contacto de Utilizador';
	$mailheader = "From: ".$_POST["email"]."\r\n";
	$mailheader .= "Reply-To: ".$_POST["email"]."\r\n";
	$mailheader .= "Content-type: text/html; charset=utf8\r\n";
	$MESSAGE_BODY = "Name: ".$_POST["name"]."";
	$MESSAGE_BODY .= "Email: ".$_POST["email"]."";
	$MESSAGE_BODY .= "Comment: ".nl2br($_POST["comment"])."";
	mail($ToEmail, $EmailSubject, $MESSAGE_BODY, $mailheader);
	header("location: ".$_POST['urlPage']."&email=success");
?>