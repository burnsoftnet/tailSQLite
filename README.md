# tailSQLite
***

This is an attempt to create an application that will act like a regular tail but for sqlite. This will allow you to pass the SQLite database and the table that you want to watch and have the program refresh every few seconds.

## Parameters
<br/>
<table>
<tr>
<th>Switch</th>
<th>Description</th>
</tr>
<tr>
<td>-help or -h</td>
<td>Display Help</td>
</tr>
<tr>
<td>tail</td>
<td>Tell the app to be in tail mode, Requires the db, table and idcol parameters, the t parameter is optional </td>
</tr>
<tr>
<td>db=DatabaseName</td>
<td>The path and name of the database you want to follow</td>
</tr>
<tr>
<td>t=seconds</td>
<td>the number of seconds you want to refresh the trace, by default this will be 5 seconds</td>
</tr>
<tr>
<td>table</td>
<td>The table name that you want to trace</td>
</tr>
<tr>
<td>idol</td>
<td>the Identity column to the table that you want to tail</td>
</tr>
<tr>
<td>showtables</td>
<td>Show all the tables of the database, requires the db parameter</td>
</tr>
<tr>
<td>showcolumns</td>
<td>Show all the columns for the table requires the table & db parameters</td>
</tr>
<tr>
<td>debug</td>
<td>Display Debug messages, currently this is just displaying on the query when running the tail.</td>
</tr>
</table>

## Examples

**start tailing:**
tailSQlite -table=process_stats_main -idcol=id -db=C:\BSAP\bsap_client.db /t=5 /tail

**Show Tables**

tailSQlite --db=C:\BSAP\bsap_client.db /showtables

**Show Columns**

tailSQlite --db=C:\BSAP\bsap_client.db /table=process_stats_main /showcolumns



## Release Notes

Version 1.0.0 - Fresh Release
