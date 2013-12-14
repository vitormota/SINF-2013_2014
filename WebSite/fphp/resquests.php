<?php 
function callAPI($quest){
		$r = @file_get_contents($quest);
		$r = json_decode($r, true);
		//print_r($r);
		return $r;
}
?>