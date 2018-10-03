select * from accounttypes where 
(@displayName is null or displayName = @displayName)
and (@state is null or state = @state) 