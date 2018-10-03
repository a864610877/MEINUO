select sum(t.amount) amount, t.ownerid, t.submitdate 
from CashDealLogs t 
where (@SubmitTimeMin is null or t.submitdate >= @SubmitTimeMin)
and  (@SubmitTimeMax is null or t.submitdate < @SubmitTimeMax)
and  (@OwnerId is null or t.OwnerId = @OwnerId)
group by t.submitdate, t.ownerid