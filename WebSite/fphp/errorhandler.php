<?PHP
set_error_handler(
	function($severity, $message, $file, $line) { 
		throw new ErrorException($message, $severity, $severity, $file, $line); 
		});
?>