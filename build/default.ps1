

Task Default -depends Build

Task Build {
   Exec { msbuild "..\source\az.contracts\az.contracts.sln" }

   Exec { msbuild "..\source\az.security\az.security.sln" }
   Exec { msbuild "..\source\az.gui\az.gui.sln" }
   Exec { msbuild "..\source\az.ironmqapi\az.ironmqapi.sln" }
   Exec { msbuild "..\source\az.serialization\az.serialization.sln" }
   Exec { msbuild "..\source\az.sqsapi\az.sqsapi.sln" }
   Exec { msbuild "..\source\az.tinyurlapi\az.tinyurlapi.sln" }
   Exec { msbuild "..\source\az.tweetstore\az.tweetstore.sln" }
   Exec { msbuild "..\source\az.twitterapi\az.twitterapi.sln" }

   Exec { msbuild "..\source\az.application\az.application.sln" }
   Exec { msbuild "..\source\az.cron.application\az.cron.application.sln" }
   Exec { msbuild "..\source\az.receiver.application\az.receiver.application.sln" }
   Exec { msbuild "..\source\az.publisher.application\az.publisher.application.sln" }
}
