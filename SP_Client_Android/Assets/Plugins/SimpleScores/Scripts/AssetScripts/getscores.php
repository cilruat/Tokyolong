<?php

error_reporting(E_ALL);
ini_set('display_errors', 1);

header('Access-Control-Allow-Origin: *');

/* 
	The following variables need to be changed to your webserver details
	Ensure the variables names themselves remain the same though
*/

$db_name = ‘databasename’; // Database name 
$username = ‘username; //Your database username
$password = ‘password’; //Your database password (for logging in with above username)
$host = 'localhost'; // localhost is this script is on the same server as your database
$tbl_name = ‘tablename; // Table name in which is being accessed - i,e "gamescoreboard"

$clearData = $_POST['clearData'];

// Connect to server and select database.
$dblink = mysql_connect($host, $username, $password); 
$seldb = mysql_select_db($db_name);

// Retrieve data from database 

if ($clearData == 0)
{
	$result = mysql_query("SELECT * FROM $tbl_name");
	
	while($rows = mysql_fetch_array($result))
	{
		// loops through the values and returns them to the C# as "name,value<br>"
		echo $rows['name'] . "," . $rows['score'] . "<br>";
	}
}

if ($clearData == 1)
{
	$sql = mysql_query("DELETE FROM $tbl_name");
	
	if ($sql)
	{
		echo "All data removed from table: " . $tbl_name;
	}
	else
	{
		echo "Data was not removed from table: " . $tbl_name;
	}
}

// close the sql connection to prevent wasted resource and constant communication between process and server
mysql_close();

?>