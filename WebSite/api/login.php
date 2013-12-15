<?php
if(isset($_GET['username']) && isset($_GET['password']))
{
	$u =$_GET['username'];
	$p =$_GET['password'];
	if($u == "admin" && $p == "admin")
	{
		echo (json_encode(array('type' => '1')));
	}
	else
	{
		if($u == "vendedor" && $p == "vendedor")
			echo (json_encode(array('type' => '2')));
		else
			if($u == "cliente" && $p == "cliente")
				echo (json_encode(array('type' => '3','name' => 'João Ferreira'))); 
			else	
				echo (json_encode(array('type' => '0')));	
	}
}
else{
	echo (json_encode(array('type' => '0')));
	}
?>