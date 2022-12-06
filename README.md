Tool to parse CSV log files (they must be seperated with ',') using custom queries.
Symbols "=" and "!=" mean contains and doesn't contain repspectively.

Basic usage information can be found by typing "help" command.

Query syntax:
When writting a query, first a word "query" has to be present followed by atleast one space.
Then query itself is written and IT DOES NOT COUNT any leading or trailing whitespace.
Lastly, output can be optionally redirected to a file. Use ">" and file path after query to do so.
Queries support 4 operations: "=" - contains, "!=" - doesn't contain, "&&" - logical AND and "||" - logical OR.
Using these all logical boolean operations can be written (using "!=" as NOT).
Special case is when <column> != <white_space> <end_of_query_or_&&/||>. Under normal circumstances, this would always return no results,
however, in this program, it will return any entry, that had ANYTHING in that column.

Valid queries:
query signatureId=4608
query signatureId = 4608
query signatureId!=4608&&severity=10||deviceProduct=Server&&deviceProduct!=2008
query signatureId != 4608 && severity = 10 || deviceProduct = Server && deviceProduct != 2008
query signatureId = 4608 > out.json
query signatureId!=&&severity=10 <- special case

Order of execution: 
	1. "=" and "!=" 
	2. "&&" AND
	3. "||" OR

Duplicates are removed using hash table.
Only for testing purposes, "use_default" command can be used to set input file as "20220601182758.csv".
Otherwise, "file <path>" command has to be used to set the input file. Column names are automatically detected for every file, if it is correct.
