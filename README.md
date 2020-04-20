# aspdotnet-3-startup-logging

ASP.NET Core 3.0 doesn't support logging in the Startup constructor or Startup.ConfigureServices as referenced in the following;  
[create-logs-in-the-startup-class](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/logging/?view=aspnetcore-3.0#create-logs-in-the-startup-class) 

After reading Christian Riedl's [solution]([LoggerBuffered](https://stackoverflow.com/questions/41287648/how-do-i-write-logs-from-within-startup-cs/60833884#60833884) on StackOverflow I adopted it.  Later you will see that it works well when I embed the asp.net webapp inside an Azure Function.  

## The Solution
1. Create an in-memmory ILogger that buffers the logs until we can hand them over to the real ILogger.  
2. Defer any exception thrown before *`Startup.Configure`* to be thrown after we copy the in-memmory log to the real ILogger.  


# Credits  
[LoggerBuffered](https://stackoverflow.com/questions/41287648/how-do-i-write-logs-from-within-startup-cs/60833884#60833884)  
[Christian Riedl](https://stackoverflow.com/users/1165242/christian-riedl)
