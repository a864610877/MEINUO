select t.* from deallogs t where
(@AccountId is null or t.AccountId = @AccountId)
and (@ShopId is null or t.ShopId = @ShopId)
and (@PosId is null or t.SourcePosId = @PosId)
and (@ShopName is null or t.ShopName = @ShopName)
and (@State is null or t.State = @State)
and (@SubmitTimeMax is null or t.SubmitTime < @SubmitTimeMax) 
and (@SubmitTimeMin is null or t.SubmitTime >= @SubmitTimeMin) 
and (@AccountName is null or t.AccountName = @AccountName)
and t.DealType in (1,2,7,8,9,10,102,105)
and (@DealType is null or t.DealType=@DealType)
and ishidden = 0