select t.* from shopdeallogs t where 
(@State is null or t.State = @State)
and (@StartDateMin is null or t.StartDate >= @StartDateMin) 
and (@EndDateMax is null or t.EndDate <= @EndDateMax) 
and (@HowToDeal is null or t.HowToDeal = @HowToDeal) 
and (dealamount <> 0 or cancelamount <> 0)