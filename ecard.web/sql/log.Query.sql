select * from logs where  
(@LogType is null or LogType = @LogType)  
and (@ContentWith is null or Content like '%' + @ContentWith + '%')  
and (@UserName is null or UserName = @UserName)  