select month, AccountName, dealAmount, cancelAmount from (
		select convert(varchar(50), DatePart(yyyy, submittime)) + '-' + convert(varchar(50), DatePart(mm, submittime)) as month, accountId,
        accountName, sum(amount) dealamount from deallogs 
        where (state <> 3) and submittime >= @start and submittime < @end
        group by accountName, convert(varchar(50), DatePart(yyyy, submittime)) + '-' + convert(varchar(50), DatePart(mm, submittime)), accountId
) v

where (@Month is null or @Month = month) and (@AccountName is null or AccountName = @AccountName)
group by month