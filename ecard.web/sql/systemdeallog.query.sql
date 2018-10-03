select t.* from systemdeallogs t where
(@DealType is null or t.DealType = @DealType)
and (@UserName is null or t.UserName = @UserName) 
and (@SubmitTimeMax is null or t.SubmitTime < @SubmitTimeMax) 
and (@SubmitTimeMin is null or t.SubmitTime >= @SubmitTimeMin) 
and (@SystemDealLogId is null or t.SystemDealLogId = @SystemDealLogId) 
and (@SerialNo is null or t.SerialNo = @SerialNo) 
and (@state is null or t.state = @state) 
