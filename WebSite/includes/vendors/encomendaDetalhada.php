<?php
	 foreach ($request as $line) {
	 	if($line['docNum'] == $_GET['encID'])
	 		$encomenda = $line;
	 }
	if(!isset($encomenda))
	{
		echo "<h1>Encomenda não existe no sistema!</h1>";
	}
	else
	{
?> 
<h1>Encomenda - <?php echo ($encomenda['docNum']);?></h1>
<p><label for="#">Modo de Expedição</label><input type="text" name="modExp" id="modExp" value='<?php echo ($encomenda["modExpedicao"]);?>'disabled></p>

<p><label for="#">Cliente</label><input type="text" name="cliente" id="cliente" value='<?php echo ($encomenda["CodClient"]);?>' disabled></p>

<p><label for="#">Total de Mercadoria</label><input type="text" name="totMerc" id="totMerc" value='<?php echo ($encomenda["totalMerc"]);?>' disabled></p>

<p><label for="#">Total IVA</label><input type="text" name="iva" id="iva" value='<?php echo( $encomenda["totalIva"]);?>' disabled></p>

<p><label for="#">Data de Encomenda</label><input type="text" name="date" id="date" value='<?php echo ($encomenda["date"]);?>' disabled></p>

<p><label for="#">Modo de Pagamento</label><input type="text" name="modPag" id="modPag" value='<?php echo ($encomenda["modPag"]);?>' disabled></p>

<p><label for="#">Condições de Pagamento</label><input type="text" name="condPag" id="condPag" value='<?php echo ($encomenda["condPag"]);?>' disabled></p>

<p><label for="#">NIF</label><input type="text" name="nif" id="nif" value='<?php echo ($encomenda["numContrib"]);?>' disabled></p>

<p><label for="#">Moeda</label><input type="text" name="moeda" id="moeda" value='<?php echo ($encomenda["moeda"]);?>' disabled></p>

<p><label for="#">Estado de Encomenda</label><input type="text" name="status" id="status" value='<?php echo ($encomenda["estadoFact"]);?>' disabled></p>

<p><label for="#">Data de Última Modificação</label><input type="text" name="lastChange" id="lastChange" value='' disabled></p>

<?php } ?>