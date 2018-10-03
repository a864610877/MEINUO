select accType.DisplayName as AccoutTypeName, 
	(select count(*) from logs inner join accounts acc on acc.accountid = addin where LogType = @LogType and acc.accountTypeId = accType.accountTypeId and (@start is null or @start <= submittime) and (@end is null or @end > submittime)) [Count]
	from accountTypes accType
	where accType.State  = 1
	