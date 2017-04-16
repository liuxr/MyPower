<?php

$key1='keyjingcai2l.8ke520lhJin.Cai@ss283.229808e';
function encrypt($input,$key1){//Êý¾Ý¼ÓÃÜ
		$size = mcrypt_get_block_size(MCRYPT_3DES,'ecb');
		echo $size;
		echo "</br>";
		$input = pkcs5_pad($input, $size);
		echo $input;
		echo "</br>";
		$key = str_pad($key1,24,'0');
		echo $key;
		echo "</br>";
		$td = mcrypt_module_open(MCRYPT_3DES, '', 'ecb', '');
echo $td;
		echo "</br>";
		$iv = @mcrypt_create_iv (mcrypt_enc_get_iv_size($td), MCRYPT_RAND);
		@mcrypt_generic_init($td, $key, $iv);
		$data = mcrypt_generic($td, $input);
		echo $data;
		echo "</br>";
		mcrypt_generic_deinit($td);
		mcrypt_module_close($td);
		//   $data = base64_encode($this->PaddingPKCS7($data));
		$data = urlsafe_b64encode($data);
		return $data;
	}

function pkcs5_pad ($text, $blocksize) {
		$pad = $blocksize - (strlen($text) % $blocksize);
		echo str_repeat(chr($pad), $pad);
		echo "</br>";
		return $text . str_repeat(chr($pad), $pad);
	}

	function urlsafe_b64encode($string) {    
		$data = base64_encode($string);  
		echo $data;
		echo "</br>";

		$data = str_replace(array('+','/','='),array('-','_',''),$data);    
		return $data; 
	}


/*print_r ($key);*/

$input='{"account":"dejin6334@163.com"}';
/*print_r ($input);*/
//echo mcrypt_get_block_size('tripledes', 'ecb');
echo "</br>";
echo encrypt($input,$key1);
echo "</br>";
?>