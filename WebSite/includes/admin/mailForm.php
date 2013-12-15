
	<form id="formMail" action="sendMail.php" method="post"> 

	<?php 
		if(isset($_GET['email']))
			echo "<script type='text/javascript'>alert('Email enviado!');</script>";	
	?>

	<h2>Contactar</h2>

	<p>
		<label>Nome:</label>
		<input name="name" type="text" id="name" size="32">
	</p>
	<p>
		<label>Email:</label>
		<input name="email" type="text" id="email" size="32">
	</p>
	<p>
		<label>Mensagem:</label>
		<br>
		<textarea name="comment" cols="45" rows="6" id="comment" class="bodytext"></textarea>
	</p>
	<p>
		<input type="submit" name="Submit" value="Enviar">
	</p>
	
	<input type="hidden" name="vendorMail" value="<?php echo $request['Email'] ?>">
	<input type="hidden" name="urlPage" value="<?php echo $_SERVER['REQUEST_URI'] ?>">
	</form>