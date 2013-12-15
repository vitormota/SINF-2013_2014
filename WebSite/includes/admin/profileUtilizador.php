<?php
if($request['Tipo'] == '3')
{
	?>

		<h1>Ficha de Cliente</h1>
		<p><label for="#">ID</label><input type="text" name="id" id="id" value='<?php echo ($request["Cod"]);?>'disabled></p>
		<p><label for="#">Nome</label><input type="text" name="nome" id="nome" value='<?php echo ($request["Nome"]);?>' disabled></p>
		<!-- <p><label for="#">E-mail</label><input type="email" name="email" id="id" value='<?php echo ($request["Email"]);?>' disabled></p> -->
		<p><label for="#">Telefone</label><input type="tel" name="telefone" id="telefone" value='<?php echo( $request["Telefone"]);?>' disabled></p>
		<p><label for="#">Fax</label><input type="tel" name="telefone" id="telefone" value='<?php echo( $request["Fax"]);?>' disabled></p>
		<p><label for="#">NIF</label><input type="number" name="nif" id="nif" value='<?php echo ($request["NumContribuinte"]);?>' disabled></p>
		<p><label for="#">País</label><input type="text" name="id" id="id" value='<?php echo ($request["Pais"]);?>' disabled></p>
		<p><label for="#">Morada</label><input type="text" name="telefone" id="telefone" value='<?php echo( $request["Morada"]);?>' disabled></p>
		<p><label for="#">Localidade</label><input type="text" name="telefone" id="telefone" value='<?php echo( $request["Localidade"]);?>' disabled></p>
		<p><label for="#">Código Postal</label><input type="text" name="telefone" id="telefone" value='<?php echo( $request["CP"]);?>' disabled></p>
		<p><label for="#">Cidade</label><input type="text" name="telefone" id="telefone" value='<?php echo($request["CPLocal"]);?>' disabled></p>
		<p><label for="#">Moeda Usada</label><input type="text" name="telefone" id="telefone" value='<?php echo( $request["Moeda"]);?>' disabled></p>

	<?php
}
else
{
	?>

		<h1>Ficha de Utilizador - <?php echo ($request['Tipo'] == '2') ? 'Vendedor' : 'Administrador' ;?></h1>
		<p><label for="#">ID</label><input type="text" name="id" id="id" value='<?php echo ($request["Cod"]);?>'disabled></p>
		<p><label for="#">Nome</label><input type="text" name="nome" id="nome" value='<?php echo ($request["Nome"]);?>' disabled></p>
		<p><label for="#">E-mail</label><input type="email" name="email" id="id" value='<?php echo ($request["Email"]);?>' disabled></p>
		<p><label for="#">Telefone</label><input type="tel" name="telefone" id="telefone" value='<?php echo( $request["Telefone"]);?>' disabled></p>


	<?php

	include_once("mailForm.php");
}
 
?> 
