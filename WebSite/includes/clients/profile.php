<?php
 //TODO buscar infos ao primavera	
 $request = array("name" => "João Antunes","id" => "1","email" => "joao@gmail.com","telefone" => "00351-912345678","nif" => "123456789","datanascimento" => "1970-12-31","morada" => "Rua dos Joaquins - lote 25 3500-290 Viseu");
?> 
<h1>Ficha de cliente - <?php echo ($request['name']);?></h1>
<p><label for="#">ID</label><input type="text" name="id" id="id" value='<?php echo ($request["id"]);?>'disabled></p>
<p><label for="#">Nome</label><input type="text" name="nome" id="nome" value='<?php echo ($request["name"]);?>' disabled></p>
<p><label for="#">E-mail</label><input type="email" name="email" id="id" value='<?php echo ($request["email"]);?>' disabled></p>
<p><label for="#">Telefone</label><input type="tel" name="telefone" id="telefone" value='<?php echo( $request["telefone"]);?>' disabled></p>
<p><label for="#">NIF</label><input type="number" name="nif" id="nif" value='<?php echo ($request["nif"]);?>' disabled></p>
<p><label for="#">Data Nascimento</label><input type="date" name="id" id="id" value='<?php echo ($request["datanascimento"]);?>' disabled></p>
<p><label for="#">Morada</label><textarea rows="40" cols="40" readonly><?php echo ($request["morada"]);?></textarea></p>
<p><label for="#">Últimas Encomendas</label></p>