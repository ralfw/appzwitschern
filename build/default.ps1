function Build_all_sln {
  foreach($n in $args[0]) {
    $slnName = "..\source\$n\$n.sln"
    Exec { msbuild $slnName }
  }
}


Task Default -depends Build

Task Build {
   Build_all_sln "az.contracts", "az.serialization", "az.tweetstore", 
                 "az.tinyurlapi", "az.security", "az.twitterapi", 
                 "az.ironmqapi", "az.gui", "az.application", 
                 "az.receiver.application", "az.publisher.application", "az.cron.application"
}



