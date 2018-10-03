select '' AccoutTypeName, 
	(select count(*) from logs inner join accounts acc on acc.accountid = addin where LogType = @LogType and (@start is null or @start <= submittime) and (@end is null or @end > submittime)) [Count]
	from accountTypes accType
	where accType.State  = 1
	