<?php

/* 
	The following variables need to be changed to your webserver details
	Ensure the variables names themselves remain the same though
*/

$db_name = ‘databasename’; // Database name 
$username = ‘username; //Your database username
$password = ‘password’; //Your database password (for logging in with above username)
$host = 'localhost'; // hostedAddress - in the form of a link if on a website?
$tbl_name = ‘tablename; // Table name in which is being accessed - i,e "gamescoreboard"

$dblink = mysql_connect($host,$username,$password);
$seldb = mysql_select_db($db_name);

if(isset($_POST['name']) && isset($_POST['score']))
{
     // sanitize the GET's to help prevent SQL injection or attacks - possibly not 100% safe, but can help! 
     $name = strip_tags(mysql_real_escape_string($_POST['name']));
     $score = strip_tags(mysql_real_escape_string($_POST['score']));
     
     $sql = mysql_query("INSERT INTO `$db_name`.`$tbl_name` (`name`,`score`) VALUES (\"$name\", $score );");
     
     if($sql)
	 {
          //The query returned true, we're done here!
          echo 'Your score was saved to the database, we are done here!';
     }
	 else
	 {
          //The query returned false, this isn't good! Log report to text file for further analysis?
          echo 'Your score could not be saved, if this continues please check your database or contact the developer :)';
          echo mysql_errno($dblink) . ": " . mysql_error($dblink). "\n";
     }
}
else
{
     echo 'Your name or score wasnt passed in the request. Make sure the values were passed in from C# correctly.';
}

// close the SQL database link, so resources are saved and constant communication between the application and database host doesn't affect use
mysql_close($dblink);

?>